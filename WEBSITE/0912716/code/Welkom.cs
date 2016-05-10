using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Gui
{
    public partial class Welkom : Form
    {
        public Welkom()
        {
            InitializeComponent();
        }

        private void nextPage()
        {
            Close();
        }

        private void Welkom_Shown(object sender, EventArgs e)
        {
            startWelkom();
        }

        public void startWelkom()
        {
            privateStartWelkom();
        }

        private void privateStartWelkom()
        {
            Application.DoEvents();
            MainBackend.doWelkom();
            nextPage();
        }
    }
}
