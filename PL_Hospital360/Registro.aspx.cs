using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BLL_Hospital360.Usuarios;
using DAL_Hospital360.Usuarios;
using PL_Hospital360.Modelos;

namespace PL_Hospital360
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(System.Object sender, System.EventArgs e) { }

        /// <summary>Invocado desde JavaScript/Registro.js vía PageMethods.</summary>
        [WebMethod]
        public static ResultadoSimpleVM Registrar(string sNombreCompleto, string sCorreo, string sTelefono,
            string sCedula, string sTipoClinica, string sNombreUsuario, string sContrasena, string sConfirmarContrasena)
        {
            if (sContrasena != sConfirmarContrasena)
            {
                return new ResultadoSimpleVM { Exito = false, Mensaje = "ERROR: Las contraseñas no coinciden" };
            }

            cls_Usuarios_DAL obj = new cls_Usuarios_DAL
            {
                sNombreCompleto = (sNombreCompleto ?? string.Empty).Trim(),
                sCorreo = (sCorreo ?? string.Empty).Trim(),
                sTelefono = (sTelefono ?? string.Empty).Trim(),
                sCedula = (sCedula ?? string.Empty).Trim(),
                sTipoClinica = sTipoClinica,
                sNombreUsuario = (sNombreUsuario ?? string.Empty).Trim(),
                sContrasena = sContrasena
            };

            cls_Usuarios_BLL bll = new cls_Usuarios_BLL();
            obj = bll.Registrar(obj);

            return new ResultadoSimpleVM { Exito = obj.sAXN == "EXITO", Mensaje = obj.sMSJError };
        }
    }
}