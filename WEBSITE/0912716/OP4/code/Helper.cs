using System.Windows.Forms;

namespace Pinautomaat
{
    public partial class Helper : Form
    {
        public Helper()
        {
            InitializeComponent(); MainBackend.moveCursor();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
