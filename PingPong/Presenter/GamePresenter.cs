using PingPong.Model;
using PingPong.Presenter.AIStrategies;
using PingPong.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPong.Presenter
{
    public class GamePresenter
    {
        private readonly IPingPong view;
        private readonly GameModel model;
        private readonly Random rand = new Random();
        private bool goDown, goUp;
        private readonly int[] speedVariations = { 5, 6, 8, 9, 10, 11, 12 };

        // Factory for AI strategies
        private readonly AIStrategyFactory strategyFactory = new AIStrategyFactory();
        private IAIStrategy currentAIStrategy;

        public GamePresenter(IPingPong view)
        {
            this.view = view;
            model = GameModel.Instance;
            currentAIStrategy = strategyFactory.CreateStrategy(model.CurrentDifficulty);
        }

        public void GameLoop()
        {
            // Update ball position
            model.BallBounds = new Rectangle(
                model.BallBounds.X - model.BallXspeed,
                model.BallBounds.Y - model.BallYspeed,
                model.BallBounds.Width,
                model.BallBounds.Height);

            view.UpdateBallPosition(new Point(model.BallBounds.X, model.BallBounds.Y));

            // Update score display
            view.UpdateScoreDisplay(model.PlayerScore, model.EnemyAIScore);

            // Ball collision with top/bottom
            if (model.BallBounds.Top < 0 || model.BallBounds.Bottom > model.GameAreaSize.Height)
            {
                model.BallYspeed = -model.BallYspeed;
            }

            // Ball out of bounds (left/right)
            if (model.BallBounds.Left < -2)
            {
                ResetGameState(true);
                model.EnemyAIScore++;
            }

            if (model.BallBounds.Right > model.GameAreaSize.Width + 2)
            {
                ResetGameState(false);
                model.PlayerScore++;
            }

            // Move players
            MovePlayer1();
            MoveAI();

            // Check collisions
            CheckCollision(model.Player1Bounds, model.Player1Bounds.Right + 5);
            CheckCollision(model.Player2Bounds, model.Player2Bounds.Left - 35);

            // Check game over
            if (model.EnemyAIScore >= model.WinningScore)
            {
                GameOver("Sorry you lost");
            }
            else if (model.PlayerScore >= model.WinningScore)
            {
                GameOver("You win the game");
            }
        }

        private void ResetGameState(bool isPlayer2Scoring)
        {
            model.BallBounds = new Rectangle(
                model.GameAreaSize.Width / 2 - model.BallBounds.Width / 2,
                model.GameAreaSize.Height / 2 - model.BallBounds.Height / 2,
                model.BallBounds.Width,
                model.BallBounds.Height);

            model.ResetBall(isPlayer2Scoring);
        }

        private void MovePlayer1()
        {
            if (goDown && model.Player1Bounds.Bottom < model.GameAreaSize.Height)
            {
                model.Player1Bounds = new Rectangle(
                    model.Player1Bounds.X,
                    model.Player1Bounds.Y + 15,
                    model.Player1Bounds.Width,
                    model.Player1Bounds.Height);
                view.UpdatePlayer1Position(model.Player1Bounds.Y);
            }
            if (goUp && model.Player1Bounds.Top > 0)
            {
                model.Player1Bounds = new Rectangle(
                    model.Player1Bounds.X,
                    model.Player1Bounds.Y - 15,
                    model.Player1Bounds.Width,
                    model.Player1Bounds.Height);
                view.UpdatePlayer1Position(model.Player1Bounds.Y);
            }
        }

        private void MoveAI()
        {
            currentAIStrategy.Move(model.BallBounds, model.Player2Bounds, model.GameAreaSize);
            view.UpdatePlayer2Position(model.Player2Bounds.Y);
        }

        private void CheckCollision(Rectangle paddle, int offset)
        {
            if (model.BallBounds.IntersectsWith(paddle))
            {
                model.BallBounds = new Rectangle(
                    offset,
                    model.BallBounds.Y,
                    model.BallBounds.Width,
                    model.BallBounds.Height);

                int x = speedVariations[rand.Next(speedVariations.Length)];
                int y = speedVariations[rand.Next(speedVariations.Length)];

                model.BallXspeed = (model.BallXspeed < 0) ? x : -x;
                model.BallYspeed = (model.BallYspeed < 0) ? -y : y;

                model.BallXspeed += (model.BallXspeed < 0) ? -1 : 1;
                model.BallYspeed += (model.BallYspeed < 0) ? -1 : 1;
            }
        }

        public void HandleKeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Down:
                    goDown = true;
                    break;
                case Keys.Up:
                    goUp = true;
                    break;
                case Keys.E:
                    model.CurrentDifficulty = Difficulty.Easy;
                    currentAIStrategy = strategyFactory.CreateStrategy(Difficulty.Easy);
                    break;
                case Keys.M:
                    model.CurrentDifficulty = Difficulty.Medium;
                    currentAIStrategy = strategyFactory.CreateStrategy(Difficulty.Medium);
                    break;
                case Keys.H:
                    model.CurrentDifficulty = Difficulty.Hard;
                    currentAIStrategy = strategyFactory.CreateStrategy(Difficulty.Hard);
                    break;
                case Keys.D1:
                    model.WinningScore = 1;
                    break;
                case Keys.D5:
                    model.WinningScore = 5;
                    break;
                case Keys.D0:
                    model.WinningScore = 10;
                    break;
            }
        }

        public void HandleKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Down:
                    goDown = false;
                    break;
                case Keys.Up:
                    goUp = false;
                    break;
            }
        }

        private void GameOver(string message)
        {
            view.ShowGameOverMessage(message);
            model.EnemyAIScore = 0;
            model.PlayerScore = 0;
            model.BallXspeed = model.BallYspeed = 4;
        }
    }
}
