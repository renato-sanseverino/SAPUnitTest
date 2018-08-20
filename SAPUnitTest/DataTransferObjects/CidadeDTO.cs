using System;


namespace DataTransferObjects
{
    public class CidadeDTO
    {
        public String nome;
        public String codigoFiscal;
        public String uf;
        public String codigoRegiao;
        public String observacao;
        public String dataAtualizacao;
        public String codigoIBGE;


        public CidadeDTO()
        {
        }

        public override string ToString()
        {
            return nome;
        }
    }

}
