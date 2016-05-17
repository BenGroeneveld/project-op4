using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Pincode : Form
    {
        public int password = 0;
        private int correctPassword = 0;
        private string cardID = ArduinoInput.strCardID;
        private bool uitloggen = false;
        private bool approval = true;
        private bool correctie = false;
        private int poging = 1;
        private int actief = 0;
        private string strVerkeerdWachtwoord = "Verkeerd wachtwoord";

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
            try
            {
                if(password != correctPassword)
                {
                    approval = false;
                    clearPincode();
                    infoText.Text = strVerkeerdWachtwoord;
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
                        checkUitloggen();
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

        private void Pincode_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            setup();
            string strToInt = MainBackend.strDbQuery("Actief", Program.Rfid);
            actief = Convert.ToInt32(strToInt);
            if(actief == 1)
            {
                infoText.Text = "Voer uw pincode in.";
            }
            startPincode();
        }

        private void startPincode()
        {
            if(actief == 1)
            {

                string str = strNumberPushed();
                if(correctie)
                {
                    btnCorrectie.PerformClick();
                }
                else if(uitloggen)
                {
                    btnUitloggen.PerformClick();
                }
                else
                {
                    inputInloggen.Text = str;
                }
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
                checkUitloggen();
            }
        }

        private string strNumberPushed()
        {
            string str = "";
            string strNumber = "";

            if(approval)
            {
                while(str.Equals("") || str.Equals("B") || str.Equals("D"))
                {
                    str = ArduinoInput.strInputText();
                    if(str.Equals("A"))
                    {
                        correctie = true;
                    }
                    else if(str.Equals("C"))
                    {
                        uitloggen = true;
                    }
                    else
                    {
                        strNumber = str;
                    }
                }
            }
            return strNumber;
        }

        private void checkButtonPushed()
        {
            string str = "";
            while(!(str.Equals("A") || str.Equals("C") || str.Equals("D")))
            {
                str = ArduinoInput.strInputText();

                if(str.Equals("A"))
                {
                    btnCorrectie.PerformClick();
                }
                else if(str.Equals("C"))
                {
                    btnUitloggen.PerformClick();
                }
                else if(str.Equals("D"))
                {
                    btnVolgende.PerformClick();
                }
            }
        }

        private void checkUitloggen()
        {
            string str = "";
            while(!str.Equals("C"))
            {
                str = ArduinoInput.strInputText();

                if(str.Equals("C"))
                {
                    btnUitloggen.PerformClick();
                }
            }
        }

        private void inputInloggen_TextChanged(object sender, EventArgs e)
        {
            if(!inputInloggen.Text.Equals(""))
            {
                string str = strNumberPushed();
                if(correctie)
                {
                    btnCorrectie.PerformClick();
                }
                else if(uitloggen)
                {
                    btnUitloggen.PerformClick();
                }
                else
                {
                    textBox1.Text = str;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(!textBox1.Text.Equals(""))
            {
                string str = strNumberPushed();
                if(correctie)
                {
                    btnCorrectie.PerformClick();
                }
                else if(uitloggen)
                {
                    btnUitloggen.PerformClick();
                }
                else
                {
                    textBox2.Text = str;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(!textBox2.Text.Equals(""))
            {
                string str = strNumberPushed();
                if(correctie)
                {
                    btnCorrectie.PerformClick();
                }
                else if(uitloggen)
                {
                    btnUitloggen.PerformClick();
                }
                else
                {
                    textBox3.Text = str;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(!textBox3.Text.Equals(""))
            {
                string passwordStr = inputInloggen.Text + textBox1.Text + textBox2.Text + textBox3.Text;
                password = Convert.ToInt32(passwordStr);
                checkButtonPushed();
            }
        }

        private void nextPage()
        {
            var next = new MainMenu();
            next.Show();
            Hide();
            next.Closed += (s, args) => Close();
        }

        private void btnCorrectie_Click(object sender, EventArgs e)
        {
            approval = false;

            clearPincode();

            correctie = false;
            approval = true;

            string str = strNumberPushed();
            if(correctie)
            {
                btnCorrectie.PerformClick();
            }
            else if(uitloggen)
            {
                btnUitloggen.PerformClick();
            }
            else
            {
                inputInloggen.Text = str;
            }
        }
    }
}
