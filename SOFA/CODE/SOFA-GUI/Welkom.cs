using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Helper
    {
        public Welkom()
        {
            InitializeComponent(); MainBackend.moveCursor();
        }

        private void nextPage()
        {
            Pincode next = new Pincode();
            Hide();
            next.Show();
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
