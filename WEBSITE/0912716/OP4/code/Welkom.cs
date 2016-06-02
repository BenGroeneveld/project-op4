using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Background
    {
        public static bool waitingText = false;
        public Welkom()
        {
            InitializeComponent();
        }

        private void nextPage()
        {
            Pincode next = new Pincode();
            next.Show();
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
            if(waitingText)
            {
                label1.Text = "Een moment geduld a.u.b.";
            }
            nextPage();
        }

        private void Welkom_Load(object sender, EventArgs e)
        {
            //Application.DoEvents();
        }
    }
}
