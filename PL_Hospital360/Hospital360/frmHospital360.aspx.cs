using BLL_Hospital360.Usuarios;
using DAL_Hospital360.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;

namespace PL_Hospital360.Hospital360
{
    /// <summary>
    /// Página encargada de gestionar el inicio de sesión
    /// de los usuarios de Hospital360.
    /// </summary>
    public partial class frmHospital360 : System.Web.UI.Page
    {
        /// <summary>
        /// Evento de carga de la página.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Recibe las credenciales enviadas mediante AJAX,
        /// ejecuta la lógica de autenticación y devuelve el resultado
        /// en una cadena separada por la etiqueta SPLITER.
        /// </summary>
        /// <param name="obj_Parametros_JS">
        /// Lista que contiene el nombre de usuario y la contraseña.
        /// </param>
        /// <returns>
        /// Resultado del inicio de sesión y datos del usuario.
        /// </returns>
        [WebMethod]
        public static string InicioSesionUsuarios(
            List<string> obj_Parametros_JS)
        {
            try
            {
                string mensaje = string.Empty;

                // Valida que JavaScript haya enviado los dos valores requeridos.
                if (obj_Parametros_JS == null ||
                    obj_Parametros_JS.Count < 2)
                {
                    return "-3<SPLITER>Debe completar el usuario y la contraseña.";
                }

                // Objetos correspondientes a las capas DAL y BLL.
                cls_Usuarios_DAL obj_Usuarios_DAL =
                    new cls_Usuarios_DAL();

                cls_Usuarios_BLL obj_Usuarios_BLL =
                    new cls_Usuarios_BLL();

                // Asigna las credenciales recibidas desde JavaScript.
                obj_Usuarios_DAL.sNombreUsuario =
                    obj_Parametros_JS[0]?.Trim();

                obj_Usuarios_DAL.sContrasena =
                    obj_Parametros_JS[1];

                // Ejecuta la lógica de inicio de sesión.
                obj_Usuarios_BLL.Inicio_Sesion_Usuarios(
                    ref obj_Usuarios_DAL);

                // Valida errores producidos durante el acceso a la BD.
                if (!string.IsNullOrEmpty(
                    obj_Usuarios_DAL.sMSJError))
                {
                    return "-5<SPLITER>" +
                           obj_Usuarios_DAL.sMSJError;
                }

                // Valida que el procedimiento haya devuelto información.
                if (obj_Usuarios_DAL.dtDatos == null ||
                    obj_Usuarios_DAL.dtDatos.Rows.Count == 0)
                {
                    return "-5<SPLITER>" +
                           "La base de datos no devolvió información.";
                }

                DataRow fila =
                    obj_Usuarios_DAL.dtDatos.Rows[0];

                int resultado =
                    Convert.ToInt32(fila["Resultado"]);

                switch (resultado)
                {
                    case 1:
                        string idUsuario =
                            fila["IdUsuario"].ToString();

                        string nombreCompleto =
                            fila["NombreCompleto"].ToString();

                        string correo =
                            fila["Correo"].ToString();

                        string tipoClinica =
                            fila["TipoClinica"].ToString();

                        mensaje =
                            idUsuario + "<SPLITER>" +
                            "Bienvenid@ de nuevo " +
                            nombreCompleto + "<SPLITER>" +
                            correo + "<SPLITER>" +
                            nombreCompleto + "<SPLITER>" +
                            tipoClinica;

                        break;

                    case -1:
                        mensaje =
                            "-1<SPLITER>" +
                            "La contraseña ingresada es incorrecta.";
                        break;

                    case -2:
                        mensaje =
                            "-2<SPLITER>" +
                            "El usuario no se encuentra registrado.";
                        break;

                    case -3:
                        mensaje =
                            "-3<SPLITER>" +
                            "Debe completar el usuario y la contraseña.";
                        break;

                    case -4:
                        mensaje =
                            "-4<SPLITER>" +
                            "El usuario se encuentra inactivo. " +
                            "Por favor contacte al administrador.";
                        break;

                    case -5:
                        mensaje =
                            "-5<SPLITER>" +
                            "Ocurrió un error al iniciar sesión.";
                        break;

                    default:
                        mensaje =
                            "-5<SPLITER>" +
                            "La respuesta del servidor no es válida.";
                        break;
                }

                return mensaje;
            }
            catch (Exception ex)
            {
                return "-5<SPLITER>" +
                       "Ocurrió un error inesperado: " +
                       ex.Message;
            }
        }
    }
}