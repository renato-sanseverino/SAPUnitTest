using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class ReportingElementDAO: DataAccessBase
    {
        public ReportingElementDAO(SqlConnection sqlConnection)
        {
            this.sqlServerConnection = sqlConnection;
        }

        public List<ReportingElementDTO> GetElements(String docCode, String filter)
        {
            List<ReportingElementDTO> elementList = new List<ReportingElementDTO>();

            String query = "SELECT * FROM RITM WHERE DocCode = '" + docCode + "'";
            if (!String.IsNullOrEmpty(filter)) query += " AND " + filter;
            SqlCommand command = new SqlCommand(query, sqlServerConnection);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                ReportingElementDTO element = new ReportingElementDTO();
                element.left = GetSmallIntValue(dataReader, "ItemLeft");
                element.top = GetSmallIntValue(dataReader, "ItemTop");
                element.width = GetSmallIntValue(dataReader, "Width");
                element.height = GetSmallIntValue(dataReader, "Height");
                element.caption = GetStringValue(dataReader, "ItemStr");
                element.borderLeft = GetSmallIntValue(dataReader, "leftLine");
                element.borderRight = GetSmallIntValue(dataReader, "rightLine");
                element.borderTop = GetSmallIntValue(dataReader, "topLine");
                element.borderBottom = GetSmallIntValue(dataReader, "bottomLine");

                elementList.Add(element);
            }
            dataReader.Close();

            return elementList;
        }
    }

}
            