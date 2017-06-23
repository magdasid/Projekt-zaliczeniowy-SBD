namespace SemiProject
{
    partial class Form3
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
            this.drinkid = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.err = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wpisz id napoju, który chcesz usunąć:";
            // 
            // drinkid
            // 
            this.drinkid.Location = new System.Drawing.Point(207, 18);
            this.drinkid.Name = "drinkid";
            this.drinkid.Size = new System.Drawing.Size(166, 20);
            this.drinkid.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Zatwierdź";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // err
            // 
            this.err.AutoSize = true;
            this.err.Location = new System.Drawing.Point(109, 60);
            this.err.Name = "err";
            this.err.Size = new System.Drawing.Size(28, 13);
            this.err.TabIndex = 3;
            this.err.Text = "error";
            this.err.Visible = false;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 103);
            this.Controls.Add(this.err);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.drinkid);
            this.Controls.Add(this.label1);
            this.Name = "Form3";
            this.Text = "Usuwanie napoju";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox drinkid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label err;
    }
}