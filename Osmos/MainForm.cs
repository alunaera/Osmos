using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Osmos
{
    public partial class MainForm : Form
    {
        private readonly Game game = new Game();

        public MainForm()
        {
            InitializeComponent();

            gameField.Paint += Draw;

            game.Defeat += () =>
            {
                Timer.Enabled = false;
                MessageBox.Show("Game over");
                game.StartGame(gameField.ClientRectangle.Width, gameField.ClientRectangle.Height);
                Timer.Enabled = true;
            };

            game.Victory += () =>
            {
                Timer.Enabled = false;
                MessageBox.Show("You win");
                game.StartGame(gameField.ClientRectangle.Width, gameField.ClientRectangle.Height);
                Timer.Enabled = true;
            };

            game.StartGame(gameField.ClientRectangle.Width, gameField.ClientRectangle.Height);
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(e.Graphics);
        }

        private void TickTimer(object sender, System.EventArgs e)
        {
            game.Update();
            gameField.Refresh();
        }

        private void ClickMouse(object sender, MouseEventArgs e)
        {
            game.MakeShot(e.X, e.Y);
        }
    }
}
