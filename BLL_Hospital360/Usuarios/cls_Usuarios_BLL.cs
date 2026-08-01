using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Usuarios;
using System;
using System.Configuration;
using System.Data;

namespace BLL_Hospital360.Usuarios
{
    /// <summary>
    /// Contiene la lógica de negocio relacionada con los usuarios
    /// del sistema Hospital360.
    /// </summary>
    public class cls_Usuarios_BLL
    {
        #region Métodos públicos

        /// <summary>
        /// Valida las credenciales del usuario contra dbo.sp_IniciarSesion
        /// y devuelve el resultado listo para exponer vía WebMethod.
        /// </summary>
        public cls_Usuarios_DAL IniciarSesion(cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Usuarios_DAL.dtParametros);

            // Código 6 = VARCHAR.
            obj_Usuarios_DAL.dtParametros.Rows.Add("@NombreUsuario", "6", obj_Usuarios_DAL.sNombreUsuario);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@Contrasena", "6", obj_Usuarios_DAL.sContrasena);

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_LogIn_Usuarios"];
            obj_BD_DAL.DT_Parametros = obj_Usuarios_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "InicioSesion";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Usuarios_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Usuarios_DAL;
            }

            DataRow fila = obj_BD_DAL.DS.Tables[0].Rows[0];
            int iResultado = Convert.ToInt32(fila["Resultado"]);

            switch (iResultado)
            {
                case 1:
                    obj_Usuarios_DAL.sAXN = "EXITO";
                    obj_Usuarios_DAL.sMSJError = string.Empty;
                    obj_Usuarios_DAL.iIdUsuario = Convert.ToInt32(fila["IdUsuario"]);
                    obj_Usuarios_DAL.sNombreCompleto = fila["NombreCompleto"].ToString();
                    obj_Usuarios_DAL.sTipoClinica = fila["TipoClinica"].ToString();
                    break;

                case -1:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "La contraseña ingresada es incorrecta.";
                    break;

                case -2:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "El usuario no se encuentra registrado.";
                    break;

                case -3:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Debe completar el usuario y la contraseña.";
                    break;

                case -4:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "El usuario se encuentra inactivo. Por favor contacte al administrador.";
                    break;

                default:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ocurrió un error al iniciar sesión.";
                    break;
            }

            return obj_Usuarios_DAL;
        }

        /// <summary>
        /// Registra un nuevo usuario mediante dbo.sp_RegistrarUsuario e interpreta
        /// el código Resultado que el procedimiento devuelve (éxito, correo
        /// duplicado, usuario duplicado, datos inválidos, etc.).
        /// </summary>
        public cls_Usuarios_DAL Registrar(cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Usuarios_DAL.dtParametros);

            // Código 6 = VARCHAR.
            obj_Usuarios_DAL.dtParametros.Rows.Add("@NombreCompleto", "6", obj_Usuarios_DAL.sNombreCompleto);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@Correo", "6", obj_Usuarios_DAL.sCorreo);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@Telefono", "6", obj_Usuarios_DAL.sTelefono);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@Cedula", "6", obj_Usuarios_DAL.sCedula);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@TipoClinica", "6", obj_Usuarios_DAL.sTipoClinica);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@NombreUsuario", "6", obj_Usuarios_DAL.sNombreUsuario);
            obj_Usuarios_DAL.dtParametros.Rows.Add("@Contrasena", "6", obj_Usuarios_DAL.sContrasena);

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Insert_Usuarios"];
            obj_BD_DAL.DT_Parametros = obj_Usuarios_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "Registro";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD != string.Empty)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                return obj_Usuarios_DAL;
            }

            if (obj_BD_DAL.DS == null || obj_BD_DAL.DS.Tables.Count == 0 || obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError = "El procedimiento almacenado no devolvió información.";
                return obj_Usuarios_DAL;
            }

            int iResultado = Convert.ToInt32(obj_BD_DAL.DS.Tables[0].Rows[0]["Resultado"]);

            switch (iResultado)
            {
                case 1:
                    obj_Usuarios_DAL.sAXN = "EXITO";
                    obj_Usuarios_DAL.sMSJError = string.Empty;
                    break;

                case -1:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ya existe un usuario registrado con ese correo electrónico.";
                    break;

                case -2:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ya existe un usuario registrado con ese nombre de usuario.";
                    break;

                case -3:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Debe completar todos los campos obligatorios con un nombre válido, un correo válido y una cédula de 9 dígitos.";
                    break;

                case -6:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ya existe un usuario registrado con esa cédula.";
                    break;

                case -5:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ocurrió un error interno al registrar el usuario. Intente nuevamente.";
                    break;

                default:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError = "Ocurrió un error al registrar el usuario.";
                    break;
            }

            return obj_Usuarios_DAL;
        }

        /// <summary>
        /// Cierra la sesión del usuario mediante dbo.sp_CerrarSesion.
        /// </summary>
        public cls_Usuarios_DAL CerrarSesion(int iIdUsuario)
        {
            cls_Usuarios_DAL obj_Usuarios_DAL = new cls_Usuarios_DAL { iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Usuarios_DAL.dtParametros);

            // Código 1 = INT.
            obj_Usuarios_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario);

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_CierraSesion_Usuarios"];
            obj_BD_DAL.DT_Parametros = obj_Usuarios_DAL.dtParametros;
            obj_BD_DAL.sIndAxn = "NORMAL";

            obj_BD_BLL.EjecutaProcesosComando(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Usuarios_DAL.sAXN = "EXITO";
                obj_Usuarios_DAL.sMSJError = string.Empty;
            }
            else
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
            }

            return obj_Usuarios_DAL;
        }

        #endregion
    }
}
