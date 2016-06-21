using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Form
    {
        public Pas pas;
        public MainBackend mainBackend;

        public bool waitingText = false;
        public bool restart { get; set; }

        public Welkom()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
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
            nextPage();
        }
    }
}
