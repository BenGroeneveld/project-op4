using System;
using System.Windows.Forms;
using System.Threading;

namespace Pinautomaat
{
    public partial class MainMenu  : Background
    {
        public MainMenu()
        {
            InitializeComponent();

            string attribute = "Balans";
            Program.StrBedrag = MainBackend.strDbQuery(attribute, Program.Rfid);
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

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            Saldo next = new Saldo();
            next.Show();
        }

        private void btnStoppen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnGeldOpnemen_Click(object sender, EventArgs e)
        {
            GeldOpnemen next = new GeldOpnemen();
            next.Show();
        }

        private void checkButtonPushed()
        {
            ArduinoInput.checkKeypad();
        }

        private void bedanktSnelpinnen()
        {
            Bedankt next = new Bedankt(false);
            next.Show();
        }

        private void btnSnelpinnen10_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 10;
            int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;

            if(nieuwSaldo >= 0)
            {
                MainBackend.doTransactie(nieuwSaldo, Program.Rfid);
                bedanktSnelpinnen();
            }
        }

        private void btnSnelpinnen20_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 20;
            int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;

            if(nieuwSaldo >= 0)
            {
                MainBackend.doTransactie(nieuwSaldo, Program.Rfid);
                bedanktSnelpinnen();
            }
        }

        private void btnSnelpinnen50_Click(object sender, EventArgs e)
        {
            int opnemenBedrag = 100 * 50;
            int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;

            if(nieuwSaldo >= 0)
            {
                MainBackend.doTransactie(nieuwSaldo, Program.Rfid);
                bedanktSnelpinnen();
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            //Application.DoEvents();
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

            checkButtonPushed();
        }
    }
}