using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Welkom : Background
    {
        public static bool waitingText = false;
        public static bool restart { get; set; }

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
            restart = false;
            MainBackend.doWelkom();
            nextPage();
        }

        private void Welkom_Load(object sender, EventArgs e)
        {
            //Application.DoEvents();
        }
    }
}
