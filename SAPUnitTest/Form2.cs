using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using org.in2bits.MyXls;
using ClassLibrary;
using DataAccessObjects;
using DataTransferObjects;



namespace SAPUnitTest
{
    public partial class Form2 : Form
    {
        private String docCode = "BOE20003";

        private String bitmapPath = @"\\DATADB\Bitmaps\";

        private static FileStream fileStream;

        private static XlsDocument document;

        private static Worksheet worksheet;

        private DataConnector dataConnector;

        public Form2()
        {
            InitializeComponent();
            dataConnector = new DataConnector();

            dataConnector.OpenConnection("sqlServer");
            ReportingDocumentDAO reportingDocumentDAO = new ReportingDocumentDAO(dataConnector.SqlServerConnection);
            ReportingElementDAO reportingElementDAO = new ReportingElementDAO(dataConnector.SqlServerConnection);

            ReportingDocumentDTO reportDoc = reportingDocumentDAO.GetReport(docCode);
            Bitmap bmp = new Bitmap(reportDoc.Width, reportDoc.Height);
            Graphics graphs = Graphics.FromImage(bmp);
            graphs.Clear(System.Drawing.Color.White);

            List<ReportingElementDTO> elementList = reportingElementDAO.GetElements(docCode, "Type=10");
            foreach (ReportingElementDTO element in elementList)
            {
                Rectangle rect = new Rectangle(element.left, element.top, element.width, element.height);
                graphs.FillRectangle(new SolidBrush(System.Drawing.Color.LightYellow), rect);
                if (element.borderTop > 0) graphs.DrawRectangle(new Pen(System.Drawing.Color.Black, element.borderTop), rect);
                if (element.borderBottom > 0) graphs.DrawRectangle(new Pen(System.Drawing.Color.Black, element.borderBottom), rect);
                if (element.borderLeft > 0) graphs.DrawRectangle(new Pen(System.Drawing.Color.Black, element.borderLeft), rect);
                if (element.borderRight > 0) graphs.DrawRectangle(new Pen(System.Drawing.Color.Black, element.borderRight), rect);
            }

            List<ReportingElementDTO> imageList = reportingElementDAO.GetElements(docCode, "Type=11");
            foreach (ReportingElementDTO image in imageList)
            {
                Rectangle rect = new Rectangle(image.left, image.top, image.width, image.height);
                Bitmap bitmap = new Bitmap(bitmapPath + image.caption);
                graphs.DrawImage(bitmap, rect);
            }

            graphs.Flush();
            imgReport.Image = bmp;

            dataConnector.CloseConnection();
        }

        /// <summary>
        /// Cria a tabela que receberá os dados na planilha
        /// </summary>
        public static void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount)
        {
            // Define o nome da planilha e a largura das colunas
            fileStream = new FileStream(@"C:\work\CadastroProdutos.xls", FileMode.Create, FileAccess.ReadWrite);
            document = new XlsDocument();
            worksheet = document.Workbook.Worksheets.Add("Cadastro de Produtos");
            ColumnInfo info = new ColumnInfo(document, worksheet);
            info.ColumnIndexStart = 1;
            info.ColumnIndexEnd = (ushort)columnNames.Length;
            info.Width = 5100;
            worksheet.AddColumnInfo(info);

            // Prepara o plano de fundo da planilha
            for (int row = 1; row < rowCount + 10; row++)
                for (int col = 1; col < columnNames.Length + 3; col++) SetCellPattern(row, col);

            // Insere o título (agrupa as celulas que fazem parte do título)
            worksheet.Cells.Merge(5, 5, 2, columnNames.Length + 1);
            Cell titleCell = worksheet.Cells.Add(5, 2, "Cadastro de Produtos");
            titleCell.HorizontalAlignment = HorizontalAlignments.Centered;
            titleCell.Font.Height = 400;
            titleCell.Font.Bold = true;
            titleCell.Font.FontFamily = FontFamilies.Roman;

            // Cria celulas com os nomes das colunas
            for (int ndx = 0; ndx < columnNames.Length; ndx++)
            {
                Cell columnName = worksheet.Cells.Add(6, ndx + 2, columnNames[ndx]);
                columnName.Font.Bold = true;
                columnName.PatternColor = Colors.Default1F;
                columnName.Pattern = 1;
                SetCellBorder(columnName);
            }
        }

        private static void SetCellPattern(int row, int col)
        {
            if (col > 255) return;
            if (row > 65535) return;

            Cell newCell = worksheet.Cells.Add(row, col, "");
            newCell.VerticalAlignment = VerticalAlignments.Centered;
            newCell.HorizontalAlignment = HorizontalAlignments.Centered;
            newCell.PatternColor = Colors.White;
            newCell.Pattern = 1;
        }

        private static void SetCellBorder(Cell cell)
        {
            cell.TopLineColor = Colors.Black;
            cell.BottomLineColor = Colors.Black;
            cell.RightLineColor = Colors.Black;
            cell.LeftLineColor = Colors.Black;

            cell.TopLineStyle = 1;
            cell.BottomLineStyle = 1;
            cell.RightLineStyle = 1;
            cell.LeftLineStyle = 1;
        }

        /// <summary>
        /// Insere uma linha na tabela de dados da planilha
        /// </summary>
        public static void InsertRow(int rowIndex, Object[] cells)
        {
            for (int ndx = 0; ndx < cells.Length; ndx++)
            {
                Cell newCell = worksheet.Cells.Add(rowIndex + 7, ndx + 2, (String)cells[ndx]);
                newCell.HorizontalAlignment = HorizontalAlignments.Left;
                newCell.VerticalAlignment = VerticalAlignments.Centered;
                SetCellBorder(newCell);
            }
        }

        private void ExportProducts()
        {
            dataConnector.OpenConnection("firebird");

            int currentRow = 0;
            CreateDataTable(new String[] { "Código", "Descrição", "Unidade" }, new int[] { 100, 500, 50 }, 2000);
            ProdutoDAO produtoDAO = new ProdutoDAO(dataConnector.FirebirdConnection);
            List<ProdutoDTO> productList = produtoDAO.GetAll();
            foreach (ProdutoDTO product in productList)
            {
                InsertRow(currentRow, new Object[] { product.codigo, product.descricao, product.unidade });
                currentRow++;
            }
            document.Save(fileStream);
            fileStream.Close();

            dataConnector.CloseConnection();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ExportProducts();
            MessageBox.Show("Exportação concluída");
        }
    }

}
