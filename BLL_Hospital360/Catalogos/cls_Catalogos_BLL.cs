using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Catalogos;
using System.Configuration;
using System.Data;

namespace BLL_Hospital360.Catalogos
{
    /// <summary>
    /// Contiene la lógica de negocio para cargar los catálogos
    /// utilizados como filtros en la pantalla de Citas.
    /// </summary>
    public class cls_Catalogos_BLL
    {
        #region Métodos públicos

        /// <summary>Carga el catálogo de estados de cita (dbo.sp_ListarEstadosCita).</summary>
        public cls_Catalogos_DAL CargaListaEstadosCita()
        {
            return CargaCatalogo("SP_Listar_EstadosCita", "EstadosCita");
        }

        /// <summary>Carga el catálogo de profesionales del tipo de clínica indicado (dbo.sp_ListarProfesionales).</summary>
        public cls_Catalogos_DAL CargaListaProfesionales(string sTipoClinica)
        {
            return CargaCatalogoPorClinica("SP_Listar_Profesionales", "Profesionales", sTipoClinica);
        }

        /// <summary>Carga el catálogo de tipos de cita del tipo de clínica indicado (dbo.sp_ListarTiposCita).</summary>
        public cls_Catalogos_DAL CargaListaTiposCita(string sTipoClinica)
        {
            return CargaCatalogoPorClinica("SP_Listar_TiposCita", "TiposCita", sTipoClinica);
        }

        /// <summary>Carga el catálogo de tipos de membresía (dbo.sp_ListarTiposMembresia).</summary>
        public cls_Catalogos_DAL CargaListaTiposMembresia()
        {
            return CargaCatalogo("SP_Listar_TipoMembresia", "TiposMembresia");
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Ejecuta, sin parámetros, el procedimiento indicado por
        /// <paramref name="sClaveAppSetting"/> y devuelve la tabla resultante.
        /// </summary>
        private cls_Catalogos_DAL CargaCatalogo(string sClaveAppSetting, string sNomTabla)
        {
            cls_Catalogos_DAL obj_Catalogos_DAL = new cls_Catalogos_DAL();

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings[sClaveAppSetting];
            obj_BD_DAL.DT_Parametros = null;
            obj_BD_DAL.sNomTabla = sNomTabla;

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Catalogos_DAL.sMSJError = string.Empty;
                obj_Catalogos_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Catalogos_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Catalogos_DAL.dtDatos = null;
            }

            return obj_Catalogos_DAL;
        }

        /// <summary>
        /// Ejecuta, con @TipoClinica como único parámetro, el procedimiento
        /// indicado por <paramref name="sClaveAppSetting"/> y devuelve la
        /// tabla resultante (usado para catálogos congruentes con el tipo
        /// de clínica del usuario en sesión).
        /// </summary>
        private cls_Catalogos_DAL CargaCatalogoPorClinica(string sClaveAppSetting, string sNomTabla, string sTipoClinica)
        {
            cls_Catalogos_DAL obj_Catalogos_DAL = new cls_Catalogos_DAL();

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            DataTable dtParametros = obj_BD_BLL.ObtieneDTParametros(null);
            dtParametros.Rows.Add("@TipoClinica", "6", sTipoClinica);

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings[sClaveAppSetting];
            obj_BD_DAL.DT_Parametros = dtParametros;
            obj_BD_DAL.sNomTabla = sNomTabla;

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Catalogos_DAL.sMSJError = string.Empty;
                obj_Catalogos_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Catalogos_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Catalogos_DAL.dtDatos = null;
            }

            return obj_Catalogos_DAL;
        }

        #endregion
    }
}
