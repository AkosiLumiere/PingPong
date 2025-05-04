using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.Model
{
    public sealed class GameModel
    {
        private static readonly Lazy<GameModel> instance =
            new Lazy<GameModel>(() => new GameModel());

        public static GameModel Instance => instance.Value;

        // Game properties
        public int BallXspeed { get; set; } = 4;
        public int BallYspeed { get; set; } = 4;
        public int PlayerScore { get; set; } = 0;
        public int EnemyAIScore { get; set; } = 0;
        public int WinningScore { get; set; } = 5;
        public Difficulty CurrentDifficulty { get; set; } = Difficulty.Easy;

        public Rectangle BallBounds { get; set; }
        public Rectangle Player1Bounds { get; set; }
        public Rectangle Player2Bounds { get; set; }
        public Size GameAreaSize { get; set; }

        private GameModel() { }

        public void ResetBall(bool isPlayer2Scoring)
        {
            var rand = new Random();
            BallXspeed = isPlayer2Scoring ? 4 : -4;
            BallYspeed = rand.Next(-4, 5);
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}