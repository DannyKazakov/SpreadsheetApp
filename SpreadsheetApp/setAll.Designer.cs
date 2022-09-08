namespace SpreadsheetApp
{
    partial class setAll
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.oldstrTextBox = new System.Windows.Forms.TextBox();
            this.newstrTextBox = new System.Windows.Forms.TextBox();
            this.ReplaceButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Truebox = new System.Windows.Forms.CheckBox();
            this.Falsebox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old string:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "New String:";
            // 
            // oldstrTextBox
            // 
            this.oldstrTextBox.Location = new System.Drawing.Point(120, 3);
            this.oldstrTextBox.Name = "oldstrTextBox";
            this.oldstrTextBox.Size = new System.Drawing.Size(139, 27);
            this.oldstrTextBox.TabIndex = 2;
            // 
            // newstrTextBox
            // 
            this.newstrTextBox.Location = new System.Drawing.Point(120, 48);
            this.newstrTextBox.Name = "newstrTextBox";
            this.newstrTextBox.Size = new System.Drawing.Size(139, 27);
            this.newstrTextBox.TabIndex = 3;
            // 
            // ReplaceButton
            // 
            this.ReplaceButton.Location = new System.Drawing.Point(265, 3);
            this.ReplaceButton.Name = "ReplaceButton";
            this.ReplaceButton.Size = new System.Drawing.Size(121, 117);
            this.ReplaceButton.TabIndex = 4;
            this.ReplaceButton.Text = "Replace";
            this.ReplaceButton.UseVisualStyleBackColor = true;
            this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Case sensitive:";
            // 
            // Truebox
            // 
            this.Truebox.AutoSize = true;
            this.Truebox.Location = new System.Drawing.Point(120, 96);
            this.Truebox.Name = "Truebox";
            this.Truebox.Size = new System.Drawing.Size(59, 24);
            this.Truebox.TabIndex = 6;
            this.Truebox.Text = "True";
            this.Truebox.UseVisualStyleBackColor = true;
            // 
            // Falsebox
            // 
            this.Falsebox.AutoSize = true;
            this.Falsebox.Location = new System.Drawing.Point(196, 96);
            this.Falsebox.Name = "Falsebox";
            this.Falsebox.Size = new System.Drawing.Size(63, 24);
            this.Falsebox.TabIndex = 7;
            this.Falsebox.Text = "False";
            this.Falsebox.UseVisualStyleBackColor = true;
            // 
            // setAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 121);
            this.Controls.Add(this.Falsebox);
            this.Controls.Add(this.Truebox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ReplaceButton);
            this.Controls.Add(this.newstrTextBox);
            this.Controls.Add(this.oldstrTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(585, 110);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(407, 168);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(407, 168);
            this.Name = "setAll";
            this.Text = "setAll";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox oldstrTextBox;
        private TextBox newstrTextBox;
        private Button ReplaceButton;
        private Label label3;
        private CheckBox Truebox;
        private CheckBox Falsebox;
    }
}