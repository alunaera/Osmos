using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Osmos
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            gameField.Paint += Draw;
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }
}
