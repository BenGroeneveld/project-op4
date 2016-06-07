using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Helper
    {
        public static bool waitingText = false;
        public static bool restart { get; set; }

        public Welkom()
        {
            InitializeComponent();
        }

        private void nextPage()
        {
            Pincode next = new Pincode();
            next.Show();
            this.Hide();
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
            MainBackend.doWelkom();
            nextPage();
        }
    }
}
