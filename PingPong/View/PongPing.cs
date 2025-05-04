using PingPong.Model;
using PingPong.Presenter;
using PingPong.View;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace PingPong
{
    public partial class PongPing : Form, IPingPong
    {
        private GamePresenter presenter;

        public PongPing()
        {
            InitializeComponent();
            presenter = new GamePresenter(this);

            // Initialize UI elements
            Player1.Width = 20;
            Player1.Height = 100;
            Player2.Width = 20;
            Player2.Height = 100;

            GameModel.Instance.GameAreaSize = this.ClientSize;
        }

        // IGameView implementation
        public void UpdateScoreDisplay(int playerScore, int enemyScore)
        {
            label1.Text = $" {playerScore} — {enemyScore}";
        }

        public void UpdateBallPosition(Point position)
        {
            ball.Location = position;
        }

        public void UpdatePlayer1Position(int top)
        {
            Player1.Top = top;
        }

        public void UpdatePlayer2Position(int top)
        {
            Player2.Top = top;
        }

        public void ShowGameOverMessage(string message)
        {
            MessageBox.Show(message, "Game Over");
        }

        public void SetGameAreaSize(Size size)
        {
            this.ClientSize = size;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            presenter.GameLoop();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            presenter.HandleKeyDown(e.KeyCode);
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            presenter.HandleKeyUp(e.KeyCode);
        }
    }
}