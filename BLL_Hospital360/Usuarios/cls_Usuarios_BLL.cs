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
        #region Inicio de sesión

        /// <summary>
        /// Valida las credenciales del usuario mediante
        /// el procedimiento almacenado dbo.sp_IniciarSesion.
        /// </summary>
        public cls_Usuarios_DAL IniciarSesion(
            cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros = null;

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(
                    obj_Usuarios_DAL.dtParametros);

            // Código 6 = VARCHAR.
            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@NombreUsuario",
                "6",
                obj_Usuarios_DAL.sNombreUsuario);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@Contrasena",
                "6",
                obj_Usuarios_DAL.sContrasena);

            obj_BD_DAL.sNomSP =
                ConfigurationManager
                    .AppSettings["SP_LogIn_Usuarios"];

            obj_BD_DAL.DT_Parametros =
                obj_Usuarios_DAL.dtParametros;

            obj_BD_DAL.sNomTabla =
                "InicioSesion";

            obj_BD_BLL.EjecutaProcesosTabla(
                ref obj_BD_DAL);

            if (!string.IsNullOrEmpty(obj_BD_DAL.sMsjErrorBD))
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError =
                    obj_BD_DAL.sMsjErrorBD;

                obj_Usuarios_DAL.dtDatos = null;

                return obj_Usuarios_DAL;
            }

            if (obj_BD_DAL.DS == null ||
                obj_BD_DAL.DS.Tables.Count == 0 ||
                obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError =
                    "El procedimiento almacenado no devolvió información.";

                obj_Usuarios_DAL.dtDatos = null;

                return obj_Usuarios_DAL;
            }

            obj_Usuarios_DAL.dtDatos =
                obj_BD_DAL.DS.Tables[0];

            DataRow fila =
                obj_Usuarios_DAL.dtDatos.Rows[0];

            int resultado =
                Convert.ToInt32(fila["Resultado"]);

            switch (resultado)
            {
                case 1:
                    obj_Usuarios_DAL.sAXN = "EXITO";
                    obj_Usuarios_DAL.sMSJError =
                        "Inicio de sesión correcto.";

                    obj_Usuarios_DAL.iIdUsuario =
                        Convert.ToInt32(fila["IdUsuario"]);

                    obj_Usuarios_DAL.sNombreCompleto =
                        fila["NombreCompleto"].ToString();

                    obj_Usuarios_DAL.sCorreo =
                        fila.Table.Columns.Contains("Correo") &&
                        fila["Correo"] != DBNull.Value
                            ? fila["Correo"].ToString()
                            : string.Empty;

                    obj_Usuarios_DAL.sTipoClinica =
                        fila.Table.Columns.Contains("TipoClinica") &&
                        fila["TipoClinica"] != DBNull.Value
                            ? fila["TipoClinica"].ToString()
                            : string.Empty;

                    break;

                case -1:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "La contraseña ingresada es incorrecta.";
                    break;

                case -2:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El usuario no se encuentra registrado.";
                    break;

                case -3:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "Debe completar el usuario y la contraseña.";
                    break;

                case -4:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El usuario se encuentra inactivo.";
                    break;

                case -5:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "Ocurrió un error interno al iniciar sesión.";
                    break;

                default:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El procedimiento devolvió un resultado no reconocido.";
                    break;
            }

            return obj_Usuarios_DAL;
        }

        #endregion

        #region Registro

        /// <summary>
        /// Registra un usuario y devuelve el objeto con el resultado.
        /// </summary>
        public cls_Usuarios_DAL Registrar(
            cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            Registrar_Usuarios(
                ref obj_Usuarios_DAL);

            return obj_Usuarios_DAL;
        }

        /// <summary>
        /// Registra un nuevo usuario mediante
        /// dbo.sp_RegistrarUsuario.
        /// </summary>
        public void Registrar_Usuarios(
            ref cls_Usuarios_DAL obj_Usuarios_DAL)
        {
            cls_BD_DAL obj_BD_DAL =
                new cls_BD_DAL();

            cls_BD_BLL obj_BD_BLL =
                new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros = null;

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(
                    obj_Usuarios_DAL.dtParametros);

            // Código 6 = VARCHAR.
            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@NombreCompleto",
                "6",
                obj_Usuarios_DAL.sNombreCompleto);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@Correo",
                "6",
                obj_Usuarios_DAL.sCorreo);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@Telefono",
                "6",
                string.IsNullOrWhiteSpace(
                    obj_Usuarios_DAL.sTelefono)
                        ? null
                        : obj_Usuarios_DAL.sTelefono);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@NombreUsuario",
                "6",
                obj_Usuarios_DAL.sNombreUsuario);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@Contrasena",
                "6",
                obj_Usuarios_DAL.sContrasena);

            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@TipoClinica",
                "6",
                string.IsNullOrWhiteSpace(
                    obj_Usuarios_DAL.sTipoClinica)
                        ? null
                        : obj_Usuarios_DAL.sTipoClinica);

            obj_BD_DAL.sNomSP =
                ConfigurationManager
                    .AppSettings["SP_Insert_Usuarios"];

            obj_BD_DAL.DT_Parametros =
                obj_Usuarios_DAL.dtParametros;

            obj_BD_DAL.sNomTabla =
                "RegistroUsuario";

            obj_BD_BLL.EjecutaProcesosTabla(
                ref obj_BD_DAL);

            if (!string.IsNullOrEmpty(
                    obj_BD_DAL.sMsjErrorBD))
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError =
                    obj_BD_DAL.sMsjErrorBD;

                obj_Usuarios_DAL.dtDatos = null;

                return;
            }

            if (obj_BD_DAL.DS == null ||
                obj_BD_DAL.DS.Tables.Count == 0 ||
                obj_BD_DAL.DS.Tables[0].Rows.Count == 0)
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError =
                    "El procedimiento almacenado no devolvió información.";

                obj_Usuarios_DAL.dtDatos = null;

                return;
            }

            obj_Usuarios_DAL.dtDatos =
                obj_BD_DAL.DS.Tables[0];

            int resultado =
                Convert.ToInt32(
                    obj_Usuarios_DAL
                        .dtDatos.Rows[0]["Resultado"]);

            switch (resultado)
            {
                case 1:
                    obj_Usuarios_DAL.sAXN = "EXITO";
                    obj_Usuarios_DAL.sMSJError =
                        "Usuario registrado correctamente.";
                    break;

                case -1:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El correo ya se encuentra registrado.";
                    break;

                case -2:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El nombre de usuario ya está en uso.";
                    break;

                case -3:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "Debe completar correctamente los campos obligatorios.";
                    break;

                case -5:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "Ocurrió un error interno al registrar el usuario.";
                    break;

                default:
                    obj_Usuarios_DAL.sAXN = "ERROR";
                    obj_Usuarios_DAL.sMSJError =
                        "El procedimiento devolvió un resultado no reconocido.";
                    break;
            }
        }

        #endregion

        #region Cierre de sesión

        /// <summary>
        /// Cierra la sesión del usuario mediante
        /// dbo.sp_CerrarSesion.
        /// </summary>
        public cls_Usuarios_DAL CerrarSesion(
            int iIdUsuario)
        {
            cls_Usuarios_DAL obj_Usuarios_DAL =
                new cls_Usuarios_DAL
                {
                    iIdUsuario = iIdUsuario
                };

            cls_BD_DAL obj_BD_DAL =
                new cls_BD_DAL();

            cls_BD_BLL obj_BD_BLL =
                new cls_BD_BLL();

            obj_Usuarios_DAL.dtParametros = null;

            obj_Usuarios_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(
                    obj_Usuarios_DAL.dtParametros);

            // Código 1 = INT.
            obj_Usuarios_DAL.dtParametros.Rows.Add(
                "@IdUsuario",
                "1",
                iIdUsuario);

            obj_BD_DAL.sNomSP =
                ConfigurationManager
                    .AppSettings["SP_CierraSesion_Usuarios"];

            obj_BD_DAL.DT_Parametros =
                obj_Usuarios_DAL.dtParametros;

            obj_BD_DAL.sIndAxn =
                "NORMAL";

            obj_BD_BLL.EjecutaProcesosComando(
                ref obj_BD_DAL);

            if (string.IsNullOrEmpty(
                    obj_BD_DAL.sMsjErrorBD))
            {
                obj_Usuarios_DAL.sAXN = "EXITO";
                obj_Usuarios_DAL.sMSJError =
                    string.Empty;
            }
            else
            {
                obj_Usuarios_DAL.sAXN = "ERROR";
                obj_Usuarios_DAL.sMSJError =
                    obj_BD_DAL.sMsjErrorBD;
            }

            return obj_Usuarios_DAL;
        }

        #endregion
    }
}