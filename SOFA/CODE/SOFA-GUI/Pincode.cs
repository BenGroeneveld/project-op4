using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Pincode : Helper
    {

        private string password = "";
        private string correctPassword = Program.Hash;
        private string cardID = Program.RekeningID + "\n#" + Program.PasID;
        private bool approval = true;

        public Pincode()
        {
            InitializeComponent();
        }

        private void btnUitloggen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnVolgende_Click(object sender, EventArgs e)
        {
            if(!textBox3.Text.Equals(""))
            {
                try
                {
                    if(password != correctPassword)
                    {
                        approval = false;
                        clearPincode();
                        infoText.Text = "Verkeerd wachtwoord";
                        if((Program.Poging + 1) < 3)
                        {
                            infoText.Text += " (" + (Program.Poging + 1) + " / 3)";
                            Program.Poging++;
                            approval = true;
                        }
                        else
                        {
                            infoText.Text += " (3 / 3)";
                            infoText.Text += "\nUw pas is geblokkeerd.";
                            inputInloggen.Hide();
                            textBox1.Hide();
                            textBox2.Hide();
                            textBox3.Hide();
                            btnCorrectie.Hide();
                            btnVolgende.Hide();
                            MainBackend.blokkeerPas(Program.PasID);
                        }
                        startPincode();
                    }
                    else
                    {
                        Program.Poging = 0;
                        nextPage();
                    }
                }
                catch
                {
                    infoText.Text = "Error! [NXTc]";
                }
            }
        }

        public void setup()
        {
            setCardID();
            setCorrectPassword();
        }

        public void setCardID()
        {
            privateSetCardID(cardID);
        }

        private void privateSetCardID(string str)
        {
            txtCardID.Text = str;
        }

        private void setCorrectPassword()
        {
            correctPassword = Program.Hash;
        }

        private void clearPincode()
        {
            inputInloggen.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        public void checkButtonPushed()
        {
            checkInput();
        }

        private void checkInput()
        {
            ArduinoInput.checkKeypad(this);
        }

        private void startPincode()
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                if(Program.Actief == 1)
                {
                    checkButtonPushed();
                }
                else if(Program.Actief == 0)
                {
                    inputInloggen.Hide();
                    textBox1.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    btnCorrectie.Hide();
                    btnVolgende.Hide();
                    infoText.Text = "Uw pas is geblokkeerd.";
                    checkButtonPushed();
                }
            }
            else
            {
                MainBackend.restart();
            }
        }

        private void nextPage()
        {
            Program.SystemGood = MainBackend.checkAllConnections();
            if(Program.SystemGood)
            {
                if(!MainBackend.AdminKaart)
                {
                    MainMenu next = new MainMenu();
                    next.Show();
                }
                else
                {
                    AdminHome next = new AdminHome();
                    next.Show();
                }
            }
            else
            {
                MainBackend.restart();
            }
        }

        private void btnCorrectie_Click(object sender, EventArgs e)
        {
            approval = false;
            clearPincode();
            approval = true;
            startPincode();
        }

        private void Pincode_KeyDown_1(object sender, KeyEventArgs e)
        {
            string strKey = e.KeyValue.ToString().Trim();
            string key0 = "48";
            string key1 = "49";
            string key2 = "50";
            string key3 = "51";
            string key4 = "52";
            string key5 = "53";
            string key6 = "54";
            string key7 = "55";
            string key8 = "56";
            string key9 = "57";
            string keyA = "65";
            string keyC = "67";
            string keyD = "68";

            if(strKey.Equals(keyA))
            {
                btnCorrectie.PerformClick();
            }
            else if(strKey.Equals(keyC))
            {
                btnUitloggen.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnVolgende.PerformClick();
            }
            else if(strKey.Equals(key0) || strKey.Equals(key1) || strKey.Equals(key2) || strKey.Equals(key3) || strKey.Equals(key4) || strKey.Equals(key5) || strKey.Equals(key6) || strKey.Equals(key7) || strKey.Equals(key8) || strKey.Equals(key9))
            {
                if(approval)
                {
                    if(inputInloggen.Text.Equals(""))
                    {
                        string str = e.KeyCode.ToString().Remove(0, 1);
                        inputInloggen.Text = str;
                    }
                    else if(textBox1.Text.Equals(""))
                    {
                        string str = e.KeyCode.ToString().Remove(0, 1);
                        textBox1.Text = str;
                    }
                    else if(textBox2.Text.Equals(""))
                    {
                        string str = e.KeyCode.ToString().Remove(0, 1);
                        textBox2.Text = str;
                    }
                    else if(textBox3.Text.Equals(""))
                    {
                        string str = e.KeyCode.ToString().Remove(0, 1);
                        textBox3.Text = str;
                        string passwordStr = inputInloggen.Text + textBox1.Text + textBox2.Text + textBox3.Text;
                        password = MainBackend.makeHash(Program.RekeningID, passwordStr);
                    }
                }
                startPincode();
            }
            else
            {
                startPincode();
            }
        }

        private void Pincode_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            Activate();
            setup();
            startPincode();
        }
    }
}