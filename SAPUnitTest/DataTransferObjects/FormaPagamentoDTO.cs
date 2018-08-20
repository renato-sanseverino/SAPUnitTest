using System;


namespace DataTransferObjects
{
    public class FormaPagamentoDTO
    {
        public String codigo;
        public String nome;
        public DateTime? atualizadoEm;
        public Boolean boleto;
        public Boolean ECFbaixaRec;
        public Boolean parcelar;
        public String formaPagamentoECF;


        public FormaPagamentoDTO()
        {
        }

        public override string ToString()
        {
            return nome;
        }
    }

}
