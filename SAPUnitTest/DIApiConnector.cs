using System;
using System.IO;
using System.Xml;
using System.Reflection;
using SAPbobsCOM;


namespace ClassLibrary
{
    public class DIApiConnector
    {
        private SAPbobsCOM.Company sboCompany;

        public SAPbobsCOM.Company SboCompany
        {
            get { return sboCompany; }
        }

        public DIApiConnector()
        {
            String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(baseDir + @"\Xml\DataAccess.xml");
            XmlNodeList nodeList = xmlDoc.ChildNodes[1].ChildNodes;
            XmlNode sqlServerNode = null;
            XmlNode sapB1Node = null;
            BoDataServerTypes sqlServerType = BoDataServerTypes.dst_MSSQL;
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["name"].Value == "SQL Server 2005")
                {
                    sqlServerNode = node;
                    sqlServerType = BoDataServerTypes.dst_MSSQL2005;
                }
                if (node.Attributes["name"].Value == "SQL Server 2008")
                {
                    sqlServerNode = node;
                    sqlServerType = BoDataServerTypes.dst_MSSQL2008;
                }
                if (node.Attributes["name"].Value == "SAP B1")
                {
                    sapB1Node = node;
                }
            }
            if (sqlServerNode == null) return;
            if (sapB1Node == null) return;
            String dbServer = sqlServerNode.SelectSingleNode("server").InnerText;
            String dbUsername = sqlServerNode.SelectSingleNode("username").InnerText;
            String dbPassword = sqlServerNode.SelectSingleNode("password").InnerText;
            String database = sapB1Node.SelectSingleNode("database").InnerText;
            String sapUsername = sapB1Node.SelectSingleNode("username").InnerText;
            String sapPassword = sapB1Node.SelectSingleNode("password").InnerText;
            sboCompany = new SAPbobsCOM.Company();
            sboCompany.DbServerType = sqlServerType;
            sboCompany.Server = dbServer;
            sboCompany.DbUserName = dbUsername;
            sboCompany.DbPassword = dbPassword;
            sboCompany.CompanyDB = database;
            sboCompany.UserName = sapUsername;
            sboCompany.Password = sapPassword;
            sboCompany.UseTrusted = false;
            sboCompany.language = BoSuppLangs.ln_Portuguese_Br;
        }

        public void OpenConnection()
        {
            int connectionResult = sboCompany.Connect();
            if (connectionResult != 0)
            {
                int errorCode;
                String errorMessage;
                sboCompany.GetLastError(out errorCode, out errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public void CloseConnection()
        {
            sboCompany.Disconnect();
        }
    }

}
