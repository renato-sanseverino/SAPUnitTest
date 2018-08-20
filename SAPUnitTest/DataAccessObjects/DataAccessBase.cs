using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;


namespace DataAccessObjects
{
    public abstract class DataAccessBase
    {
        protected MySqlConnection mySqlConnection;

        protected FbConnection firebirdConnection;

        protected SqlConnection sqlServerConnection;


        protected String GetStringValue(DbDataReader dataReader, String fieldName)
        {
            if (dataReader[fieldName] is DBNull) return null;
            return (String)dataReader[fieldName];
        }

        protected short GetSmallIntValue(DbDataReader dataReader, String fieldName)
        {
            if (dataReader[fieldName] is DBNull) return 0;
            return (short)dataReader[fieldName];
        }

        protected int GetIntegerValue(DbDataReader dataReader, String fieldName)
        {
            if (dataReader[fieldName] is DBNull) return 0;
            return (int)dataReader[fieldName];
        }

        protected Decimal GetDecimalValue(DbDataReader dataReader, String fieldName)
        {
            if (dataReader[fieldName] is DBNull) return 0;
            return (Decimal)dataReader[fieldName];
        }

        protected DateTime? GetDateTimeValue(DbDataReader dataReader, String fieldName)
        {
            if (dataReader[fieldName] is DBNull) return null;
            return (DateTime)dataReader[fieldName];
        }

        protected Boolean GetBooleanValue(DbDataReader dataReader, String fieldName)
        {
            String storedValue = (String)dataReader[fieldName];
            if (storedValue == "S") return true;
            if (storedValue == "N") return false;

            return false;
        }

        protected DateTime? ConvertDateTime(String value)
        {
            // Verifica se é uma string válida
            if (String.IsNullOrEmpty(value)) return null;

            // Verifica se o valor possui data e hora, duas partes pelo menos
            String[] tokenArray = value.Split(new Char[] {' '});
            if (tokenArray.Length < 2) return null;

            // Verifica se o valor está no formato esperado
            DateTime result;
            if (!DateTime.TryParse(tokenArray[0] + ' ' + tokenArray[1], out result)) return null;

            return result;
        }
    }

}
