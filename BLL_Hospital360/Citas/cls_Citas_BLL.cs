using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Citas;
using System.Configuration;

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

        #endregion
    }
}
