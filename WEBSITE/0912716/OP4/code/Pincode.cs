using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Pincode : Background
    {

        public int password = 0;
        private int correctPassword = 987654321;
        private string cardID = ArduinoInput.strCardID;
        private bool approval = true;
        private int poging = 1;
        private int actief = 0;

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
                        if(poging < 3)
                        {
                            infoText.Text += " (" + poging + " / 3)";
                            poging++;
                            approval = true;
                            startPincode();
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
                            MainBackend.blokkeerPas(Program.Rfid);
                            startPincode();
                        }
                    }
                    else
                    {
                        poging = 0;
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
            string strToInt = MainBackend.strDbQuery("Actief", Program.Rfid);
            actief = Convert.ToInt32(strToInt);
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
            string strPassword = MainBackend.strDbQuery("Pincode", Program.Rfid);
            correctPassword = Convert.ToInt32(strPassword);
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
            ArduinoInput.checkKeypad();
        }

        private void startPincode()
        {
            if(actief == 1)
            {
                checkButtonPushed(); ;
            }
            else if(actief == 0)
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

        private void nextPage()
        {
            MainMenu next = new MainMenu();
            next.Show();
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
            string keyB = "66";
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
                        password = Convert.ToInt32(passwordStr);
                    }
                    else
                    {
                        MessageBox.Show("Foutje2");
                    }
                }
                startPincode();
            }
            else
            {
                MessageBox.Show("Foutje1");
            }
        }

        private void Pincode_Shown(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            setup();
            startPincode();
        }
    }
}