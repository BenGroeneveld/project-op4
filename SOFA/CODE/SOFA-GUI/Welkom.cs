using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Helper
    {
        public Welkom()
        {
            InitializeComponent();
        }

        private void nextPage()
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                Pincode next = new Pincode();
                Hide();
                next.Show();
            }
            else
            {
                MainBackend.restart();
            }
        }

        public void startWelkom()
        {
            privateStartWelkom();
        }

        private void privateStartWelkom()
        {
            MainBackend.doWelkom();
            nextPage();
        }

        private void Welkom_Load(object sender, EventArgs e)
        {
            Activate();
            startWelkom();
        }
    }
}
