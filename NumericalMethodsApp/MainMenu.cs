// Form1.cs
using System;
using System.Windows.Forms;

namespace DichotomyApp
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnDichotomy_Click(object sender, EventArgs e)
        {
            DihotomyForm f2 = new DihotomyForm();
            f2.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}