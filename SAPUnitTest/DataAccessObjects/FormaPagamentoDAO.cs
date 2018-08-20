using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class FormaPagamentoDAO: DataAccessBase
    {
        public FormaPagamentoDAO(FbConnection firebirdConnection)
        {
            this.firebirdConnection = firebirdConnection;
        }

        public List<FormaPagamentoDTO> GetAll()
        {
            List<FormaPagamentoDTO> paymntMehtodList = new List<FormaPagamentoDTO>();

            FbCommand command = new FbCommand("SELECT * FROM IFORMAPAGTO", firebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                FormaPagamentoDTO paymntMehtod = new FormaPagamentoDTO();
                paymntMehtod.codigo = GetStringValue(dataReader, "CDFORMAPAGTO");
                paymntMehtod.nome = GetStringValue(dataReader, "NMFORMAPAGTO");
                paymntMehtod.atualizadoEm = ConvertDateTime(GetStringValue(dataReader, "ATUALIZADO"));
                paymntMehtod.boleto = GetBooleanValue(dataReader, "TFBOLETO");
                paymntMehtod.ECFbaixaRec = GetBooleanValue(dataReader, "TFECFBAIXAREC");
                paymntMehtod.parcelar = GetBooleanValue(dataReader, "TFPARCELAR");
                paymntMehtod.formaPagamentoECF = GetStringValue(dataReader, "CDFORMAPAGTOECF");

                paymntMehtodList.Add(paymntMehtod);
            }
            dataReader.Close();

            return paymntMehtodList;
        }
    }

}
