using System;
using System.Data;

namespace DAL_Hospital360.Citas
{
    /// <summary>
    /// Representa los filtros de búsqueda y el resultado de consultar
    /// las citas mediante dbo.sp_FiltrarCitas.
    /// </summary>
    public class cls_Citas_DAL
    {
        #region Variables privadas

        private string _sNombrePaciente;
        private DateTime? _dFecha;
        private int? _iIdEstadoCita;
        private int? _iIdProfesional;
        private int? _iIdTipoCita;
        private int? _iIdTipoMembresia;

        private string _sAXN;
        private string _sMSJError;
        private DataTable _dtDatos;
        private DataTable _dtParametros;

        #endregion

        #region Propiedades públicas

        /// <summary>Obtiene o establece el nombre del paciente a buscar (filtro opcional).</summary>
        public string sNombrePaciente
        {
            get => _sNombrePaciente;
            set => _sNombrePaciente = value;
        }

        /// <summary>Obtiene o establece la fecha de la cita a buscar (filtro opcional).</summary>
        public DateTime? dFecha
        {
            get => _dFecha;
            set => _dFecha = value;
        }

        /// <summary>Obtiene o establece el estado de cita a filtrar (filtro opcional).</summary>
        public int? iIdEstadoCita
        {
            get => _iIdEstadoCita;
            set => _iIdEstadoCita = value;
        }

        /// <summary>Obtiene o establece el profesional a filtrar (filtro opcional).</summary>
        public int? iIdProfesional
        {
            get => _iIdProfesional;
            set => _iIdProfesional = value;
        }

        /// <summary>Obtiene o establece el tipo de cita a filtrar (filtro opcional).</summary>
        public int? iIdTipoCita
        {
            get => _iIdTipoCita;
            set => _iIdTipoCita = value;
        }

        /// <summary>Obtiene o establece el tipo de membresía a filtrar (filtro opcional).</summary>
        public int? iIdTipoMembresia
        {
            get => _iIdTipoMembresia;
            set => _iIdTipoMembresia = value;
        }

        /// <summary>Obtiene o establece la acción que se realizó.</summary>
        public string sAXN
        {
            get => _sAXN;
            set => _sAXN = value;
        }

        /// <summary>Obtiene o establece el mensaje de error generado.</summary>
        public string sMSJError
        {
            get => _sMSJError;
            set => _sMSJError = value;
        }

        /// <summary>Obtiene o establece la tabla con las citas encontradas.</summary>
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
