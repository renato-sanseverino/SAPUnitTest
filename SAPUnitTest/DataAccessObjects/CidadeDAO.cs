using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class CidadeDAO: DataAccessBase
    {
        public CidadeDAO(FbConnection firebirdConnection)
        {
            this.firebirdConnection = firebirdConnection;
        }

        public List<CidadeDTO> GetAll()
        {
            List<CidadeDTO> cityList = new List<CidadeDTO>();

            FbCommand command = new FbCommand("SELECT * FROM ICIDADES ORDER BY CIDADE", firebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                CidadeDTO cidade = new CidadeDTO();
                cidade.nome = GetStringValue(dataReader, "CIDADE");
                cidade.codigoFiscal = GetStringValue(dataReader, "CDFISCAL");
                cidade.uf = GetStringValue(dataReader, "UF");
                cidade.codigoRegiao = GetStringValue(dataReader, "CDREGIAO");
                cidade.observacao = GetStringValue(dataReader, "OBS");
                cidade.dataAtualizacao = GetStringValue(dataReader, "ATUALIZADO");
                cidade.codigoIBGE = GetStringValue(dataReader, "CDCIDADEIBGE");

                cityList.Add(cidade);
            }
            dataReader.Close();

            return cityList;
        }
    }

}
