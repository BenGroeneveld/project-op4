﻿using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class GeldOpnemen : Helper
    {
        private int veelvoudBedrag = 10;
        private bool uitloggen = false;
        private bool leaveThisPage = false;
        private bool printBon = true;
        private string geldOpnemenBedrag = "";
        private string strGeldOpnemen = "";

        public GeldOpnemen()
        {
            InitializeComponent();
        }

        private void bedankt(bool bon, int saldo, int bedrag)
        {
            Bedankt next = new Bedankt(bon, saldo, bedrag);
            next.Show();
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
                int bedrag = 100 * (Convert.ToInt32(geldOpnemenBedrag));
                int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
                int nieuwSaldo = huidigSaldo - bedrag;

                bedankt(printBon, nieuwSaldo, bedrag);
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
                int bedrag = 100 * (Convert.ToInt32(geldOpnemenBedrag));
                int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
                int nieuwSaldo = huidigSaldo - bedrag;

                bedankt(printBon, nieuwSaldo, bedrag);
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

            while(!(str.Equals("A") || str.Equals("B") || str.Equals("C") || str.Equals("D")))
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
                    geldOpnemenBedrag += str;
                    strGeldOpnemen = geldOpnemenBedrag + ",00";
                    bedrag.Text = strGeldOpnemen;
                }
            }
        }

        private bool isCorrectBedrag(string str)
        {
            if(str.Equals(""))
            {
                label1.Text = "Incorrect bedrag.\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
                resetBedrag();
                return false;
            }
            int opnemenBedrag = 100*(Convert.ToInt32(str));
            int huidigSaldo = Convert.ToInt32(Program.StrBedrag);
            int nieuwSaldo = huidigSaldo - opnemenBedrag;

            if(opnemenBedrag % (100 * veelvoudBedrag) != 0)
            {
                label1.Text = "Incorrect bedrag.\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
                resetBedrag();
                return false;
            }
            else if(opnemenBedrag > huidigSaldo)
            {
                label1.Text = "Saldo niet toereikend.\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
                resetBedrag();
                return false;
            }
            else
            {
                MainBackend.doTransactie(nieuwSaldo, Program.Rfid);
                return true;
            }
        }

        private void doGeldOpnemen()
        {
            while(!leaveThisPage)
            {
                checkButtonPushed();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetBedrag();
        }

        private void resetBedrag()
        {
            bedrag.ResetText();
            geldOpnemenBedrag = "";
            strGeldOpnemen = "";
        }

        private void GeldOpnemen_Load(object sender, EventArgs e)
        {
            label1.Text = "Hoeveel geld wilt u opnemen?\nTyp een veelvoud van €" + veelvoudBedrag + ",00 in.";
            MainBackend.closePrevForms();
            doGeldOpnemen();
        }

        private void GeldOpnemen_KeyDown(object sender, KeyEventArgs e)
        {
            string strKey = e.KeyValue.ToString().Trim();

            string key1 = "49";
            string key2 = "50";
            string key3 = "51";
            string keyA = "65";
            string keyB = "66";
            string keyC = "67";
            string keyD = "68";

            if(strKey.Equals(keyA))
            {
                button1.PerformClick();
            }
            else if(strKey.Equals(keyB))
            {
                btnPrintBonWel.PerformClick();
                leaveThisPage = true;
            }
            else if(strKey.Equals(keyC))
            {
                btnUitloggen.PerformClick();
                leaveThisPage = true;
            }
            else if(strKey.Equals(keyD))
            {
                btnPrintBonNiet.PerformClick();
                leaveThisPage = true;
            }
            else
            {
                string str = e.KeyCode.ToString().Remove(0, 1);
                geldOpnemenBedrag += str;
                strGeldOpnemen = geldOpnemenBedrag + ",00";
                bedrag.Text = strGeldOpnemen;
                checkButtonPushed();
            }
        }
    }
}
