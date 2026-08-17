using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Auditoria;
using System.Configuration;

namespace BLL_Hospital360.Auditoria
{
    /// <summary>
    /// Contiene la lógica de negocio relacionada con el historial de
    /// auditoría (inicios/cierres de sesión y acciones dentro de la app)
    /// del sistema Hospital360.
    /// </summary>
    public class cls_Auditoria_BLL
    {
        #region Métodos públicos

        /// <summary>
        /// Lista el historial de auditoría del usuario mediante
        /// dbo.sp_ListarAuditoria, ordenado del más reciente al más antiguo.
        /// </summary>
        public cls_Auditoria_DAL Listar(int iIdUsuario)
        {
            cls_Auditoria_DAL obj_Auditoria_DAL = new cls_Auditoria_DAL { iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Auditoria_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Auditoria_DAL.dtParametros);

            // Código 1 = INT.
            obj_Auditoria_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Listar_Auditoria"];
            obj_BD_DAL.DT_Parametros = obj_Auditoria_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "Auditoria";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Auditoria_DAL.sMSJError = string.Empty;
                obj_Auditoria_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Auditoria_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Auditoria_DAL.dtDatos = null;
            }

            return obj_Auditoria_DAL;
        }

        #endregion
    }
}
