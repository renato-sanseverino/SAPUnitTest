using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class ProdutoDAO: DataAccessBase
    {
        public ProdutoDAO(FbConnection firebirdConnection)
        {
            this.firebirdConnection = firebirdConnection;
        }

        public List<ProdutoDTO> GetAll()
        {
            List<ProdutoDTO> productList = new List<ProdutoDTO>();

            FbCommand command = new FbCommand("SELECT * FROM IPRODUTO ORDER BY CDPRODUTO", firebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                ProdutoDTO produto = new ProdutoDTO();
                produto.codigo = GetStringValue(dataReader, "CDPRODUTO");
                produto.descricao = GetStringValue(dataReader, "NMPRODUTO");
                produto.unidade = GetStringValue(dataReader, "CDUNIDADE");

                productList.Add(produto);
            }
            dataReader.Close();

            return productList;
        }
    }

}
