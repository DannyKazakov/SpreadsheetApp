namespace SpreadsheetApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreateSpreadSheet = new System.Windows.Forms.Button();
            this.LoadFile = new System.Windows.Forms.Button();
            this.Ro = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nOfRows = new System.Windows.Forms.TextBox();
            this.nOfCols = new System.Windows.Forms.TextBox();
            this.spreadSheetName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreateSpreadSheet
            // 
            this.CreateSpreadSheet.BackColor = System.Drawing.Color.MintCream;
            this.CreateSpreadSheet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CreateSpreadSheet.Location = new System.Drawing.Point(168, 198);
            this.CreateSpreadSheet.Name = "CreateSpreadSheet";
            this.CreateSpreadSheet.Size = new System.Drawing.Size(186, 29);
            this.CreateSpreadSheet.TabIndex = 0;
            this.CreateSpreadSheet.Text = "Create";
            this.CreateSpreadSheet.UseVisualStyleBackColor = false;
            this.CreateSpreadSheet.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoadFile
            // 
            this.LoadFile.BackColor = System.Drawing.Color.MintCream;
            this.LoadFile.Location = new System.Drawing.Point(168, 245);
            this.LoadFile.Name = "LoadFile";
            this.LoadFile.Size = new System.Drawing.Size(186, 29);
            this.LoadFile.TabIndex = 1;
            this.LoadFile.Text = "Load";
            this.LoadFile.UseVisualStyleBackColor = false;
            this.LoadFile.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // Ro
            // 
            this.Ro.AutoSize = true;
            this.Ro.Location = new System.Drawing.Point(168, 117);
            this.Ro.Name = "Ro";
            this.Ro.Size = new System.Drawing.Size(47, 20);
            this.Ro.TabIndex = 2;
            this.Ro.Text = "Rows:";
            this.Ro.Click += new System.EventHandler(this.Ro_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Columns:";
            // 
            // nOfRows
            // 
            this.nOfRows.Location = new System.Drawing.Point(240, 117);
            this.nOfRows.Name = "nOfRows";
            this.nOfRows.Size = new System.Drawing.Size(114, 27);
            this.nOfRows.TabIndex = 4;
            // 
            // nOfCols
            // 
            this.nOfCols.Location = new System.Drawing.Point(240, 155);
            this.nOfCols.Name = "nOfCols";
            this.nOfCols.Size = new System.Drawing.Size(114, 27);
            this.nOfCols.TabIndex = 5;
            // 
            // spreadSheetName
            // 
            this.spreadSheetName.Location = new System.Drawing.Point(240, 78);
            this.spreadSheetName.Name = "spreadSheetName";
            this.spreadSheetName.Size = new System.Drawing.Size(114, 27);
            this.spreadSheetName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(115, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(304, 35);
            this.label3.TabIndex = 8;
            this.label3.Text = "Spreadsheet Creation Kit";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(519, 279);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.spreadSheetName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nOfCols);
            this.Controls.Add(this.nOfRows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ro);
            this.Controls.Add(this.LoadFile);
            this.Controls.Add(this.CreateSpreadSheet);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(537, 326);
            this.MinimumSize = new System.Drawing.Size(537, 326);
            this.Name = "Form1";
            this.Text = "Startup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button CreateSpreadSheet;
        private Button LoadFile;
        private Label Ro;
        private Label label1;
        private TextBox nOfRows;
        private TextBox nOfCols;
        private TextBox spreadSheetName;
        private Label label2;
        private Label label3;
    }
}