using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class BuitenGebruik : Helper
    {
        public BuitenGebruik()
        {
            InitializeComponent();
        }

        private void BuitenGebruik_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
