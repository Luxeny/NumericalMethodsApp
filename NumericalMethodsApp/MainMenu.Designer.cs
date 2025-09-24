// Form1.Designer.cs
using System.Windows.Forms;

namespace DichotomyApp
{
    partial class MainMenu
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnDichotomy;
        private Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.btnDichotomy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDichotomy
            // 
            this.btnDichotomy.Location = new System.Drawing.Point(50, 30);
            this.btnDichotomy.Name = "btnDichotomy";
            this.btnDichotomy.Size = new System.Drawing.Size(180, 40);
            this.btnDichotomy.TabIndex = 0;
            this.btnDichotomy.Text = "Метод дихотомии";
            this.btnDichotomy.UseVisualStyleBackColor = true;
            this.btnDichotomy.Click += new System.EventHandler(this.btnDichotomy_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(50, 90);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 40);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Выйти";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainMenu
            // 
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDichotomy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "Меню";
            this.ResumeLayout(false);

        }
    }
}
