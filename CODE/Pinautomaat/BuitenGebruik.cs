using System.Media;
using System.Windows.Forms;
using System.Threading;

namespace Pinautomaat
{
    public partial class BuitenGebruik : Form
    {
        private DatabaseConnection dbConnect = new DatabaseConnection();
        private MainBackend mainBackend = new MainBackend();
        private Dispenser dispenser = new Dispenser();
        private Rekening rekening = new Rekening();
        private Pas pas = new Pas();
        private Klant klant = new Klant();
        private Transactie transactie = new Transactie();

        public BuitenGebruik()
        {
            InitializeComponent();
        }

        private void playAudio()
        {
            string file = @"EasterEgg.wav";
            SoundPlayer easterEgg = new SoundPlayer(file);
            
            easterEgg.PlayLooping();
        }

        private void BuitenGebruik_Shown(object sender, System.EventArgs e)
        {
            playAudio();
        }
    }
}
