using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;


namespace ClassLibrary
{
    public class RdbMetadata
    {
        public static List<String> RetrieveUserTables(String databaseType)
        {
            if (databaseType == "mySql") return RetrieveMySqlUserTables(databaseType);
            if (databaseType == "firebird") return RetrieveFirebirdUserTables(databaseType);
            return new List<String>(); // Caso databaseType seja inválido retorna uma lista vazia
        }

        public static List<String> RetrieveMySqlUserTables(String databaseType)
        {
            List<String> userTables = new List<String>();

            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection(databaseType);

            String query = "SELECT * FROM information_schema.tables WHERE table_schema = 'addoncontratos'";
            MySqlCommand command = new MySqlCommand(query, dataConnector.MySqlConnection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String tableName = ((String)dataReader["TABLE_NAME"]).Trim();
                userTables.Add(tableName);
            }
            dataReader.Close();

            dataConnector.CloseConnection();

            return userTables;
        }

        public static List<String> RetrieveFirebirdUserTables(String databaseType)
        {
            List<String> userTables = new List<String>();

            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection(databaseType);

            String query = "SELECT * FROM RDB$RELATIONS WHERE RDB$VIEW_BLR IS NULL AND RDB$SYSTEM_FLAG = 0 ORDER BY RDB$RELATION_NAME";
            FbCommand command = new FbCommand(query, dataConnector.FirebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String tableName = ((String)dataReader["RDB$RELATION_NAME"]).Trim();
                userTables.Add(tableName);
            }
            dataReader.Close();

            dataConnector.CloseConnection();

            return userTables;
        }

        public static List<RdbField> RetrieveTableFields(String databaseType, String tableName)
        {
            if (databaseType == "mySql") return RetrieveMySqlTableFields(databaseType, tableName);
            if (databaseType == "firebird") return RetrieveFirebirdTableFields(databaseType, tableName);
            return null; // Caso databaseType seja inválido
        }

        public static List<RdbField> RetrieveMySqlTableFields(String databaseType, String tableName)
        {
            List<RdbField> fieldNames = new List<RdbField>();

            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection(databaseType);

            String query = "SELECT * FROM information_schema.columns WHERE table_schema = 'addoncontratos' AND table_name = '" + tableName + "'";
            MySqlCommand command = new MySqlCommand(query, dataConnector.MySqlConnection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String columnInfo = ((String)dataReader["COLUMN_TYPE"]).Trim();
                columnInfo = columnInfo.Replace(((String)dataReader["DATA_TYPE"]).Trim(), "");
                columnInfo = columnInfo.Replace("(", "");
                columnInfo = columnInfo.Replace(")", "");
                short columnLenght = 0;
                short.TryParse(columnInfo, out columnLenght);

                RdbField field = new RdbField();
                field.name = ((String)dataReader["COLUMN_NAME"]).Trim();
                field.type = ((String)dataReader["DATA_TYPE"]).Trim();
                field.size = columnLenght;

                fieldNames.Add(field);
            }
            dataReader.Close();

            dataConnector.CloseConnection();

            return fieldNames;
        }

        public static List<RdbField> RetrieveFirebirdTableFields(String databaseType, String tableName)
        {
            List<RdbField> fieldNames = new List<RdbField>();

            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection(databaseType);

            String query = "SELECT R.RDB$FIELD_NAME, T.RDB$TYPE_NAME, F.RDB$FIELD_LENGTH, R.RDB$NULL_FLAG " +
                    "FROM RDB$RELATION_FIELDS R, RDB$TYPES T, RDB$FIELDS F " +
                    "WHERE R.RDB$RELATION_NAME = '" + tableName + "' AND " +
                    "F.RDB$FIELD_NAME = R.RDB$FIELD_SOURCE AND " +
                    "T.RDB$FIELD_NAME = 'RDB$FIELD_TYPE' AND " +
                    "F.RDB$FIELD_TYPE = T.RDB$TYPE";

            FbCommand command = new FbCommand(query, dataConnector.FirebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                RdbField field = new RdbField();
                field.name = ((String)dataReader["RDB$FIELD_NAME"]).Trim();
                field.type = ((String)dataReader["RDB$TYPE_NAME"]).Trim();
                field.size = (short)dataReader["RDB$FIELD_LENGTH"];

                fieldNames.Add(field);
            }
            dataReader.Close();

            dataConnector.CloseConnection();

            return fieldNames;
        }

        /// <summary>
        /// Cria o cabeçalho no datatable
        /// </summary>
        private static void FillHeaders(DataTable table, String[] headers)
        {
            foreach (String field in headers)
            {
                table.Columns.Add(new DataColumn(field, Type.GetType("System.String")));
            }
        }

        /// <summary>
        /// Obtem o conteúdo do .CSV e cria as linhas correspondentes no DataTable
        /// </summary>
        private static void FillRows(DataTable table, String databaseType, String source, String[] headers)
        {
            DataConnector dataConnector = new DataConnector();
            dataConnector.OpenConnection(databaseType);

            DbCommand command = null;
            if (databaseType == "mySql") command = new MySqlCommand("SELECT * FROM " + source, dataConnector.MySqlConnection);
            if (databaseType == "firebird") command = new FbCommand("SELECT * FROM " + source, dataConnector.FirebirdConnection);

            DbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                DataRow newRow = table.NewRow();
                int ndx = 0;
                foreach (String field in headers)
                {
                    newRow[ndx] = dataReader[field].ToString();
                    ndx++;
                }
                table.Rows.Add(newRow);
            }
            dataReader.Close();

            dataConnector.CloseConnection();
        }

        /// <summary>
        /// Cria um datatable com os registros extraídos da tabela
        /// </summary>
        public static DataTable Fetch(String databaseType, String tableName, String[] headers)
        {
            DataTable returnTable = new DataTable();

            try
            {
                // Carrega os nomes dos campos
                FillHeaders(returnTable, headers);

                // Carrega os valores dos campos para cada linha
                FillRows(returnTable, databaseType, tableName, headers);
            }
            catch (Exception exc)
            {
                String lastError = exc.Message;
            }

            return returnTable;
        }
    }

}
