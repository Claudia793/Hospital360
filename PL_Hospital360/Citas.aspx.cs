using System;
using System.Web.UI;

namespace PL_Hospital360
{
    public partial class Citas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }
    }
}