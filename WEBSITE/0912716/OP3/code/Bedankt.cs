using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Bedankt : Form
    {
        private bool printBon;

        public Bedankt(bool bon)
        {
            InitializeComponent();
            printBon = bon;
        }

        private void Bedankt_Shown(object sender, EventArgs e)
        {
            startBedankt(printBon);
        }

        public void startBedankt(bool printBon)
        {
            privateStartBedankt(printBon);
        }

        private void privateStartBedankt(bool printBon)
        {
            Application.DoEvents();
            label1.Text = "Bedankt voor het pinnen!";

            if(printBon)
            {
                label1.Text += "\nVergeet uw geld en bon niet";
            }
            else
            {
                label1.Text += "\nVergeet uw geld niet";
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            MainBackend.restart();
        }
    }
}
