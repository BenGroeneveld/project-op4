using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class AdminHome : Form
    {
        private DatabaseConnection dbConnect = new DatabaseConnection();
        private MainBackend mainBackend = new MainBackend();
        private Dispenser dispenser = new Dispenser();
        private Rekening rekening = new Rekening();
        private Pas pas = new Pas();
        private Klant klant = new Klant();
        private Transactie transactie = new Transactie();

        private bool leaveThisPage = false;

        public AdminHome()
        {
            InitializeComponent();
        }

        private void btnStoppen_Click(object sender, EventArgs e)
        {
            MainBackend.restart();
        }

        private void btnVolgende_Click(object sender, EventArgs e)
        {
            Admin10 next = new Admin10();
            next.Show();
            next.Focus();
        }

        private void AdminHome_Load(object sender, EventArgs e)
        {
            MainBackend.closePrevForms();
            checkInput();
        }

        private void checkInput()
        {
            while(!leaveThisPage)
            {
                ArduinoInput.checkKeypad();
            }
        }

        private void AdminHome_KeyDown(object sender, KeyEventArgs key)
        {
            leaveThisPage = false;
            string strKey = key.KeyValue.ToString().Trim();
            string str = key.KeyCode.ToString().Remove(0, 1);
            string keyC = "67";
            string keyD = "68";
            
            if(strKey.Equals(keyC))
            {
                btnStoppen.PerformClick();
            }
            else if(strKey.Equals(keyD))
            {
                btnVolgende.PerformClick();
                leaveThisPage = true;
            }
        }
    }
}
