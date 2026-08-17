using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
using BLL_Hospital360.Auditoria;
using BLL_Hospital360.Catalogos;
using BLL_Hospital360.Citas;
using BLL_Hospital360.Reportes;
using BLL_Hospital360.Usuarios;
using DAL_Hospital360.Catalogos;
using DAL_Hospital360.Citas;
using DAL_Hospital360.Reportes;
using PL_Hospital360.Modelos;

namespace PL_Hospital360
{
    public partial class Citas : System.Web.UI.Page
    {
        protected string NombreUsuarioSesion = "Usuario";
        protected string InicialesUsuario = "US";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Guarda de sesión del lado servidor: si no hay sesión activa,
            // ni siquiera se sirve la página (independiente de lo que haga el JS).
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            NombreUsuarioSesion = Session["NombreCompleto"]?.ToString() ?? "Usuario";
            InicialesUsuario = Iniciales(NombreUsuarioSesion);
        }

        private static string Iniciales(string sNombre)
        {
            var partes = sNombre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return "US";
            if (partes.Length == 1) return partes[0].Substring(0, Math.Min(2, partes[0].Length)).ToUpper();
            return (partes[0][0].ToString() + partes[1][0]).ToUpper();
        }

        /// <summary>
        /// Llena de un solo viaje los 4 combos de filtro, usando los 4
        /// procedimientos sp_Listar* ya existentes en la BD.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static CatalogosCitasVM CargarCatalogos()
        {
            cls_Catalogos_BLL bll = new cls_Catalogos_BLL();
            string sTipoClinica = HttpContext.Current.Session["TipoClinica"]?.ToString() ?? string.Empty;

            return new CatalogosCitasVM
            {
                EstadosCita = ConvertirCombo(bll.CargaListaEstadosCita().dtDatos, "IdEstadoCita", "NombreEstado"),
                Profesionales = ConvertirCombo(bll.CargaListaProfesionales(sTipoClinica).dtDatos, "IdProfesional", "NombreCompleto"),
                TiposCita = ConvertirCombo(bll.CargaListaTiposCita(sTipoClinica).dtDatos, "IdTipoCita", "NombreTipoCita"),
                TiposMembresia = ConvertirCombo(bll.CargaListaTiposMembresia().dtDatos, "IdTipoMembresia", "NombreMembresia")
            };
        }

        /// <summary>
        /// Llama a sp_FiltrarCitas con los filtros que mande el JS. Todos
        /// los parámetros son opcionales (mandar null/"" = sin filtro).
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<CitaVM> FiltrarCitas(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            cls_Citas_DAL obj = new cls_Citas_DAL
            {
                iIdUsuario = ObtenerIdUsuarioSesion(),
                sNombrePaciente = string.IsNullOrWhiteSpace(sNombrePaciente) ? null : sNombrePaciente.Trim(),
                dFecha = string.IsNullOrWhiteSpace(sFecha) ? (DateTime?)null : DateTime.Parse(sFecha),
                iIdEstadoCita = iIdEstadoCita,
                iIdProfesional = iIdProfesional,
                iIdTipoCita = iIdTipoCita,
                iIdTipoMembresia = iIdTipoMembresia
            };

            cls_Citas_BLL bll = new cls_Citas_BLL();
            obj = bll.ListarFiltro(obj);

            List<CitaVM> lista = new List<CitaVM>();
            if (obj.dtDatos != null)
            {
                foreach (DataRow fila in obj.dtDatos.Rows)
                {
                    lista.Add(new CitaVM
                    {
                        IdCita = Convert.ToInt32(fila["IdCita"]),
                        NombrePaciente = fila["NombrePaciente"].ToString(),
                        Telefono = fila["Telefono"].ToString(),
                        NombreTipoCita = fila["NombreTipoCita"].ToString(),
                        NombreMembresia = fila["NombreMembresia"] == DBNull.Value ? "" : fila["NombreMembresia"].ToString(),
                        NombreEstado = fila["NombreEstado"].ToString(),
                        NombreProfesional = fila["NombreProfesional"].ToString(),
                        Fecha = Convert.ToDateTime(fila["Fecha"]).ToString("dd/MM/yyyy"),
                        Hora = fila["Hora"].ToString()
                    });
                }
            }

            return lista;
        }

