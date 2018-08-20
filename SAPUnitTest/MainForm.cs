using System;
using System.Windows.Forms;
using System.Collections.Generic;
using SAPbobsCOM;
using ClassLibrary;


namespace SAPUnitTest
{
    public partial class MainForm : Form
    {
        private String databaseType = "firebird";

        /*
        private DIApiConnector diApiConnector = new DIApiConnector();

        private void ConnectDI()
        {
            diApiConnector.OpenConnection();
            Company sboCompany = diApiConnector.SboCompany;
            Manufacturers manufacturers = (Manufacturers)sboCompany.GetBusinessObject(BoObjectTypes.oManufacturers);
            manufacturers.GetByKey(14);
            String manufacturerInfo = manufacturers.Code + " - " + manufacturers.ManufacturerName;
            diApiConnector.CloseConnection();
        }
        */

        public MainForm()
        {
            InitializeComponent();

            List<String> tableNames = RdbMetadata.RetrieveUserTables(databaseType);
            listBox1.Items.AddRange(tableNames.ToArray());
            listBox1.SelectedItem = null;
            listBox1.SelectionMode = SelectionMode.One;
            listBox1.SelectedValueChanged += new EventHandler(SelectedValueChanged);
        }

        void SelectedValueChanged(object sender, EventArgs e)
        {
            // Item selecionado na lista de tabelas
            String tableName = (String)listBox1.SelectedItem;

            // Traz informações sobre os campos da tabela selecionada
            List<RdbField> fieldList = RdbMetadata.RetrieveTableFields(databaseType, tableName);
            listBox2.Items.Clear();
            listBox2.Items.AddRange(fieldList.ToArray());

            List<String> headers = new List<String>();
            foreach(RdbField field in fieldList) headers.Add(field.name);

            // Recupera os registros da tabela
            if (!String.IsNullOrEmpty(tableName))
                dataGridView1.DataSource = RdbMetadata.Fetch(databaseType, tableName, headers.ToArray());
        }
    }

}
