using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class BuitenGebruik : Helper
    {
        public BuitenGebruik()
        {
            InitializeComponent(); MainBackend.moveCursor();
        }

        private void BuitenGebruik_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
