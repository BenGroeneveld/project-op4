using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class AdminCheck : Helper
    {
        private bool leaveThisPage = false;

        public AdminCheck()
        {
            InitializeComponent();
        }

        private void AdminCheck_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            aantal10.Text = MainBackend.AantalBiljetten10.ToString();
            aantal20.Text = MainBackend.AantalBiljetten20.ToString();
            aantal50.Text = MainBackend.AantalBiljetten50.ToString();
            checkInput();
        }

        private void AdminCheck_KeyDown(object sender, KeyEventArgs key)
        {
            leaveThisPage = false;
            string strKey = key.KeyValue.ToString().Trim();
            string str = key.KeyCode.ToString().Remove(0, 1);
            string keyC = "67";
            string keyD = "68";

            if(strKey.Equals(keyC))
            {
                btnVorige.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnVolgende.PerformClick();
            }
        }

        private void btnVorige_Click(object sender, EventArgs e)
        {
            Admin50 next = new Admin50();
            next.Show();
        }

        private void btnVolgende_Click(object sender, EventArgs e)
        {
            leaveThisPage = true;
            MainBackend.restart();
        }

        private void checkInput()
        {
            while(!leaveThisPage)
            {
                ArduinoInput.checkKeypad();
            }
        }
    }
}
