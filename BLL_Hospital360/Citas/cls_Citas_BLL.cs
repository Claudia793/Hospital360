using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Citas;
using System;
using System.Configuration;
using System.Data;

namespace BLL_Hospital360.Citas
{
    /// <summary>
    /// Contiene la lógica de negocio relacionada con las citas
    /// del sistema Hospital360.
    /// </summary>
    public class cls_Citas_BLL
    {
        #region Métodos públicos

        /// <summary>
        /// Filtra las citas mediante dbo.sp_FiltrarCitas. Todos los
        /// filtros son opcionales; los valores nulos/vacíos se envían
        /// como DBNull para que el procedimiento los ignore.
        /// </summary>
        public cls_Citas_DAL ListarFiltro(cls_Citas_DAL obj_Citas_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Citas_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Citas_DAL.dtParametros);

            // Códigos de tipo de dato: 6 = VARCHAR, 8 = DATETIME, 1 = INT.
            obj_Citas_DAL.dtParametros.Rows.Add("@IdUsuario", "1", obj_Citas_DAL.iIdUsuario.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@NombrePaciente", "6", obj_Citas_DAL.sNombrePaciente);
            obj_Citas_DAL.dtParametros.Rows.Add("@Fecha", "8", obj_Citas_DAL.dFecha.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdEstadoCita", "1", obj_Citas_DAL.iIdEstadoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdProfesional", "1", obj_Citas_DAL.iIdProfesional.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoCita", "1", obj_Citas_DAL.iIdTipoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoMembresia", "1", obj_Citas_DAL.iIdTipoMembresia.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Filtrar_Citas"];
            obj_BD_DAL.DT_Parametros = obj_Citas_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "Citas";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Citas_DAL.sAXN = "EXITO";
                obj_Citas_DAL.sMSJError = string.Empty;
                obj_Citas_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Citas_DAL.dtDatos = null;
            }

            return obj_Citas_DAL;
        }

        /// <summary>
        /// Registra una nueva cita mediante dbo.sp_InsertarCita e interpreta
        /// el código Resultado (éxito, choque de horario, FK inválida,
        /// validación, inactivo, excepción).
        /// </summary>
        public cls_Citas_DAL Insertar(cls_Citas_DAL obj_Citas_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Citas_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Citas_DAL.dtParametros);

            // Códigos de tipo de dato: 6 = VARCHAR, 8 = DATETIME, 1 = INT, 12 = TIME.
            obj_Citas_DAL.dtParametros.Rows.Add("@NombrePaciente", "6", obj_Citas_DAL.sNombrePaciente);
            obj_Citas_DAL.dtParametros.Rows.Add("@Telefono", "6", obj_Citas_DAL.sTelefono);
            obj_Citas_DAL.dtParametros.Rows.Add("@Correo", "6", obj_Citas_DAL.sCorreo);
            obj_Citas_DAL.dtParametros.Rows.Add("@Fecha", "8", obj_Citas_DAL.dFecha.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@Hora", "12", obj_Citas_DAL.dHora.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoCita", "1", obj_Citas_DAL.iIdTipoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoMembresia", "1", obj_Citas_DAL.iIdTipoMembresia.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdEstadoCita", "1", obj_Citas_DAL.iIdEstadoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdProfesional", "1", obj_Citas_DAL.iIdProfesional.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdUsuario", "1", obj_Citas_DAL.iIdUsuario.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@Observaciones", "6", obj_Citas_DAL.sObservaciones);
            obj_Citas_DAL.dtParametros.Rows.Add("@FechaNacimiento", "8", obj_Citas_DAL.dFechaNacimiento.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Insert_Citas"];
            obj_BD_DAL.DT_Parametros = obj_Citas_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "InsertarCita";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Citas_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Citas_DAL;
            }

            int iResultado = Convert.ToInt32(obj_BD_DAL.DS.Tables[0].Rows[0]["Resultado"]);

            switch (iResultado)
            {
                case 1:
                    obj_Citas_DAL.sAXN = "EXITO";
                    obj_Citas_DAL.sMSJError = string.Empty;
                    break;

                case -1:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ya existe una cita agendada para ese profesional en esa fecha y hora.";
                    break;

                case -2:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Uno de los datos seleccionados (tipo de cita, membresía, estado o profesional) no es válido.";
                    break;

                case -3:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Debe completar todos los campos obligatorios con un formato válido (correo válido, fecha de nacimiento correcta y fecha de la cita no puede ser en el pasado).";
                    break;

                case -4:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "El profesional o el usuario se encuentra inactivo.";
                    break;

                case -5:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error interno al registrar la cita. Intente nuevamente.";
                    break;

                default:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error al registrar la cita.";
                    break;
            }

            return obj_Citas_DAL;
        }

        /// <summary>
        /// Actualiza una cita existente mediante dbo.sp_ActualizarCita.
        /// El procedimiento solo actualiza si la cita pertenece al
        /// usuario actual (aislamiento por IdUsuario).
        /// </summary>
        public cls_Citas_DAL Actualizar(cls_Citas_DAL obj_Citas_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Citas_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Citas_DAL.dtParametros);

