using System;


namespace DataTransferObjects
{
    public class ProdutoDTO
    {
        public String codigo;
        public String descricao;
        public String unidade;


        public ProdutoDTO()
        {
        }

        public override string ToString()
        {
            return codigo + " - " + descricao;
        }
    }

}
