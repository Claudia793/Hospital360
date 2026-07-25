using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Hospital360;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Hospital360.Hospital360
{
    /// <summary>
    /// Contiene la lógica de negocio relacionada con los usuarios
    /// del sistema Hospital360.
    /// </summary>
    public class cls_Usuarios_BLL
    {
        #region Métodos públicos

        /// <summary>
        /// Ejecuta el procedimiento almacenado encargado de validar
        /// las credenciales del usuario y recuperar su información.
        /// </summary>
        /// <param name="obj_Usuarios_DAL">
        /// Objeto que contiene las credenciales ingresadas y en el cual
        /// se almacenará el resultado obtenido de la base de datos.
        /// </param>
        public void Inicio_Sesion_Usuarios(
            ref cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            try
            {
                // Objetos necesarios para comunicarse con la base de datos.
                cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
                cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

                /*
                 * Se crea la estructura del DataTable que almacenará
                 * los parámetros requeridos por el procedimiento almacenado.
                 */
                obj_Usuarios_DAL.dtParametros = null;

                obj_Usuarios_DAL.dtParametros =
                    obj_BD_BLL.ObtieneDTParametros(
                        obj_Usuarios_DAL.dtParametros);

                /*
                 * Parámetros requeridos por dbo.sp_IniciarSesion.
                 *
                 * Código 6 = VARCHAR.
                 */
                obj_Usuarios_DAL.dtParametros.Rows.Add(
                    "@NombreUsuario",
                    "6",
                    obj_Usuarios_DAL.sNombreUsuario);

                obj_Usuarios_DAL.dtParametros.Rows.Add(
                    "@Contrasena",
                    "6",
                    obj_Usuarios_DAL.sContrasena);

                // Obtiene el nombre del procedimiento desde Web.config.
                obj_BD_DAL.sNomSP =
                    ConfigurationManager
                        .AppSettings["SP_LogIn_Usuarios"];

                // Asigna los parámetros que serán enviados a SQL Server.
                obj_BD_DAL.DT_Parametros =
                    obj_Usuarios_DAL.dtParametros;

                /*
                 * Nombre lógico de la tabla que se creará
                 * dentro del DataSet.
                 */
                obj_BD_DAL.sNomTabla = "InicioSesion";

                /*
                 * Ejecuta el procedimiento como consulta de tabla,
                 * debido a que devuelve Resultado y, cuando el acceso
                 * es exitoso, también devuelve los datos del usuario.
                 */
                obj_BD_BLL.EjecutaProcesosTabla(
                    ref obj_BD_DAL);

                // Valida si SQL Server produjo algún error.
                if (obj_BD_DAL.sMsjErrorBD == string.Empty)
                {
                    obj_Usuarios_DAL.sMSJError =
                        string.Empty;

                    /*
                     * Verifica que el procedimiento haya devuelto
                     * al menos una tabla antes de acceder a ella.
                     */
                    if (obj_BD_DAL.DS != null &&
                        obj_BD_DAL.DS.Tables.Count > 0)
                    {
                        obj_Usuarios_DAL.dtDatos =
                            obj_BD_DAL.DS.Tables[0];
                    }
                    else
                    {
                        obj_Usuarios_DAL.dtDatos = null;

                        obj_Usuarios_DAL.sMSJError =
                            "El procedimiento almacenado no devolvió información.";
                    }
                }
                else
                {
                    obj_Usuarios_DAL.sMSJError =
                        obj_BD_DAL.sMsjErrorBD;

                    obj_Usuarios_DAL.dtDatos = null;
                }
            }
            catch (Exception)
            {
                /*
                 * Conserva la excepción original para que pueda ser
                 * atendida por la capa de presentación.
                 */
                throw;
            }
        }

        #endregion
    }
}
