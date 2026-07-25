using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using BLL_Hospital360.Catalogos;
using BLL_Hospital360.Citas;
using BLL_Hospital360.Usuarios;
using DAL_Hospital360.Catalogos;
using DAL_Hospital360.Citas;
using PL_Hospital360.Modelos;

namespace PL_Hospital360
{
    public partial class Citas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Guarda de sesión del lado servidor: si no hay sesión activa,
            // ni siquiera se sirve la página (independiente de lo que haga el JS).
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        /// <summary>
        /// Llena de un solo viaje los 4 combos de filtro, usando los 4
        /// procedimientos sp_Listar* ya existentes en la BD.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static CatalogosCitasVM CargarCatalogos()
        {
            cls_Catalogos_BLL bll = new cls_Catalogos_BLL();

            return new CatalogosCitasVM
            {
                EstadosCita = ConvertirCombo(bll.CargaListaEstadosCita().dtDatos, "IdEstadoCita", "NombreEstado"),
                Profesionales = ConvertirCombo(bll.CargaListaProfesionales().dtDatos, "IdProfesional", "NombreCompleto"),
                TiposCita = ConvertirCombo(bll.CargaListaTiposCita().dtDatos, "IdTipoCita", "NombreTipoCita"),
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

        private static List<ItemComboVM> ConvertirCombo(DataTable dt, string sColId, string sColNombre)
        {
            List<ItemComboVM> lista = new List<ItemComboVM>();
            if (dt == null) return lista;

            foreach (DataRow fila in dt.Rows)
            {
                lista.Add(new ItemComboVM
                {
                    Id = Convert.ToInt32(fila[sColId]),
                    Nombre = fila[sColNombre].ToString()
                });
            }
            return lista;
        }
    }
}