namespace SpreadsheetApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpreadSheetForm s = new SpreadSheetForm();
            try
            {
                int rows = int.Parse(nOfRows.Text);
                int cols = int.Parse(nOfCols.Text);
                if (rows <= 0 || cols <= 0)
                    throw new Exception("Input of rows/columns must be bigger than 0!");
                if(spreadSheetName.Text==String.Empty)
                    throw new Exception("Must input the name of the spreadsheet");
                s.createForm(rows, cols,spreadSheetName.Text);
                s.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Invalid Arguments", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Ro_Click(object sender, EventArgs e)
        {

        }

        private void LoadFile_Click(object sender, EventArgs e)
        {
            Stream st;
            OpenFileDialog loadFile = new OpenFileDialog();
            loadFile.InitialDirectory = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory());
            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                if((st =loadFile.OpenFile()) != null)
                {
                    try
                    {
                        string file = loadFile.FileName;
                        if (!file.EndsWith(".sprdsht"))
                            throw new Exception("The file must be with \".sprdsht\" ending");
                        st.Flush();
                        st.Close();
                        SpreadSheetForm s = new SpreadSheetForm();
                        s.loadFile(file);
                        s.ShowDialog();
                    }
                    catch(Exception exx)
                    {
                        MessageBox.Show(exx.Message, "Wrong File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        st.Flush();
                        st.Close();
                    }
                }
            }
        }
    }
}