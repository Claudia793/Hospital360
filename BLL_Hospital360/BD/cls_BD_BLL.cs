using DAL_Hospital360.BD;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Hospital360.BD
{
    /// <summary>
    /// Contiene los métodos generales utilizados para ejecutar
    /// procedimientos almacenados en la base de datos Hospital360.
    /// </summary>
    public class cls_BD_BLL
    {
        #region Métodos públicos

        /// <summary>
        /// Crea la estructura del DataTable utilizado para almacenar
        /// los parámetros que serán enviados a un procedimiento almacenado.
        /// </summary>
        /// <param name="DT_Param">
        /// DataTable que será inicializado con las columnas necesarias.
        /// </param>
        /// <returns>
        /// DataTable con las columnas Nombre, Tipo_Dato y Valor.
        /// </returns>
        public DataTable ObtieneDTParametros(DataTable DT_Param)
        {
            DT_Param = new DataTable("Tabla_Parametros");

            DT_Param.Columns.Add("Nombre");
            DT_Param.Columns.Add("Tipo_Dato");
            DT_Param.Columns.Add("Valor");

            return DT_Param;
        }

        /// <summary>
        /// Ejecuta procedimientos almacenados que devuelven información,
        /// como procesos de listar o filtrar registros.
        /// </summary>
        /// <param name="Obj_BD_DAL">
        /// Objeto que contiene la conexión, el procedimiento almacenado,
        /// los parámetros y el resultado de la ejecución.
        /// </param>
        public void EjecutaProcesosTabla(ref cls_BD_DAL Obj_BD_DAL)
        {
            try
            {
                // Crea la conexión utilizando la cadena definida en Web.config.
                Obj_BD_DAL.Obj_CNX = new SqlConnection(
                    ConfigurationManager
                        .ConnectionStrings["CNX_SQL"]
                        .ToString()
                );

                // Abre la conexión si se encuentra cerrada.
                if (Obj_BD_DAL.Obj_CNX.State == ConnectionState.Closed)
                {
                    Obj_BD_DAL.Obj_CNX.Open();
                }

                // Crea el adaptador encargado de ejecutar el procedimiento almacenado.
                Obj_BD_DAL.Obj_DAP = new SqlDataAdapter(
                    Obj_BD_DAL.sNomSP,
                    Obj_BD_DAL.Obj_CNX
                );

                Obj_BD_DAL.Obj_DAP.SelectCommand.CommandType =
                    CommandType.StoredProcedure;

                #region Agregar parámetros

                if (Obj_BD_DAL.DT_Parametros != null)
                {
                    SqlDbType TipoDatoSQL = SqlDbType.VarChar;

                    foreach (DataRow dr in Obj_BD_DAL.DT_Parametros.Rows)
                    {
                        TipoDatoSQL = ObtenerTipoDatoSQL(
                            dr["Tipo_Dato"].ToString()
                        );

                        Obj_BD_DAL.Obj_DAP
                            .SelectCommand
                            .Parameters
                            .Add(
                                dr["Nombre"].ToString(),
                                TipoDatoSQL
                            )
                            .Value = ObtenerValorParametro(
                                dr["Valor"].ToString(),
                                TipoDatoSQL
                            );
                    }
                }

                #endregion

                // Inicializa el DataSet que almacenará los resultados.
                Obj_BD_DAL.DS = new DataSet();

                // Ejecuta el procedimiento y llena el DataSet.
                Obj_BD_DAL.Obj_DAP.Fill(
                    Obj_BD_DAL.DS,
                    Obj_BD_DAL.sNomTabla
                );

                // Limpia el mensaje de error si la operación fue exitosa.
                Obj_BD_DAL.sMsjErrorBD = string.Empty;
            }
            catch (SqlException ex)
            {
                Obj_BD_DAL.sMsjErrorBD = ex.Message;
            }
            finally
            {
                if (Obj_BD_DAL.Obj_CNX != null)
                {
                    if (Obj_BD_DAL.Obj_CNX.State == ConnectionState.Open)
                    {
                        Obj_BD_DAL.Obj_CNX.Close();
                    }

                    Obj_BD_DAL.Obj_CNX.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta procedimientos almacenados relacionados con operaciones
        /// de insertar, actualizar o eliminar registros.
        /// </summary>
        /// <param name="Obj_BD_DAL">
        /// Objeto que contiene la conexión, el procedimiento almacenado,
        /// los parámetros y el resultado de la ejecución.
        /// </param>
        public void EjecutaProcesosComando(ref cls_BD_DAL Obj_BD_DAL)
        {
            try
            {
                // Crea la conexión utilizando la cadena definida en Web.config.
                Obj_BD_DAL.Obj_CNX = new SqlConnection(
                    ConfigurationManager
                        .ConnectionStrings["CNX_SQL"]
                        .ToString()
                );

                // Abre la conexión si se encuentra cerrada.
                if (Obj_BD_DAL.Obj_CNX.State == ConnectionState.Closed)
                {
                    Obj_BD_DAL.Obj_CNX.Open();
                }

                // Crea el comando que ejecutará el procedimiento almacenado.
                Obj_BD_DAL.Obj_CMD = new SqlCommand(
                    Obj_BD_DAL.sNomSP,
                    Obj_BD_DAL.Obj_CNX
                );

                Obj_BD_DAL.Obj_CMD.CommandType =
                    CommandType.StoredProcedure;

                #region Agregar parámetros

                if (Obj_BD_DAL.DT_Parametros != null)
                {
                    SqlDbType TipoDatoSQL = SqlDbType.VarChar;

                    foreach (DataRow dr in Obj_BD_DAL.DT_Parametros.Rows)
                    {
                        TipoDatoSQL = ObtenerTipoDatoSQL(
                            dr["Tipo_Dato"].ToString()
                        );

                        Obj_BD_DAL.Obj_CMD
                            .Parameters
                            .Add(
                                dr["Nombre"].ToString(),
                                TipoDatoSQL
                            )
                            .Value = ObtenerValorParametro(
                                dr["Valor"].ToString(),
                                TipoDatoSQL
                            );
                    }
                }

                #endregion

                /*
                 * NORMAL ejecuta un comando sin esperar un valor.
                 * Cualquier otro indicador ejecuta ExecuteScalar.
                 */
                if (Obj_BD_DAL.sIndAxn == "NORMAL")
                {
                    Obj_BD_DAL.Obj_CMD.ExecuteNonQuery();
                    Obj_BD_DAL.sValorScalar = string.Empty;
                }
                else
                {
                    object Resultado =
                        Obj_BD_DAL.Obj_CMD.ExecuteScalar();

                    Obj_BD_DAL.sValorScalar =
                        Resultado == null
                            ? string.Empty
                            : Resultado.ToString().Trim();
                }

                Obj_BD_DAL.sMsjErrorBD = string.Empty;
            }
            catch (SqlException ex)
            {
                Obj_BD_DAL.sMsjErrorBD = ex.Message;
            }
            finally
            {
                if (Obj_BD_DAL.Obj_CNX != null)
                {
                    if (Obj_BD_DAL.Obj_CNX.State == ConnectionState.Open)
                    {
                        Obj_BD_DAL.Obj_CNX.Close();
                    }

                    Obj_BD_DAL.Obj_CNX.Dispose();
                }
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Convierte el valor de texto almacenado en la tabla de parámetros
        /// al valor que se enviará al SqlParameter. Los tipos no textuales
        /// (Int, DateTime, etc.) reciben DBNull cuando el valor viene vacío,
        /// ya que no pueden convertirse desde una cadena vacía.
        /// </summary>
        private object ObtenerValorParametro(string sValor, SqlDbType TipoDatoSQL)
        {
            bool bEsTipoTexto =
                TipoDatoSQL == SqlDbType.VarChar ||
                TipoDatoSQL == SqlDbType.NVarChar ||
                TipoDatoSQL == SqlDbType.Char ||
                TipoDatoSQL == SqlDbType.NChar;

            if (!bEsTipoTexto && string.IsNullOrEmpty(sValor))
            {
                return DBNull.Value;
            }

            return sValor;
        }

        /// <summary>
        /// Convierte el código almacenado en la tabla de parámetros
        /// al tipo de dato correspondiente de SQL Server.
        /// </summary>
        /// <param name="CodigoTipoDato">
        /// Código numérico que representa el tipo de dato.
        /// </param>
        /// <returns>
        /// Tipo de dato equivalente de SQL Server.
        /// </returns>
        private SqlDbType ObtenerTipoDatoSQL(string CodigoTipoDato)
        {
            switch (CodigoTipoDato)
            {
                case "1":
                    return SqlDbType.Int;

                case "2":
                    return SqlDbType.Decimal;

                case "3":
                    return SqlDbType.Float;

                case "4":
                    return SqlDbType.Char;

                case "5":
                    return SqlDbType.NChar;

                case "6":
                    return SqlDbType.VarChar;

                case "7":
                    return SqlDbType.NVarChar;

                case "8":
                    return SqlDbType.DateTime;

                case "9":
                    return SqlDbType.Bit;

                case "10":
                    return SqlDbType.Money;

                case "11":
                    return SqlDbType.TinyInt;

                case "12":
                    return SqlDbType.Time;

                default:
                    return SqlDbType.VarChar;
            }
        }

        #endregion
    }
}
