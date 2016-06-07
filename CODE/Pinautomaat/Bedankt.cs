using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Bedankt : Helper
    {
        private bool Bon { get; set; }
        private int Saldo { get; set; }
        private int Bedrag { get; set; }

        public Bedankt(bool bon, int saldo, int bedrag)
        {
            InitializeComponent();

            Bon = bon;
            Saldo = saldo;
            Bedrag = bedrag;
        }

        public void startBedankt()
        {
            privateStartBedankt();
        }

        private void Bedankt_Load(object sender, EventArgs e)
        {
            startBedankt();
        }

        private void privateStartBedankt()
        {
            label1.Text = "Bedankt voor het pinnen!";
            string strBedrag = (Bedrag / 100).ToString();
            strBedrag += ",00";

            if(Bon)
            {
                label1.Text += "\nVergeet uw geld en bon niet";
                MainBackend.printBon(strBedrag, Program.StrRekeningID, Program.Rfid);
            }
            else
            {
                label1.Text += "\nVergeet uw geld niet";
            }

            MainBackend.doTransactie(Saldo, Program.Rfid);
            Dispenser.dispense(Bedrag);

            Thread.Sleep(1000);
            MainBackend.restart();
        }
    }
}
