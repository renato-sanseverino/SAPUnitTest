using System;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Common;
using System.Windows.Forms;


namespace SAPUnitTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            /*
                http://stackoverflow.com/questions/754436/odbc-dbf-files-in-c-sharp/
                http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework/topic62548.aspx
            */
        }

        public static DataTable GetDataTableDBF(String strFileName)
        {
            String connectionString = @"Provider=VFPOLEDB.1; Data Source=" + System.IO.Path.GetFullPath(strFileName).Replace(System.IO.Path.GetFileName(strFileName), "") + ";";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            String query = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
            System.Data.DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Arquivos DBF (*.dbf)|*.dbf";
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel) return;

            txtFilename.Text = fileDialog.FileName;
            dataGridView1.DataSource = GetDataTableDBF(fileDialog.FileName);
        }
    }

}
