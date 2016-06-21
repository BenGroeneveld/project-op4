using System;
using System.Windows.Forms;
using System.Threading;

namespace Pinautomaat
{
    public partial class MainMenu  : Form
    {
        private DatabaseConnection dbConnect = new DatabaseConnection();
        private MainBackend mainBackend = new MainBackend();
        private Dispenser dispenser = new Dispenser();
        private Rekening rekening = new Rekening();
        private Pas pas = new Pas();
        private Klant klant = new Klant();
        private Transactie transactie = new Transactie();

        public MainMenu()
        {
            InitializeComponent();
            label7.Text = MainBackend.AantalBiljetten10.ToString();
            label8.Text = MainBackend.AantalBiljetten20.ToString();
            label9.Text = MainBackend.AantalBiljetten50.ToString();
        }

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            Saldo next = new Saldo();
            next.Show();
            next.Focus();
        }

        private void btnStoppen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnGeldOpnemen_Click(object sender, EventArgs e)
        {
            GeldOpnemen next = new GeldOpnemen();
            next.Show();
            next.Focus();
        }

        private void checkButtonPushed()
        {
            ArduinoInput.checkKeypad();
        }

        private void bedankt(bool bon, int saldo, int bedrag)
        {
            Bedankt next = new Bedankt(bon, saldo, bedrag);
            next.Show();
            next.Focus();
        }

        private void btnSnelpinnen10_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 10;
            int huidigSaldo = rekening.Balans;
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(dispenser.isGeldBeschikbaar(bedrag))
                {
                    mainBackend.doTransactie(nieuwSaldo, pas.PasID);
                    bedankt(false, nieuwSaldo, bedrag);
                }
                else
                {
                    label1.Text = "Er is niet genoeg geld beschikbaar voor deze transactie.";
                    checkButtonPushed();
                }
            }
        }

        private void btnSnelpinnen20_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 20;
            int huidigSaldo = rekening.Balans;
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(dispenser.isGeldBeschikbaar(bedrag))
                {
                    mainBackend.doTransactie(nieuwSaldo, pas.PasID);
                    bedankt(false, nieuwSaldo, bedrag);
                }
                else
                {
                    label1.Text = "Er is niet genoeg geld beschikbaar voor deze transactie.";
                    checkButtonPushed();
                }
            }
        }

        private void btnSnelpinnen50_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 50;
            int huidigSaldo = rekening.Balans;
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(dispenser.isGeldBeschikbaar(bedrag))
                {
                    mainBackend.doTransactie(nieuwSaldo, pas.PasID);
                    bedankt(false, nieuwSaldo, bedrag);
                }
                else
                {
                    label1.Text = "Er is niet genoeg geld beschikbaar voor deze transactie.";
                    checkButtonPushed();
                }
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
        }

        private void MainMenu_Shown(object sender, EventArgs e)
        {
            checkButtonPushed();
        }

        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            string strKey = e.KeyValue.ToString().Trim();
            string key1 = "49";
            string key2 = "50";
            string key3 = "51";
            string keyA = "65";
            string keyB = "66";
            string keyC = "67";
            string keyD = "68";

            if(strKey.Equals(key1))
            {
                btnSnelpinnen10.PerformClick();
            }
            else if(strKey.Equals(key2))
            {
                btnSnelpinnen20.PerformClick();
            }
            else if(strKey.Equals(key3))
            {
                btnSnelpinnen50.PerformClick();
            }
            else if(strKey.Equals(keyA))
            {
                checkButtonPushed();
            }
            else if(strKey.Equals(keyB))
            {
                btnSaldo.PerformClick();
            }
            else if(strKey.Equals(keyC))
            {
                btnStoppen.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnGeldOpnemen.PerformClick();
            }
            else
            {
                checkButtonPushed();
            }
        }
    }
}