            obj_Citas_DAL.dtParametros.Rows.Add("@IdCita", "1", obj_Citas_DAL.iIdCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@NombrePaciente", "6", obj_Citas_DAL.sNombrePaciente);
            obj_Citas_DAL.dtParametros.Rows.Add("@Telefono", "6", obj_Citas_DAL.sTelefono);
            obj_Citas_DAL.dtParametros.Rows.Add("@Correo", "6", obj_Citas_DAL.sCorreo);
            obj_Citas_DAL.dtParametros.Rows.Add("@Fecha", "8", obj_Citas_DAL.dFecha.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@Hora", "12", obj_Citas_DAL.dHora.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoCita", "1", obj_Citas_DAL.iIdTipoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdTipoMembresia", "1", obj_Citas_DAL.iIdTipoMembresia.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdEstadoCita", "1", obj_Citas_DAL.iIdEstadoCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdProfesional", "1", obj_Citas_DAL.iIdProfesional.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdUsuario", "1", obj_Citas_DAL.iIdUsuario.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@Observaciones", "6", obj_Citas_DAL.sObservaciones);
            obj_Citas_DAL.dtParametros.Rows.Add("@FechaNacimiento", "8", obj_Citas_DAL.dFechaNacimiento.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Update_Citas"];
            obj_BD_DAL.DT_Parametros = obj_Citas_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "ActualizarCita";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Citas_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Citas_DAL;
            }

            int iResultado = Convert.ToInt32(obj_BD_DAL.DS.Tables[0].Rows[0]["Resultado"]);

            switch (iResultado)
            {
                case 1:
                    obj_Citas_DAL.sAXN = "EXITO";
                    obj_Citas_DAL.sMSJError = string.Empty;
                    break;

                case 0:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "La cita no existe o no pertenece al usuario actual.";
                    break;

                case -1:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ya existe una cita agendada para ese profesional en esa fecha y hora.";
                    break;

                case -2:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Uno de los datos seleccionados (tipo de cita, membresía, estado o profesional) no es válido.";
                    break;

                case -3:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Debe completar todos los campos obligatorios con un formato válido (correo válido y fecha de nacimiento correcta).";
                    break;

                case -4:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "El profesional o el usuario se encuentra inactivo.";
                    break;

                case -5:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error interno al actualizar la cita. Intente nuevamente.";
                    break;

                default:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error al actualizar la cita.";
                    break;
            }

            return obj_Citas_DAL;
        }

        /// <summary>
        /// Elimina una cita mediante dbo.sp_EliminarCita. Solo elimina
        /// si la cita pertenece al usuario actual (aislamiento por IdUsuario).
        /// </summary>
        public cls_Citas_DAL Eliminar(int iIdCita, int iIdUsuario)
        {
            cls_Citas_DAL obj_Citas_DAL = new cls_Citas_DAL { iIdCita = iIdCita, iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Citas_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Citas_DAL.dtParametros);

            obj_Citas_DAL.dtParametros.Rows.Add("@IdCita", "1", iIdCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Delete_Citas"];
            obj_BD_DAL.DT_Parametros = obj_Citas_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "EliminarCita";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Citas_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Citas_DAL;
            }

            int iResultado = Convert.ToInt32(obj_BD_DAL.DS.Tables[0].Rows[0]["Resultado"]);

            switch (iResultado)
            {
                case 1:
                    obj_Citas_DAL.sAXN = "EXITO";
                    obj_Citas_DAL.sMSJError = string.Empty;
                    break;

                case 0:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "La cita no existe o no pertenece al usuario actual.";
                    break;

                case -5:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error interno al eliminar la cita.";
                    break;

                default:
                    obj_Citas_DAL.sAXN = "ERROR";
                    obj_Citas_DAL.sMSJError = "Ocurrió un error al eliminar la cita.";
                    break;
            }

            return obj_Citas_DAL;
        }

        /// <summary>
        /// Obtiene el detalle completo de una cita mediante dbo.sp_ObtenerCita,
        /// para prellenar el formulario de edición. Solo devuelve la cita
        /// si pertenece al usuario actual (aislamiento por IdUsuario).
        /// </summary>
        public cls_Citas_DAL Obtener(int iIdCita, int iIdUsuario)
        {
            cls_Citas_DAL obj_Citas_DAL = new cls_Citas_DAL { iIdCita = iIdCita, iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Citas_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Citas_DAL.dtParametros);

            obj_Citas_DAL.dtParametros.Rows.Add("@IdCita", "1", iIdCita.ToString());
            obj_Citas_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Info_Cita"];
            obj_BD_DAL.DT_Parametros = obj_Citas_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "ObtenerCita";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Citas_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Citas_DAL;
            }

            DataRow fila = obj_BD_DAL.DS.Tables[0].Rows[0];
            int iResultado = Convert.ToInt32(fila["Resultado"]);

            if (iResultado == 1)
            {
                obj_Citas_DAL.sAXN = "EXITO";
                obj_Citas_DAL.sMSJError = string.Empty;
                obj_Citas_DAL.sNombrePaciente = fila["NombrePaciente"].ToString();
                obj_Citas_DAL.sTelefono = fila["Telefono"].ToString();
                obj_Citas_DAL.sCorreo = fila["Correo"].ToString();
                obj_Citas_DAL.dFecha = Convert.ToDateTime(fila["Fecha"]);
                obj_Citas_DAL.dHora = (TimeSpan)fila["Hora"];
                obj_Citas_DAL.dFechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]);
                obj_Citas_DAL.iIdTipoCita = Convert.ToInt32(fila["IdTipoCita"]);
                obj_Citas_DAL.iIdTipoMembresia = fila["IdTipoMembresia"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["IdTipoMembresia"]);
                obj_Citas_DAL.iIdEstadoCita = Convert.ToInt32(fila["IdEstadoCita"]);
                obj_Citas_DAL.iIdProfesional = Convert.ToInt32(fila["IdProfesional"]);
                obj_Citas_DAL.sObservaciones = fila["Observaciones"] == DBNull.Value ? string.Empty : fila["Observaciones"].ToString();
            }
            else
            {
                obj_Citas_DAL.sAXN = "ERROR";
                obj_Citas_DAL.sMSJError = "La cita no existe o no pertenece al usuario actual.";
            }

            return obj_Citas_DAL;
        }

        #endregion
    }
}
