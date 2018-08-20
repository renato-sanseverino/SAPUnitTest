using System;


namespace ClassLibrary
{
    public class SystemError
    {
        public int id;
        public String ocorrencia;
        public DateTime dataOcorrencia;
        public int severidade;
        public String localizacao;

        public SystemError()
        {
            // construtor sem parâmetros
        }

        public SystemError(String ocorrencia, int severidade, String localizacao)
        {
            this.ocorrencia = ocorrencia;
            this.dataOcorrencia = DateTime.Now;
            this.severidade = severidade;
            this.localizacao = localizacao;
        }
    }

}
