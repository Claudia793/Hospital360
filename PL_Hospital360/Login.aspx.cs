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
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        /// <summary>
        /// Invocado desde JavaScript/InicioSesion.js vía PageMethods.
        /// EnableSession = true porque necesita escribir en Session cuando
        /// el login es correcto.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static ResultadoLoginVM IniciarSesion(string sNombreUsuario, string sContrasena)
        {
            cls_Usuarios_DAL obj = new cls_Usuarios_DAL
            {
                sNombreUsuario = (sNombreUsuario ?? string.Empty).Trim(),
                sContrasena = sContrasena
            };

            cls_Usuarios_BLL bll = new cls_Usuarios_BLL();
            bll.Inicio_Sesion_Usuarios(ref obj);

            bool exito = obj.sAXN == "EXITO";

            if (exito)
            {
                System.Web.HttpContext.Current.Session["IdUsuario"] = obj.iIdUsuario;
                System.Web.HttpContext.Current.Session["NombreCompleto"] = obj.sNombreCompleto;
                System.Web.HttpContext.Current.Session["TipoClinica"] = obj.sTipoClinica;
            }

            return new ResultadoLoginVM
            {
                Exito = exito,
                Mensaje = obj.sMSJError,
                IdUsuario = obj.iIdUsuario,
                NombreCompleto = obj.sNombreCompleto,
                TipoClinica = obj.sTipoClinica
            };
        }
    }
}