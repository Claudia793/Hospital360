using BLL_Hospital360.BD;
using DAL_Hospital360.BD;
using DAL_Hospital360.Reportes;
using System;
using System.Configuration;

namespace BLL_Hospital360.Reportes
{
    /// <summary>
    /// Contiene la lógica de negocio para los reportes agregados
    /// utilizados en el Dashboard (siempre limitados al usuario actual).
    /// </summary>
    public class cls_Reportes_BLL
    {
        #region Métodos públicos

        /// <summary>Citas agrupadas por estado (dbo.sp_ReporteCitasPorEstado).</summary>
        public cls_Reportes_DAL ReporteCitasPorEstado(int iIdUsuario, cls_FiltroReporte_DAL filtro)
        {
            return Ejecutar(iIdUsuario, filtro, "SP_Reporte_CitasPorEstado", "CitasPorEstado");
        }

        /// <summary>Citas agrupadas por mes (dbo.sp_ReporteCitasPorMes).</summary>
        public cls_Reportes_DAL ReporteCitasPorMes(int iIdUsuario, cls_FiltroReporte_DAL filtro)
        {
            return Ejecutar(iIdUsuario, filtro, "SP_Reporte_CitasPorMes", "CitasPorMes");
        }

        /// <summary>Citas agrupadas por tipo de atención (dbo.sp_ReporteCitasPorTipoCita).</summary>
        public cls_Reportes_DAL ReporteCitasPorTipoCita(int iIdUsuario, cls_FiltroReporte_DAL filtro)
        {
            return Ejecutar(iIdUsuario, filtro, "SP_Reporte_CitasPorTipoCita", "CitasPorTipoCita");
        }

        /// <summary>Citas agrupadas por profesional (dbo.sp_ReporteCitasPorProfesional).</summary>
        public cls_Reportes_DAL ReporteCitasPorProfesional(int iIdUsuario, cls_FiltroReporte_DAL filtro)
        {
            return Ejecutar(iIdUsuario, filtro, "SP_Reporte_CitasPorProfesional", "CitasPorProfesional");
        }

        /// <summary>Ingresos reales (basados en TipoMembresia.Precio) agrupados por mes (dbo.sp_ReporteIngresosPorMes).</summary>
        public cls_Reportes_DAL ReporteIngresosPorMes(int iIdUsuario, cls_FiltroReporte_DAL filtro)
        {
            return Ejecutar(iIdUsuario, filtro, "SP_Reporte_IngresosPorMes", "IngresosPorMes");
        }

        /// <summary>Citas por día dentro de un rango de fechas (dbo.sp_ReporteCitasPorRango).</summary>
        public cls_Reportes_DAL ReporteCitasPorRango(int iIdUsuario, DateTime dFechaInicio, DateTime dFechaFin, cls_FiltroReporte_DAL filtro)
        {
            cls_Reportes_DAL obj_Reportes_DAL = new cls_Reportes_DAL { iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Reportes_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Reportes_DAL.dtParametros);

            obj_Reportes_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@FechaInicio", "8", dFechaInicio.ToString("yyyy-MM-dd"));
            obj_Reportes_DAL.dtParametros.Rows.Add("@FechaFin", "8", dFechaFin.ToString("yyyy-MM-dd"));
            obj_Reportes_DAL.dtParametros.Rows.Add("@NombrePaciente", "6", filtro?.sNombrePaciente);
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdEstadoCita", "1", filtro?.iIdEstadoCita.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdProfesional", "1", filtro?.iIdProfesional.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdTipoCita", "1", filtro?.iIdTipoCita.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdTipoMembresia", "1", filtro?.iIdTipoMembresia.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings["SP_Reporte_CitasPorRango"];
            obj_BD_DAL.DT_Parametros = obj_Reportes_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = "CitasPorRango";

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Reportes_DAL.sMSJError = string.Empty;
                obj_Reportes_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Reportes_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Reportes_DAL.dtDatos = null;
            }

            return obj_Reportes_DAL;
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Ejecuta, con @IdUsuario más los mismos 6 filtros opcionales de
        /// dbo.sp_FiltrarCitas, el procedimiento indicado por
        /// <paramref name="sClaveAppSetting"/> y devuelve la tabla
        /// resultante (Etiqueta/Cantidad).
        /// </summary>
        private cls_Reportes_DAL Ejecutar(int iIdUsuario, cls_FiltroReporte_DAL filtro, string sClaveAppSetting, string sNomTabla)
        {
            cls_Reportes_DAL obj_Reportes_DAL = new cls_Reportes_DAL { iIdUsuario = iIdUsuario };

            cls_BD_DAL obj_BD_DAL = new cls_BD_DAL();
            cls_BD_BLL obj_BD_BLL = new cls_BD_BLL();

            obj_Reportes_DAL.dtParametros =
                obj_BD_BLL.ObtieneDTParametros(obj_Reportes_DAL.dtParametros);

            obj_Reportes_DAL.dtParametros.Rows.Add("@IdUsuario", "1", iIdUsuario.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@NombrePaciente", "6", filtro?.sNombrePaciente);
            obj_Reportes_DAL.dtParametros.Rows.Add("@Fecha", "8", filtro?.dFecha.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdEstadoCita", "1", filtro?.iIdEstadoCita.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdProfesional", "1", filtro?.iIdProfesional.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdTipoCita", "1", filtro?.iIdTipoCita.ToString());
            obj_Reportes_DAL.dtParametros.Rows.Add("@IdTipoMembresia", "1", filtro?.iIdTipoMembresia.ToString());

            obj_BD_DAL.sNomSP = ConfigurationManager.AppSettings[sClaveAppSetting];
            obj_BD_DAL.DT_Parametros = obj_Reportes_DAL.dtParametros;
            obj_BD_DAL.sNomTabla = sNomTabla;

            obj_BD_BLL.EjecutaProcesosTabla(ref obj_BD_DAL);

            if (obj_BD_DAL.sMsjErrorBD == string.Empty)
            {
                obj_Reportes_DAL.sMSJError = string.Empty;
                obj_Reportes_DAL.dtDatos =
                    (obj_BD_DAL.DS != null && obj_BD_DAL.DS.Tables.Count > 0)
                        ? obj_BD_DAL.DS.Tables[0]
                        : null;
            }
            else
            {
                obj_Reportes_DAL.sMSJError = obj_BD_DAL.sMsjErrorBD;
                obj_Reportes_DAL.dtDatos = null;
            }

            return obj_Reportes_DAL;
        }

        #endregion
    }
}
