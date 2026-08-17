using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Hospital360.Auditoria
{
    /// <summary>
    /// Representa el historial de auditoría (inicios/cierres de sesión y
    /// acciones realizadas dentro de la app) de un usuario del sistema
    /// Hospital360, y almacena los datos necesarios para realizar
    /// operaciones relacionadas con la base de datos.
    /// </summary>
    public class cls_Auditoria_DAL
    {
        #region Variables privadas

        // Campos correspondientes a la tabla Auditoria.
        private int _iIdUsuario;

        // Campos utilizados en todas las clases DAL.
        private string _sMSJError;
        private DataTable _dtDatos;
        private DataTable _dtParametros;

        #endregion

        #region Propiedades públicas

        /// <summary>
        /// Obtiene o establece el identificador del usuario dueño del historial.
        /// </summary>
        public int iIdUsuario
        {
            get => _iIdUsuario;
            set => _iIdUsuario = value;
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
