using System.Data;

namespace DAL_Hospital360.Catalogos
{
    /// <summary>
    /// Representa el resultado de consultar un catálogo simple
    /// (estados de cita, profesionales, tipos de cita, tipos de membresía).
    /// </summary>
    public class cls_Catalogos_DAL
    {
        #region Variables privadas

        private string _sMSJError;
        private DataTable _dtDatos;
        private DataTable _dtParametros;

        #endregion

        #region Propiedades públicas

        /// <summary>
        /// Obtiene o establece el mensaje de error generado.
        /// </summary>
        public string sMSJError
        {
            get => _sMSJError;
            set => _sMSJError = value;
        }

        /// <summary>
        /// Obtiene o establece la tabla con los datos del catálogo.
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