        /// <summary>Registra una nueva cita (dbo.sp_InsertarCita) para el usuario actual.</summary>
        [WebMethod(EnableSession = true)]
        public static ResultadoSimpleVM CrearCita(string sNombrePaciente, string sTelefono, string sCorreo,
            string sFecha, string sHora, int iIdTipoCita, int? iIdTipoMembresia, int iIdEstadoCita,
            int iIdProfesional, string sObservaciones, string sFechaNacimiento)
        {
            cls_Citas_DAL obj = new cls_Citas_DAL
            {
                iIdUsuario = ObtenerIdUsuarioSesion(),
                sNombrePaciente = (sNombrePaciente ?? string.Empty).Trim(),
                sTelefono = (sTelefono ?? string.Empty).Trim(),
                sCorreo = (sCorreo ?? string.Empty).Trim(),
                dFecha = ParsearFecha(sFecha),
                dHora = ParsearHora(sHora),
                iIdTipoCita = iIdTipoCita,
                iIdTipoMembresia = iIdTipoMembresia,
                iIdEstadoCita = iIdEstadoCita,
                iIdProfesional = iIdProfesional,
                sObservaciones = (sObservaciones ?? string.Empty).Trim(),
                dFechaNacimiento = ParsearFecha(sFechaNacimiento)
            };

            cls_Citas_BLL bll = new cls_Citas_BLL();
            obj = bll.Insertar(obj);

            return new ResultadoSimpleVM { Exito = obj.sAXN == "EXITO", Mensaje = obj.sMSJError };
        }

        /// <summary>Actualiza una cita existente (dbo.sp_ActualizarCita) del usuario actual.</summary>
        [WebMethod(EnableSession = true)]
        public static ResultadoSimpleVM ActualizarCita(int iIdCita, string sNombrePaciente, string sTelefono,
            string sCorreo, string sFecha, string sHora, int iIdTipoCita, int? iIdTipoMembresia,
            int iIdEstadoCita, int iIdProfesional, string sObservaciones, string sFechaNacimiento)
        {
            cls_Citas_DAL obj = new cls_Citas_DAL
            {
                iIdCita = iIdCita,
                iIdUsuario = ObtenerIdUsuarioSesion(),
                sNombrePaciente = (sNombrePaciente ?? string.Empty).Trim(),
                sTelefono = (sTelefono ?? string.Empty).Trim(),
                sCorreo = (sCorreo ?? string.Empty).Trim(),
                dFecha = ParsearFecha(sFecha),
                dHora = ParsearHora(sHora),
                iIdTipoCita = iIdTipoCita,
                iIdTipoMembresia = iIdTipoMembresia,
                iIdEstadoCita = iIdEstadoCita,
                iIdProfesional = iIdProfesional,
                sObservaciones = (sObservaciones ?? string.Empty).Trim(),
                dFechaNacimiento = ParsearFecha(sFechaNacimiento)
            };

            cls_Citas_BLL bll = new cls_Citas_BLL();
            obj = bll.Actualizar(obj);

            return new ResultadoSimpleVM { Exito = obj.sAXN == "EXITO", Mensaje = obj.sMSJError };
        }

        /// <summary>Elimina una cita (dbo.sp_EliminarCita) del usuario actual.</summary>
        [WebMethod(EnableSession = true)]
        public static ResultadoSimpleVM EliminarCita(int iIdCita)
        {
            cls_Citas_BLL bll = new cls_Citas_BLL();
            cls_Citas_DAL obj = bll.Eliminar(iIdCita, ObtenerIdUsuarioSesion());

            return new ResultadoSimpleVM { Exito = obj.sAXN == "EXITO", Mensaje = obj.sMSJError };
        }

        /// <summary>Obtiene el detalle de una cita del usuario actual, para editarla.</summary>
        [WebMethod(EnableSession = true)]
        public static CitaDetalleVM ObtenerCita(int iIdCita)
        {
            cls_Citas_BLL bll = new cls_Citas_BLL();
            cls_Citas_DAL obj = bll.Obtener(iIdCita, ObtenerIdUsuarioSesion());

            if (obj.sAXN != "EXITO")
            {
                return new CitaDetalleVM { Encontrada = false };
            }

            return new CitaDetalleVM
            {
                Encontrada = true,
                IdCita = iIdCita,
                NombrePaciente = obj.sNombrePaciente,
                Telefono = obj.sTelefono,
                Correo = obj.sCorreo,
                Fecha = obj.dFecha.Value.ToString("yyyy-MM-dd"),
                Hora = FormatearHora(obj.dHora.Value),
                IdTipoCita = obj.iIdTipoCita.Value,
                IdTipoMembresia = obj.iIdTipoMembresia,
                IdEstadoCita = obj.iIdEstadoCita.Value,
                IdProfesional = obj.iIdProfesional.Value,
                Observaciones = obj.sObservaciones,
                FechaNacimiento = obj.dFechaNacimiento.Value.ToString("yyyy-MM-dd")
            };
        }

