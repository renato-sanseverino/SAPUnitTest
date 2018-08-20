using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class ReportingDocumentDAO: DataAccessBase
    {
        public ReportingDocumentDAO(SqlConnection sqlConnection)
        {
            this.sqlServerConnection = sqlConnection;
        }

        public ReportingDocumentDTO GetReport(String docCode)
        {
            ReportingDocumentDTO report = null;

            String query = "SELECT DocCode, DocName, PaperSize, Width, Height FROM RDOC WHERE DocCode='" + docCode + "'";
            SqlCommand command = new SqlCommand(query, sqlServerConnection);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                report = new ReportingDocumentDTO();
                report.DocCode = GetStringValue(dataReader, "DocCode");
                report.DocName = GetStringValue(dataReader, "DocName");
                report.PaperSize = GetStringValue(dataReader, "PaperSize");
                report.Width = GetSmallIntValue(dataReader, "Width");
                report.Height = GetSmallIntValue(dataReader, "Height");
            }
            dataReader.Close();

            return report;
        }

        public List<ReportingDocumentDTO> GetReports(String filter)
        {
            List<ReportingDocumentDTO> reportList = new List<ReportingDocumentDTO>();

            String query = "SELECT DocCode, DocName, PaperSize, Width, Height FROM RDOC WHERE " + filter;
            if (String.IsNullOrEmpty(filter)) query = "SELECT DocCode, DocName, PaperSize FROM RDOC";
            SqlCommand command = new SqlCommand(query, sqlServerConnection);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                ReportingDocumentDTO report = new ReportingDocumentDTO();
                report.DocCode = GetStringValue(dataReader, "DocCode");
                report.DocName = GetStringValue(dataReader, "DocName");
                report.PaperSize = GetStringValue(dataReader, "PaperSize");
                report.Width = GetSmallIntValue(dataReader, "Width");
                report.Height = GetSmallIntValue(dataReader, "Height");

                reportList.Add(report);
            }
            dataReader.Close();

            return reportList;
        }
    }

}
