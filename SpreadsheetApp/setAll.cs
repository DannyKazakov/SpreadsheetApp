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
    public partial class setAll : Form
    {
        private string oldStr = null;
        private string newStr = null;
        private int caseSensitive= -1;
        public setAll()
        {
            InitializeComponent();
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Falsebox.Checked == false && Truebox.Checked == false)
                    throw new Exception("Must check on which sensitivity to do the replacement.");
                if (Falsebox.Checked == true && Truebox.Checked == true)
                    throw new Exception("Must check True or False.");
                if (Falsebox.Checked == true)
                    caseSensitive = 0;
                else
                    caseSensitive = 1;
                if(oldstrTextBox.Text == String.Empty)
                    throw new Exception("Old string cannot be empty.");
                else
                    oldStr = oldstrTextBox.Text;
                if (newstrTextBox.Text == String.Empty)
                    throw new Exception("New string cannot be empty.");
                else
                    newStr = newstrTextBox.Text;

                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public string getOldStr()
        {
            return oldStr;
        }
        public string getNewStr()
        {
            return newStr;
        }
        public int getCaseSensitivity()
        {
            return caseSensitive;
        }
    }
}
