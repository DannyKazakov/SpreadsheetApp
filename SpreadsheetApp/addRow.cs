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
    public partial class addRow : Form
    {
        private int row=-1;
        public addRow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                row = int.Parse(numberOfRow.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "invalid argument", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public int getRow()
        {
            return row;
        }
    }
}
