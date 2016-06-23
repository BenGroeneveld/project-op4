using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Saldo : Helper
    {
        private static string doubleBedrag = "";

        public Saldo()
        {
            InitializeComponent();
            
            double i = Convert.ToDouble(Program.Balans);
            double j = i / 100;
            int saldo = 0;
            doubleBedrag = Convert.ToString(j);

            if(i % 100 == 0)
            {
                bedrag.Text = doubleBedrag;
                bedrag.Text += ",00";
            }
            else if(i % 10 == 0)
            {
                bedrag.Text = doubleBedrag;
                bedrag.Text += "0";
            }
            else if(i % 1 == 0)
            {
                bedrag.Text = doubleBedrag;
            }
            saldo = Convert.ToInt32(100 * j);
        }

        private void nextPage()
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

        private void btnUitloggen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnGeldOpnemen_Click(object sender, EventArgs e)
        {
            nextPage();
        }

        private void checkButtonPushed()
        {
            ArduinoInput.checkKeypad(this);
        }

        private void Saldo_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            Activate();
            checkButtonPushed();
        }

        private void Saldo_KeyDown(object sender, KeyEventArgs e)
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
                checkButtonPushed();
            }
            else if(strKey.Equals(key2))
            {
                checkButtonPushed();
            }
            else if(strKey.Equals(key3))
            {
                checkButtonPushed();
            }
            else if(strKey.Equals(keyA))
            {
                checkButtonPushed();
            }
            else if(strKey.Equals(keyB))
            {
                checkButtonPushed();
            }
            else if(strKey.Equals(keyC))
            {
                btnUitloggen.PerformClick();
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
