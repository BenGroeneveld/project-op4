using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Admin10 : Helper
    {
        private bool leaveThisPage = false;
        private string geldToevoegenAantal = "";

        public Admin10()
        {
            InitializeComponent();
        }

        private void btnVolgende_Click(object sender, EventArgs e)
        {
            if(isCorrectBedrag(geldToevoegenAantal))
            {
                Admin20 next = new Admin20();
                next.Show();
                leaveThisPage = true;
            }
            else
            {
                leaveThisPage = false;
                checkInput();
            }
        }

        private void Admin10_KeyDown(object sender, KeyEventArgs key)
        {
            leaveThisPage = false;
            string strKey = key.KeyValue.ToString().Trim();
            string str = key.KeyCode.ToString().Remove(0, 1);
            string keyA = "65";
            string keyC = "67";
            string keyD = "68";

            if(strKey.Equals(keyA))
            {
                button1.PerformClick();
            }
            else if(strKey.Equals(keyC))
            {
                btnVorige.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnVolgende.PerformClick();
            }
            else if(str.Equals("1") || str.Equals("2") || str.Equals("3") || str.Equals("4") || str.Equals("5") || str.Equals("6") || str.Equals("7") || str.Equals("8") || str.Equals("9") || str.Equals("0"))
            {
                geldToevoegenAantal += str;
                aantal.Text = geldToevoegenAantal;
            }
        }

        private void checkInput()
        {
            while(!leaveThisPage)
            {
                ArduinoInput.checkKeypad();
            }
        }

        private bool isCorrectBedrag(string str)
        {
            if(!str.Equals(""))
            {
                int aantalBiljetten = Convert.ToInt32(str);
                if(aantalBiljetten < 0)
                {
                    label1.Text = "Incorrect aantal.\nTyp een positief getal in.";
                    resetBedrag();
                    return false;
                }
                else
                {
                    MainBackend.AantalBiljetten10 = aantalBiljetten;
                    return true;
                }
            }
            else
            {
                label1.Text = "Incorrect aantal.\nTyp een positief getal in.";
                resetBedrag();
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetBedrag();
        }

        private void resetBedrag()
        {
            aantal.ResetText();
            geldToevoegenAantal = "";
        }

        private void Admin10_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            checkInput();
        }

        private void btnVorige_Click(object sender, EventArgs e)
        {
            AdminHome next = new AdminHome();
            next.Show();
            leaveThisPage = true;
        }
    }
}
