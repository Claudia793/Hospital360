using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PL_Hospital360.Modelos
{
    /// <summary>Resultado genérico (registro, cierre de sesión, etc.).</summary>
    public class ResultadoSimpleVM
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>Resultado de sp_IniciarSesion vía WebMethod.</summary>
    public class ResultadoLoginVM
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoClinica { get; set; }
    }

    /// <summary>Fila de un combo (Id/Nombre) para los catálogos de Citas.aspx.</summary>
    public class ItemComboVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    /// <summary>Los 4 catálogos que llenan los filtros de Citas.aspx en una sola llamada.</summary>
    public class CatalogosCitasVM
    {
        public System.Collections.Generic.List<ItemComboVM> EstadosCita { get; set; }
        public System.Collections.Generic.List<ItemComboVM> Profesionales { get; set; }
        public System.Collections.Generic.List<ItemComboVM> TiposCita { get; set; }
        public System.Collections.Generic.List<ItemComboVM> TiposMembresia { get; set; }
    }

    /// <summary>Una fila de la grilla de citas, ya lista para pintar en el JS.</summary>
    public class CitaVM
    {
        public int IdCita { get; set; }
        public string NombrePaciente { get; set; }
        public string Telefono { get; set; }
        public string NombreTipoCita { get; set; }
        public string NombreMembresia { get; set; }
        public string NombreEstado { get; set; }
        public string NombreProfesional { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
    }

    /// <summary>Detalle completo de una cita, para prellenar el formulario de edición.</summary>
    public class CitaDetalleVM
    {
        public bool Encontrada { get; set; }
        public int IdCita { get; set; }
        public string NombrePaciente { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int IdTipoCita { get; set; }
        public int? IdTipoMembresia { get; set; }
        public int IdEstadoCita { get; set; }
        public int IdProfesional { get; set; }
        public string Observaciones { get; set; }
        public string FechaNacimiento { get; set; }
    }

    /// <summary>Un punto Etiqueta/Cantidad, reutilizado por los reportes del panel de control.</summary>
    public class ReporteItemVM
    {
        public string Etiqueta { get; set; }
        public int Cantidad { get; set; }
    }
}