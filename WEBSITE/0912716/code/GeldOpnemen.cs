using System;
using System.Windows.Forms;

namespace Gui
{
    public partial class GeldOpnemen : Form
    {
        private int veelvoudBedrag = 5;
        private bool uitloggen = false;
        private bool leaveThisPage = false;
        private bool printBon = true;
        private string geldOpnemenBedrag = "";

        public GeldOpnemen()
        {
            InitializeComponent();
        }

        private void nextPage(bool printBon)
        {
            var bedankt = new Bedankt(printBon);
            bedankt.Show();
            this.Hide();
            bedankt.Closed += (s, args) => this.Close();
        }

        private void btnUitloggen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnPrintBonWel_Click(object sender, EventArgs e)
        {
            if(isCorrectBedrag(geldOpnemenBedrag))
            {
                printBon = true;
                nextPage(printBon);
            }
            else
            {
                doGeldOpnemen();
            }
        }

        private void btnPrintBonNiet_Click(object sender, EventArgs e)
        {
            if(isCorrectBedrag(geldOpnemenBedrag))
            {
                printBon = false;
                nextPage(printBon);
            }
            else
            {
                doGeldOpnemen();
            }
        }

        private void checkUitloggen()
        {
            if(uitloggen)
            {
                btnUitloggen.PerformClick();
            }
        }

        private void checkButtonPushed()
        {
            string str = "";

            while(str.Equals(""))
            {
                str = ArduinoInput.strInputText();

                if(str.Equals("A"))
                {
                    button1.PerformClick();
                }
                else if(str.Equals("B"))
                {
                    btnPrintBonWel.PerformClick();
                    leaveThisPage = true;
                }
                else if(str.Equals("C"))
                {
                    btnUitloggen.PerformClick();
                    leaveThisPage = true;
                }
                else if(str.Equals("D"))
                {
                    btnPrintBonNiet.PerformClick();
                    leaveThisPage = true;
                }
                else
                {
                    bedrag.Text += str;
                }
            }
        }

        private bool isCorrectBedrag(string str)
        {
            int opnemenBedrag = 100*(Convert.ToInt32(str));
            int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
            if(opnemenBedrag % (100 * veelvoudBedrag) != 0)
            {
                label1.Text = "Incorrect bedrag.\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
                bedrag.ResetText();
                return false;
            }
            else if(opnemenBedrag > huidigSaldo)
            {
                label1.Text = "Saldo niet toereikend.\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
                bedrag.ResetText();
                return false;
            }
            else
            {
                int nieuwSaldo = huidigSaldo - opnemenBedrag;
                MainBackend.doTransactie(nieuwSaldo, Program.Rfid);
                return true;
            }
        }

        private void GeldOpnemen_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            label1.Text = "Hoeveel geld wilt u opnemen?\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
            doGeldOpnemen();
        }

        private void doGeldOpnemen()
        {
            while(!leaveThisPage)
            {
                geldOpnemenBedrag = bedrag.Text;
                checkButtonPushed();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bedrag.ResetText();
        }
    }
}
