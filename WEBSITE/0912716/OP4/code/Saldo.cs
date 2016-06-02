using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Saldo : Background
    {
        private static string attribute = "";
        private static string doubleBedrag = "";

        public Saldo()
        {
            InitializeComponent();

            attribute = "Balans";
            string str = MainBackend.strDbQuery(attribute, Program.Rfid);
            double i = Convert.ToDouble(str);
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
            Program.StrBedrag = Convert.ToString(saldo);
        }

        private void btnUitloggen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnGeldOpnemen_Click(object sender, EventArgs e)
        {
            var geldOpnemenForm = new GeldOpnemen();
            geldOpnemenForm.Show();
            this.Hide();
            geldOpnemenForm.Closed += (s, args) => this.Close();
        }

        private void checkButtonPushed()
        {
            ArduinoInput.checkKeypad();
        }

        private void Saldo_Load(object sender, EventArgs e)
        {
            //Application.DoEvents();
            MainBackend.closePrevForms();
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

            }
            else if(strKey.Equals(key2))
            {

            }
            else if(strKey.Equals(key3))
            {

            }
            else if(strKey.Equals(keyA))
            {

            }
            else if(strKey.Equals(keyB))
            {

            }
            else if(strKey.Equals(keyC))
            {
                btnUitloggen.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnGeldOpnemen.PerformClick();
            }
            checkButtonPushed();
        }
    }
}
