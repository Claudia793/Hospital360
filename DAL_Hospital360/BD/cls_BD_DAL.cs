using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Hospital360.BD
{
    /// <summary>
    /// Clase que almacena los objetos necesarios para realizar operaciones
    /// con la base de datos, como la conexión, comandos, adaptadores,
    /// parámetros y resultados de las consultas.
    /// </summary>
    public class cls_BD_DAL
    {
        #region Variables privadas

        /// <summary>
        /// Objeto de conexión con SQL Server.
        /// </summary>
        private SqlConnection _Obj_CNX;

        /// <summary>
        /// Adaptador utilizado para llenar DataSet o DataTable.
        /// </summary>
        private SqlDataAdapter _Obj_DAP;

        /// <summary>
        /// Comando SQL que ejecutará procedimientos almacenados o consultas.
        /// </summary>
        private SqlCommand _Obj_CMD;

        /// <summary>
        /// Conjunto de datos que almacenará los resultados de una consulta.
        /// </summary>
        private DataSet _DS;

        /// <summary>
        /// Tabla que almacenará los parámetros enviados a un procedimiento almacenado.
        /// </summary>
        private DataTable _DT_Parametros;

        /// <summary>
        /// Nombre de la tabla que contendrá la información obtenida.
        /// </summary>
        private string _sNomTabla;

        /// <summary>
        /// Nombre del procedimiento almacenado que se ejecutará.
        /// </summary>
        private string _sNomSP;

        /// <summary>
        /// Mensaje de error generado durante una operación de base de datos.
        /// </summary>
        private string _sMsjErrorBD;

        /// <summary>
        /// Indicador de la acción que se realizará (Insertar, Modificar, Eliminar, etc.).
        /// </summary>
        private string _sIndAxn;

        /// <summary>
        /// Valor devuelto por una consulta escalar.
        /// </summary>
        private string _sValorScalar;

        #endregion

        #region Variables públicas

        /// <summary>
        /// Obtiene o establece la conexión con la base de datos.
        /// </summary>
        public SqlConnection Obj_CNX
        {
            get => _Obj_CNX;
            set => _Obj_CNX = value;
        }

        /// <summary>
        /// Obtiene o establece el adaptador de datos.
        /// </summary>
        public SqlDataAdapter Obj_DAP
        {
            get => _Obj_DAP;
            set => _Obj_DAP = value;
        }

        /// <summary>
        /// Obtiene o establece el comando SQL.
        /// </summary>
        public SqlCommand Obj_CMD
        {
            get => _Obj_CMD;
            set => _Obj_CMD = value;
        }

        /// <summary>
        /// Obtiene o establece el mensaje de error generado por la base de datos.
        /// </summary>
        public string sMsjErrorBD
        {
            get => _sMsjErrorBD;
            set => _sMsjErrorBD = value;
        }

        /// <summary>
        /// Obtiene o establece el nombre de la tabla utilizada en el DataSet.
        /// </summary>
        public string sNomTabla
        {
            get => _sNomTabla;
            set => _sNomTabla = value;
        }

        /// <summary>
        /// Obtiene o establece el nombre del procedimiento almacenado.
        /// </summary>
        public string sNomSP
        {
            get => _sNomSP;
            set => _sNomSP = value;
        }

        /// <summary>
        /// Obtiene o establece el DataSet con los resultados de la consulta.
        /// </summary>
        public DataSet DS
        {
            get => _DS;
            set => _DS = value;
        }

        /// <summary>
        /// Obtiene o establece la tabla de parámetros.
        /// </summary>
        public DataTable DT_Parametros
        {
            get => _DT_Parametros;
            set => _DT_Parametros = value;
        }

        /// <summary>
        /// Obtiene o establece el indicador de la acción a ejecutar.
        /// </summary>
        public string sIndAxn
        {
            get => _sIndAxn;
            set => _sIndAxn = value;
        }

        /// <summary>
        /// Obtiene o establece el valor devuelto por una consulta escalar.
        /// </summary>
        public string sValorScalar
        {
            get => _sValorScalar;
            set => _sValorScalar = value;
        }

        #endregion
    }
}
