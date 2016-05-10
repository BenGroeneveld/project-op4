using System;
using System.Windows.Forms;

namespace Gui
{
    public partial class Saldo : Form
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

        private void Saldo_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            checkButtonPushed();
        }

        private void checkButtonPushed()
        {
            string str = "";
            while(!(str.Equals("C") || str.Equals("D")))
            {
                str = ArduinoInput.strInputText();
                if(str.Equals("C"))
                {
                    btnUitloggen.PerformClick();
                }
                else if(str.Equals("D"))
                {
                    btnGeldOpnemen.PerformClick();
                }
            }
        }
    }
}
