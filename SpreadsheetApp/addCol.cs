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
    public partial class addCol : Form
    {
        private int col = -1;
        public addCol()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                col = int.Parse(numberOfColumn.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "invalid argument", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public int getCol()
        {
            return col;
        }
    }
}