        /// <summary>Citas del usuario actual agrupadas por estado (dbo.sp_ReporteCitasPorEstado).</summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteCitasPorEstado(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            var filtro = ArmarFiltroReporte(sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteCitasPorEstado(ObtenerIdUsuarioSesion(), filtro);
            return ConvertirReporte(obj.dtDatos);
        }

        /// <summary>Citas del usuario actual agrupadas por tipo de atención (dbo.sp_ReporteCitasPorTipoCita).</summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteCitasPorTipoCita(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            var filtro = ArmarFiltroReporte(sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteCitasPorTipoCita(ObtenerIdUsuarioSesion(), filtro);
            return ConvertirReporte(obj.dtDatos);
        }

        /// <summary>Citas del usuario actual agrupadas por profesional (dbo.sp_ReporteCitasPorProfesional).</summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteCitasPorProfesional(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            var filtro = ArmarFiltroReporte(sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteCitasPorProfesional(ObtenerIdUsuarioSesion(), filtro);
            return ConvertirReporte(obj.dtDatos);
        }

        /// <summary>
        /// Citas del usuario actual agrupadas por mes (dbo.sp_ReporteCitasPorMes),
        /// con relleno de los últimos 12 meses sin datos para que la línea
        /// de tendencia se vea continua.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteCitasPorMes(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            var filtro = ArmarFiltroReporte(sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteCitasPorMes(ObtenerIdUsuarioSesion(), filtro);

            var conteos = new Dictionary<int, int>();
            if (obj.dtDatos != null)
            {
                foreach (DataRow fila in obj.dtDatos.Rows)
                {
                    conteos[Convert.ToInt32(fila["Anio"]) * 100 + Convert.ToInt32(fila["Mes"])] = Convert.ToInt32(fila["Cantidad"]);
                }
            }

            return RellenarUltimos12Meses(conteos);
        }

        /// <summary>
        /// Ingresos reales del usuario actual (basados en TipoMembresia.Precio),
        /// agrupados por mes (dbo.sp_ReporteIngresosPorMes), con relleno de
        /// los últimos 12 meses sin datos.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteIngresosPorMes(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            var filtro = ArmarFiltroReporte(sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteIngresosPorMes(ObtenerIdUsuarioSesion(), filtro);

            var montos = new Dictionary<int, int>();
            if (obj.dtDatos != null)
            {
                foreach (DataRow fila in obj.dtDatos.Rows)
                {
                    montos[Convert.ToInt32(fila["Anio"]) * 100 + Convert.ToInt32(fila["Mes"])] = Convert.ToInt32(fila["Ingreso"]);
                }
            }

            return RellenarUltimos12Meses(montos);
        }

        /// <summary>
        /// Citas del usuario actual por día, dentro de la semana actual
        /// (Lunes a Domingo), usando dbo.sp_ReporteCitasPorRango.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<ReporteItemVM> ReporteCitasSemanaActual(string sNombrePaciente,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            DateTime hoy = DateTime.Today;
            int iDiferenciaLunes = ((int)hoy.DayOfWeek + 6) % 7;
            DateTime lunes = hoy.AddDays(-iDiferenciaLunes);
            DateTime domingo = lunes.AddDays(6);

            var filtro = ArmarFiltroReporte(sNombrePaciente, null, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia);
            var obj = new cls_Reportes_BLL().ReporteCitasPorRango(ObtenerIdUsuarioSesion(), lunes, domingo, filtro);

            var conteos = new Dictionary<DateTime, int>();
            if (obj.dtDatos != null)
            {
                foreach (DataRow fila in obj.dtDatos.Rows)
                {
                    conteos[Convert.ToDateTime(fila["Fecha"]).Date] = Convert.ToInt32(fila["Cantidad"]);
                }
            }

            string[] nombresDias = { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };
            List<ReporteItemVM> lista = new List<ReporteItemVM>();
            for (int i = 0; i < 7; i++)
            {
                DateTime dia = lunes.AddDays(i);
                int iCantidad;
                conteos.TryGetValue(dia.Date, out iCantidad);
                lista.Add(new ReporteItemVM { Etiqueta = nombresDias[i], Cantidad = iCantidad });
            }
            return lista;
        }

        private static List<ReporteItemVM> RellenarUltimos12Meses(Dictionary<int, int> valoresPorClave)
        {
            List<ReporteItemVM> lista = new List<ReporteItemVM>();
            CultureInfo cultura = new CultureInfo("es-ES");
            DateTime cursor = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11);

            for (int i = 0; i < 12; i++)
            {
                int iClave = cursor.Year * 100 + cursor.Month;
                int iValor;
                valoresPorClave.TryGetValue(iClave, out iValor);

                lista.Add(new ReporteItemVM { Etiqueta = cursor.ToString("MMM yyyy", cultura), Cantidad = iValor });
                cursor = cursor.AddMonths(1);
            }
            return lista;
        }

        private static List<ReporteItemVM> ConvertirReporte(DataTable dt)
        {
            List<ReporteItemVM> lista = new List<ReporteItemVM>();
            if (dt == null) return lista;

            foreach (DataRow fila in dt.Rows)
            {
                lista.Add(new ReporteItemVM
                {
                    Etiqueta = fila["Etiqueta"].ToString(),
                    Cantidad = Convert.ToInt32(fila["Cantidad"])
                });
            }
            return lista;
        }

        /// <summary>
        /// Lista el historial de auditoría (dbo.sp_ListarAuditoria) del
        /// usuario en sesión, del más reciente al más antiguo.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<AuditoriaVM> ListarAuditoria()
        {
            cls_Auditoria_BLL bll = new cls_Auditoria_BLL();
            var obj = bll.Listar(ObtenerIdUsuarioSesion());

            List<AuditoriaVM> lista = new List<AuditoriaVM>();
            if (obj.dtDatos != null)
            {
                foreach (DataRow fila in obj.dtDatos.Rows)
                {
                    lista.Add(new AuditoriaVM
                    {
                        IdAuditoria = Convert.ToInt32(fila["IdAuditoria"]),
                        TablaAfectada = fila["TablaAfectada"].ToString(),
                        TipoAccion = fila["TipoAccion"].ToString(),
                        Descripcion = fila["Descripcion"] == DBNull.Value ? "" : fila["Descripcion"].ToString(),
                        FechaAccion = Convert.ToDateTime(fila["FechaAccion"]).ToString("dd/MM/yyyy HH:mm:ss")
                    });
                }
            }

            return lista;
        }

        /// <summary>Cierra sesión (sp_CerrarSesion) y limpia la Session de ASP.NET.</summary>
        [WebMethod(EnableSession = true)]
        public static ResultadoSimpleVM CerrarSesion()
        {
            int iIdUsuario = Convert.ToInt32(System.Web.HttpContext.Current.Session["IdUsuario"]);

            cls_Usuarios_BLL bll = new cls_Usuarios_BLL();
            var obj = bll.CerrarSesion(iIdUsuario);

            System.Web.HttpContext.Current.Session.Clear();

            return new ResultadoSimpleVM { Exito = obj.sAXN == "EXITO", Mensaje = obj.sMSJError };
        }

        private static int ObtenerIdUsuarioSesion()
        {
            object valor = HttpContext.Current.Session["IdUsuario"];
            if (valor == null)
            {
                throw new InvalidOperationException("No hay una sesión activa.");
            }
            return Convert.ToInt32(valor);
        }

        private static DateTime? ParsearFecha(string sFecha)
        {
            return string.IsNullOrWhiteSpace(sFecha)
                ? (DateTime?)null
                : DateTime.ParseExact(sFecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static TimeSpan? ParsearHora(string sHora)
        {
            if (string.IsNullOrWhiteSpace(sHora)) return null;
            return TimeSpan.ParseExact(sHora, new[] { "hh\\:mm", "hh\\:mm\\:ss" }, CultureInfo.InvariantCulture);
        }

        private static string FormatearHora(TimeSpan hora)
        {
            return hora.ToString(@"hh\:mm");
        }

        private static cls_FiltroReporte_DAL ArmarFiltroReporte(string sNombrePaciente, string sFecha,
            int? iIdEstadoCita, int? iIdProfesional, int? iIdTipoCita, int? iIdTipoMembresia)
        {
            return new cls_FiltroReporte_DAL
            {
                sNombrePaciente = string.IsNullOrWhiteSpace(sNombrePaciente) ? null : sNombrePaciente.Trim(),
                dFecha = string.IsNullOrWhiteSpace(sFecha) ? (DateTime?)null : DateTime.Parse(sFecha),
                iIdEstadoCita = iIdEstadoCita,
                iIdProfesional = iIdProfesional,
                iIdTipoCita = iIdTipoCita,
                iIdTipoMembresia = iIdTipoMembresia
            };
        }

        private static List<ItemComboVM> ConvertirCombo(DataTable dt, string sColId, string sColNombre)
        {
            List<ItemComboVM> lista = new List<ItemComboVM>();
            if (dt == null) return lista;

            foreach (DataRow fila in dt.Rows)
            {
                lista.Add(new ItemComboVM
                {
                    Id = Convert.ToInt32(fila[sColId]),
                    Nombre = fila[sColNombre].ToString(),
                    Precio = dt.Columns.Contains("Precio") ? Convert.ToDecimal(fila["Precio"]) : (decimal?)null
                });
            }
            return lista;
        }
    }
}