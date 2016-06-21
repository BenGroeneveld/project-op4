using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Bedankt : Form
    {
        private DatabaseConnection dbConnect = new DatabaseConnection();
        private MainBackend mainBackend = new MainBackend();
        private Dispenser dispenser = new Dispenser();
        private Rekening rekening = new Rekening();
        private Pas pas = new Pas();
        private Klant klant = new Klant();
        private Transactie transactie = new Transactie();

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
            string balans = (Bedrag / 100).ToString();
            balans += ",00";

            if(Bon)
            {
                label1.Text += "\nVergeet uw geld en bon niet";
                MainBackend.printBon(balans, pas.RekeningID, pas.PasID);
            }
            else
            {
                label1.Text += "\nVergeet uw geld niet";
            }

            mainBackend.doTransactie(Saldo, pas.PasID);
            dispenser.dispense();

            Thread.Sleep(1000);
            MainBackend.restart();
        }
    }
}
