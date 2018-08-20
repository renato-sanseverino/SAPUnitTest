using System;
using System.Windows.Forms;
using System.Collections.Generic;
using FlowManager;
using ClassLibrary;
using DataAccessObjects;
using DataTransferObjects;


namespace SAPUnitTest
{
    public partial class Form1 : Form
    {
        private DataConnector dataConnector;


        public Form1()
        {
            InitializeComponent();
            dataConnector = new DataConnector();

            dataConnector.OpenConnection("all");
            // Recupera os relatórios em formato Crystal Reports ( Categoria C )
            ReportingDocumentDAO reportingDocumentDAO = new ReportingDocumentDAO(dataConnector.SqlServerConnection);
            List<ReportingDocumentDTO> reportList = reportingDocumentDAO.GetReports("Category = 'C'");
            // Recupera dados do firebird
            ProdutoDAO productDAO = new ProdutoDAO(dataConnector.FirebirdConnection);
            List<ProdutoDTO> productList = productDAO.GetAll();
            CidadeDAO cidadeDAO = new CidadeDAO(dataConnector.FirebirdConnection);
            List<CidadeDTO> cityList = cidadeDAO.GetAll();
            FormaPagamentoDAO formaPagamentoDAO = new FormaPagamentoDAO(dataConnector.FirebirdConnection);
            List<FormaPagamentoDTO> paymentMethodList = formaPagamentoDAO.GetAll();
            dataConnector.CloseConnection();

            cmbReport.Items.AddRange(reportList.ToArray());
            cmbReport.SelectedIndex = 0;

            cmbProduct.Items.AddRange(productList.ToArray());
            cmbProduct.SelectedIndex = 0;

            cmbCity.Items.AddRange(cityList.ToArray());
            cmbCity.SelectedIndex = 0;

            cmbPaymentMethod.Items.AddRange(paymentMethodList.ToArray());
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ExecutionBlock task1 = new ExecutionBlock();
            task1.name = "Origem";
            ExecutionBlock task2 = new ExecutionBlock();
            task2.name = "Destino";

            Loop loop = new Loop();
            loop.name = "Aguarda";
            loop.excutionBlock = new ExecutionBlock();
            loop.exitCondition = "Confirmado";

            Decision decision = new Decision();
            decision.name = "Valida";
            decision.condition = "Validação OK";
            decision.positiveResult = task1;
            decision.negativeResult = task2;

            Workflow workflow = new Workflow("Processo 1");
            workflow.AddElement(task1);
            workflow.AddElement(task2);
            workflow.AddElement(decision);
            workflow.AddElement(loop);
            workflow.CreateRelation(task1, task2);
            workflow.CreateRelation(decision, loop);
            List<FlowElement> previousElements = workflow.GetPreviousElements(task2);
            List<FlowElement> nextElements = workflow.GetNextElements(decision);

            workflow.Store();
        }
    }

}
