using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            var saldoForm = new Saldo();
            saldoForm.Show();
            this.Hide();
            saldoForm.Closed += (s, args) => this.Close();
        }

        private void btnStoppen_Click(object sender, EventArgs e)
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
            string str = "";
            while(str.Equals("") || !(str.Equals("B") || str.Equals("C") || str.Equals("D")))
            {
                str = ArduinoInput.strInputText();
                if(str.Equals("B"))
                {
                    btnSaldo.PerformClick();
                }
                else if(str.Equals("C"))
                {
                    btnStoppen.PerformClick();
                }
                else if(str.Equals("D"))
                {
                    btnGeldOpnemen.PerformClick();
                }
            }
        }

        private void MainMenu_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            checkButtonPushed();
        }
    }
}