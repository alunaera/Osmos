using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Osmos
{
    public partial class MainForm : Form
    {
        private readonly Game game = new Game();
        private bool isMouseDowned;
        private double fps;
        private int cursorPositionX;
        private int cursorPositionY;

        public MainForm()
        {
            InitializeComponent();

            gameField.Paint += Draw;

            game.Defeat += () =>
            {
                gameField.Refresh();
                Timer.Enabled = false;
                MessageBox.Show("Game over");
                game.StartGame(gameField.ClientRectangle.Width, gameField.ClientRectangle.Height);
                Timer.Enabled = true;
            };

            game.Victory += () =>
            {
                gameField.Refresh();
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
            Stopwatch stopwatch = Stopwatch.StartNew();
            game.Update(fps);
            stopwatch.Stop();

            fps = 1000 / (Timer.Interval + stopwatch.ElapsedMilliseconds);

            if(isMouseDowned)
                game.MakeShot(cursorPositionX, cursorPositionY);

            gameField.Refresh();
        }

        private void DownMouse(object sender, MouseEventArgs e)
        {
            isMouseDowned = true;
            cursorPositionX = e.X;
            cursorPositionY = e.Y;
        }

        private void UpMouse(object sender, MouseEventArgs e)
        {
            isMouseDowned = false;
        }

        private void ClickRepulsion(object sender, System.EventArgs e)
        {
            game.ChangeGameMode(GameMode.Repulsion);
        }

        private void ClickCycle(object sender, System.EventArgs e)
        {
            game.ChangeGameMode(GameMode.Cycle);
        }

        private void ClickManyCircles(object sender, System.EventArgs e)
        {
            game.ChangeGameMode(GameMode.ManyCircles);
        }
    }
}
