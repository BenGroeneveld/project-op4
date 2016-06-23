using System;
using System.Windows.Forms;
using System.Threading;

namespace Pinautomaat
{
    public partial class MainMenu  : Helper
    {
        public MainMenu()
        {
            InitializeComponent();
            setWeergaveAantalBiljetten();
        }

        private void setWeergaveAantalBiljetten()
        {
            if(MainBackend.AantalBiljetten10 < 5 && MainBackend.AantalBiljetten10 > 0)
            {
                label7.Text = "Laag";
            }
            else if(MainBackend.AantalBiljetten10 == 0)
            {
                label7.Text = "Geen";
            }
            else
            {
                label7.Text = "OK";
            }
            if(MainBackend.AantalBiljetten20 < 5 && MainBackend.AantalBiljetten20 > 0)
            {
                label8.Text = "Laag";
            }
            else if(MainBackend.AantalBiljetten20 == 0)
            {
                label8.Text = "Geen";
            }
            else
            {
                label8.Text = "OK";
            }
            if(MainBackend.AantalBiljetten50 < 5 && MainBackend.AantalBiljetten50 > 0)
            {
                label9.Text = "Laag";
            }
            else if(MainBackend.AantalBiljetten50 == 0)
            {
                label9.Text = "Geen";
            }
            else
            {
                label9.Text = "OK";
            }
        }

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                Saldo next = new Saldo();
                next.Show();
            }
            else
            {
                MainBackend.restart();
            }
        }

        private void btnStoppen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnGeldOpnemen_Click(object sender, EventArgs e)
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                GeldOpnemen next = new GeldOpnemen();
                next.Show();
            }
            else
            {
                MainBackend.restart();
            }
        }

        private void checkButtonPushed()
        {
            ArduinoInput.checkKeypad(this);
        }
        
        private void bedankt(bool bon, int saldo, int bedrag)
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                Bedankt next = new Bedankt(bon, saldo, bedrag);
                next.Show();
            }
            else
            {
                MainBackend.restart();
            }
        }

        private void btnSnelpinnen10_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 10;
            int huidigSaldo = Convert.ToInt32(Program.Balans);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(Dispenser.isGeldBeschikbaar(bedrag))
                {
                    MainBackend.doTransactie(nieuwSaldo);
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
            int huidigSaldo = Convert.ToInt32(Program.Balans);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(Dispenser.isGeldBeschikbaar(bedrag))
                {
                    MainBackend.doTransactie(nieuwSaldo);
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
            int huidigSaldo = Convert.ToInt32(Program.Balans);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;
            int bedrag = 0;

            if(nieuwSaldo >= 0)
            {
                bedrag = opnemenBedrag;
                if(Dispenser.isGeldBeschikbaar(bedrag))
                {
                    MainBackend.doTransactie(nieuwSaldo);
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
            Activate();
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