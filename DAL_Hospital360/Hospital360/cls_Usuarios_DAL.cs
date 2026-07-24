using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Hospital360.Hospital360
{
    /// <summary>
    /// Representa la información de los usuarios del sistema Hospital360
    /// y almacena los datos necesarios para realizar operaciones
    /// relacionadas con la base de datos.
    /// </summary>
    public class cls_Usuarios_DAL
    {
        #region Variables privadas

        // Campos correspondientes a la tabla Usuarios.
        private int _iIdUsuario;
        private string _sNombreCompleto;
        private string _sCorreo;
        private string _sTelefono;
        private string _sNombreUsuario;
        private string _sContrasena;
        private string _sTipoClinica;
        private bool _bActivo;

        // Campos utilizados en todas las clases DAL.
        private string _sValorScalar;
        private string _sAXN;
        private string _sMSJError;
        private DataTable _dtDatos;
        private DataTable _dtParametros;

        #endregion

        #region Propiedades públicas

        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        public int iIdUsuario
        {
            get => _iIdUsuario;
            set => _iIdUsuario = value;
        }

        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string sNombreCompleto
        {
            get => _sNombreCompleto;
            set => _sNombreCompleto = value;
        }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string sCorreo
        {
            get => _sCorreo;
            set => _sCorreo = value;
        }

        /// <summary>
        /// Obtiene o establece el número de teléfono del usuario.
        /// </summary>
        public string sTelefono
        {
            get => _sTelefono;
            set => _sTelefono = value;
        }

        /// <summary>
        /// Obtiene o establece el nombre utilizado para iniciar sesión.
        /// </summary>
        public string sNombreUsuario
        {
            get => _sNombreUsuario;
            set => _sNombreUsuario = value;
        }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// </summary>
        public string sContrasena
        {
            get => _sContrasena;
            set => _sContrasena = value;
        }

        /// <summary>
        /// Obtiene o establece el tipo de clínica asociada al usuario.
        /// </summary>
        public string sTipoClinica
        {
            get => _sTipoClinica;
            set => _sTipoClinica = value;
        }

        /// <summary>
        /// Obtiene o establece si el usuario se encuentra activo.
        /// </summary>
        public bool bActivo
        {
            get => _bActivo;
            set => _bActivo = value;
        }

        /// <summary>
        /// Obtiene o establece el valor devuelto por una operación escalar.
        /// </summary>
        public string sValorScalar
        {
            get => _sValorScalar;
            set => _sValorScalar = value;
        }

        /// <summary>
        /// Obtiene o establece la acción que se realizará.
        /// </summary>
        public string sAXN
        {
            get => _sAXN;
            set => _sAXN = value;
        }

        /// <summary>
        /// Obtiene o establece el mensaje de error generado.
        /// </summary>
        public string sMSJError
        {
            get => _sMSJError;
            set => _sMSJError = value;
        }

        /// <summary>
        /// Obtiene o establece la tabla con los datos obtenidos.
        /// </summary>
        public DataTable dtDatos
        {
            get => _dtDatos;
            set => _dtDatos = value;
        }

        /// <summary>
        /// Obtiene o establece la tabla con los parámetros
        /// enviados al procedimiento almacenado.
        /// </summary>
        public DataTable dtParametros
        {
            get => _dtParametros;
            set => _dtParametros = value;
        }

        #endregion
    }
}
