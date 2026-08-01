using System;
using System.Data;

namespace DAL_Hospital360.Reportes
{
    /// <summary>
    /// Filtros opcionales aplicados a los reportes del panel de control,
    /// los mismos 6 filtros disponibles en la grilla de citas
    /// (dbo.sp_FiltrarCitas), para que los gráficos reflejen lo que el
    /// usuario tiene filtrado en la pantalla.
    /// </summary>
    public class cls_FiltroReporte_DAL
    {
        public string sNombrePaciente { get; set; }
        public DateTime? dFecha { get; set; }
        public int? iIdEstadoCita { get; set; }
        public int? iIdProfesional { get; set; }
        public int? iIdTipoCita { get; set; }
        public int? iIdTipoMembresia { get; set; }
    }

    /// <summary>
    /// Representa el resultado de cualquiera de los reportes agregados
    /// del Dashboard (citas por estado, por mes, por tipo, por profesional).
    /// </summary>
    public class cls_Reportes_DAL
    {
        #region Variables privadas

        private int _iIdUsuario;
        private string _sMSJError;
        private DataTable _dtDatos;
        private DataTable _dtParametros;

        #endregion

        #region Propiedades públicas

        /// <summary>Obtiene o establece el IdUsuario de la sesión activa.</summary>
        public int iIdUsuario
        {
            get => _iIdUsuario;
            set => _iIdUsuario = value;
        }

        /// <summary>Obtiene o establece el mensaje de error generado.</summary>
        public string sMSJError
        {
            get => _sMSJError;
            set => _sMSJError = value;
        }

        /// <summary>Obtiene o establece la tabla con el reporte (Etiqueta/Cantidad).</summary>
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
