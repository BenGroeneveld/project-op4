using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Bedankt : Background
    {
        private bool printBon;

        public Bedankt(bool bon)
        {
            InitializeComponent();
            printBon = bon;
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

        public void startBedankt(bool printBon)
        {
            privateStartBedankt(printBon);
        }

        private void Bedankt_Load(object sender, EventArgs e)
        {
            startBedankt(printBon);
        }

        private void privateStartBedankt(bool printBon)
        {
            //Application.DoEvents();
            label1.Text = "Bedankt voor het pinnen!";

            if(printBon)
            {
                label1.Text += "\nVergeet uw geld en bon niet";
            }
            else
            {
                label1.Text += "\nVergeet uw geld niet";
            }
            
            Thread.Sleep(2500);
            MainBackend.restart();
        }
    }
}
