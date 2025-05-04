using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.View
{
    public interface IPingPong
    {
        void UpdateScoreDisplay(int playerScore, int enemyScore);
        void UpdateBallPosition(Point position);
        void UpdatePlayer1Position(int top);
        void UpdatePlayer2Position(int top);
        void ShowGameOverMessage(string message);
        void SetGameAreaSize(Size size);
    }
}
