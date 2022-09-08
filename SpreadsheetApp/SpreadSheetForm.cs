using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetApp
{
    public partial class SpreadSheetForm : Form
    {
        private SharableSpreadSheet SpreadSheet;
        private string fileName;
        public SpreadSheetForm()
        {
            InitializeComponent();
        }

        internal void createForm(int rows, int cols, string name)
        {
            SpreadSheet = new SharableSpreadSheet(rows, cols);
            createDataGrid(rows, cols);
            fileName = name;

        }
        private void createDataGrid(int rows, int cols)
        {
            
            for (int i = 0; i < cols; i++)
                SpreedSheetDataGrid.Columns.Add(i.ToString(), i.ToString());
            for (int i = 0; i < rows; i++)
            {
                SpreedSheetDataGrid.Rows.Add();
                SpreedSheetDataGrid.Rows[i].HeaderCell.Value = i.ToString();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /* the edits in the data grid are transfered to the spreadsheet*/
        private void SpreedSheetDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            String str = SpreedSheetDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value==null ? null :
                SpreedSheetDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            SpreadSheet.setCell(e.RowIndex, e.ColumnIndex, str);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("click about");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help helpForm = new Help();
            helpForm.ShowDialog();
        }

        internal void loadFile(string file)
        {
            //creating the grid
            SpreadSheet = new SharableSpreadSheet();
            SpreadSheet.load(file);
            int rows = SpreadSheet.getSize().Item1;
            int cols = SpreadSheet.getSize().Item2;
            createDataGrid(rows, cols);
            fileName = file.Replace(".sprdsht","");
            FillInTheGrid(rows,cols);
        }
        private void FillInTheGrid(int rows, int cols) {
            //filling in the grid
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    SpreedSheetDataGrid.Rows[i].Cells[j].Value = SpreadSheet.getCell(i, j);
                }
            }

        }
        /*save current file*/
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadSheet.save(fileName);
            MessageBox.Show("Spreadsheet saved successfully", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        /*load file*/
        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stream st;
            OpenFileDialog loadFile = new OpenFileDialog();
            loadFile.InitialDirectory = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory());
            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                if ((st = loadFile.OpenFile()) != null)
                {
                    try
                    {
                        string file = loadFile.FileName;
                        if (!file.EndsWith(".sprdsht"))
                            throw new Exception("The file must be with \".sprdsht\" ending");
                        st.Flush();
                        st.Close();
                        SpreedSheetDataGrid.Columns.Clear();
                        this.loadFile(file);

                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "Wrong File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        st.Flush();
                        st.Close();
                    }
                }
            }
        }
        /*save file as..*/
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory());

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (saveFile.FileName != null)
                    {
                        SpreadSheet.save(saveFile.FileName);
                        fileName = saveFile.FileName; 
                    }
                        
                    else
                        throw new Exception("File name can't be empty!");
                }
                catch (Exception exx)
                {
                    MessageBox.Show(exx.Message, "Empty String", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        /*search string in spreadsheet*/
        private void searchStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchString form = new searchString();
            form.ShowDialog();
            string str = form.getString();
            if (str == null || str=="")
                return;
            try
            {
                Tuple<int, int> cell = SpreadSheet.searchString(str);
                if (cell == null)
                    throw new Exception("Didn't find the string: " + str);
                else
                {
                    SpreedSheetDataGrid.Rows[cell.Item1].Cells[cell.Item2].Selected = true;
                    MessageBox.Show("Found The string " + "\"" + str + "\" " + "at [" + cell.Item1 + ", " + cell.Item2 + "]"
                        , "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "String not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRow addRowForm = new addRow();
            addRowForm.ShowDialog();
            int newRow = addRowForm.getRow(),rows,cols;
            if (newRow == -1)
                return;
            try
            {
                if (newRow > SpreadSheet.getSize().Item1 - 1)
                    throw new Exception("can't add a row after row " + newRow + ". Exceeds length.");
                SpreadSheet.addRow(newRow);
                SpreedSheetDataGrid.Columns.Clear();
                rows = SpreadSheet.getSize().Item1;
                cols = SpreadSheet.getSize().Item2;
                createDataGrid(rows, cols);
                FillInTheGrid(rows, cols);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void addColToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addCol addColForm = new addCol();
            addColForm.ShowDialog();
            int newCol = addColForm.getCol(), rows, cols;
            if (newCol == -1)
                return;
            try
            {
                if (newCol > SpreadSheet.getSize().Item2 - 1)
                    throw new Exception("can't add a column after column " + newCol + ". Exceeds length.");
                SpreadSheet.addCol(newCol);
                SpreedSheetDataGrid.Columns.Clear();
                rows = SpreadSheet.getSize().Item1;
                cols = SpreadSheet.getSize().Item2;
                createDataGrid(rows, cols);
                FillInTheGrid(rows, cols);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void setAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAll setallform = new setAll();
            setallform.ShowDialog();
            bool sensitivity;
            string oldstr, newstr;
            int i = setallform.getCaseSensitivity(),rows,cols;
            if (i == 0)
                sensitivity = false;
            else if (i == 1)
                sensitivity = true;
            else
                return;
            oldstr = setallform.getOldStr();
            newstr = setallform.getNewStr();
            if (newstr == null || oldstr == null)
                return;
            SpreadSheet.setAll(oldstr, newstr, sensitivity);
            SpreedSheetDataGrid.Columns.Clear();
            rows = SpreadSheet.getSize().Item1;
            cols = SpreadSheet.getSize().Item2;
            createDataGrid(rows, cols);
            FillInTheGrid(rows, cols);
            MessageBox.Show("all occurrences of \""+oldstr+"\" replaced with \""+newstr+"\" successfully",
                "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
