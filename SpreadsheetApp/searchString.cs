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
    public partial class searchString : Form
    {
       private string str;
        public searchString()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (searchStringTextBox.Text != null)
                    str = searchStringTextBox.Text;
                else
                    throw new Exception("string must not be empty!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Empty String", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
        public string getString()
        {
            return str;
        }
    }
}
