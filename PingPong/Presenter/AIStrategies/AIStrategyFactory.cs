using PingPong.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.Presenter.AIStrategies
{
    public class AIStrategyFactory
    {
        void Move(Rectangle ballBounds, Rectangle paddleBounds, Size gameArea);
    }

    public class AIStrategyFactory
    {
        public IAIStrategy CreateStrategy(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => new EasyAIStrategy(),
                Difficulty.Medium => new MediumAIStrategy(),
                Difficulty.Hard => new HardAIStrategy(), 
                _ => new MediumAIStrategy(),
            };
        }
    }

    // Concrete strategies
    public class EasyAIStrategy : IAIStrategy
    {
        private readonly Random rand = new Random();

        public void Move(Rectangle ballBounds, Rectangle paddleBounds, Size gameArea)
        {
            if (rand.Next(0, 100) > 50) // 50% chance to react
            {
                if (ballBounds.Top < paddleBounds.Top + (paddleBounds.Height / 2) && ballBounds.Left >= 300)
                {
                    paddleBounds.Y -= 10;
                }
                if (ballBounds.Top > paddleBounds.Top + (paddleBounds.Height / 2) && ballBounds.Left >= 300)
                {
                    paddleBounds.Y += 10;
                }
            }
        }
    }

    public class MediumAIStrategy : IAIStrategy
    {
        private readonly Random rand = new Random();

        public void Move(Rectangle ballBounds, Rectangle paddleBounds, Size gameArea)
        {
            if (rand.Next(0, 100) > 30) // 70% chance to react
            {
                if (ballBounds.Top < paddleBounds.Top + (paddleBounds.Height / 2) && ballBounds.Left >= 300)
                {
                    paddleBounds.Y -= 15;
                }
                if (ballBounds.Top > paddleBounds.Top + (paddleBounds.Height / 2) && ballBounds.Left >= 300)
                {
                    paddleBounds.Y += 15;
                }
            }
        }
    }

    public class HardAIStrategy : IAIStrategy
    {
        public void Move(Rectangle ballBounds, Rectangle paddleBounds, Size gameArea)
        {
            // Always reacts and predicts ball movement
            int targetY = ballBounds.Top + (ballBounds.Height / 2);
            int paddleCenter = paddleBounds.Top + (paddleBounds.Height / 2);

            if (targetY < paddleCenter)
            {
                paddleBounds.Y -= 20;
            }
            else if (targetY > paddleCenter)
            {
                paddleBounds.Y += 20;
            }

            // Keep paddle within bounds
            paddleBounds.Y = Math.Max(0, Math.Min(gameArea.Height - paddleBounds.Height, paddleBounds.Y));
        }
    }
}