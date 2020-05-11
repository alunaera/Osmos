using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Osmos
{
    internal class Game
    {
        public static readonly Random Random = new Random();
        private readonly Font font = new Font("Arial", 15);

        private int gameFieldWidth;
        private int gameFieldHeight;
        private double fps;
        private int delayOfShot;
        private GameMode gameMode;
        private List<Circle> circles;
        private double SummaryArea => circles.Sum(circle => circle.Area);
        private Circle PlayerCircle => circles[0];

        public event Action Defeat = delegate { };
        public event Action Victory = delegate { };

        public void StartGame(int gameFieldWidth, int gameFieldHeight)
        {
            this.gameFieldWidth = gameFieldWidth;
            this.gameFieldHeight = gameFieldHeight;
            delayOfShot = 0;

            circles = new List<Circle> ();
            GenerateCircles();
        }

        private void GenerateCircles()
        {
            int circlesCount;

            switch (gameMode)
            {
                case GameMode.Cycle:
                case GameMode.Repulsion:
                    circles.Add(new Circle(gameFieldWidth, gameFieldHeight, CircleType.PlayerCircle));

                    circlesCount = Random.Next(300, 400);
                    for (int i = 0; i < circlesCount; i++)
                        circles.Add(new Circle(gameFieldWidth, gameFieldHeight, CircleType.EnemyCircle));
                    break;

                case GameMode.ManyCircles:
                    circles.Add(new Circle(gameFieldWidth, gameFieldHeight, CircleType.PlayerCircle));
                    PlayerCircle.Radius = 6;

                    circlesCount = 30000;
                    for (int i = 0; i < circlesCount; i++)
                    {
                        circles.Add(new Circle(gameFieldWidth, gameFieldHeight, CircleType.EnemyCircle));
                        circles[i + 1].Radius = 2;
                    }
                    break;
            }
        }

        public void Update(double fps)
        {
            this.fps = fps;

            foreach (Circle circle in circles)
                circle.Update(gameMode);

            ProcessAbsorb();

            if (delayOfShot > 0)
                delayOfShot--;

            if (PlayerCircle.Area <= 0)
                Defeat();

            if (PlayerCircle.Area > SummaryArea / 2)
                Victory();
        }

        private void ProcessAbsorb()
        {
            List<Circle>[] leftEdges = new List<Circle>[gameFieldWidth + 5];
            List<Circle>[] rightEdges = new List<Circle>[gameFieldWidth + 5];

            for (int i = 0; i < gameFieldWidth + 5; ++i)
            {
                leftEdges[i] = new List<Circle>();
                rightEdges[i] = new List<Circle>();
            }

            foreach (var circle in circles)
            {
                leftEdges[Math.Max(0, (int) (circle.PositionX - Math.Abs(circle.Radius)))].Add(circle);
                rightEdges[Math.Min((int) (circle.PositionX + Math.Abs(circle.Radius)), gameFieldWidth)].Add(circle);
            }

            List<Circle> workList = new List<Circle>();

            for (int i = 0; i < gameFieldWidth; i++)
            {
                foreach (Circle circle in workList)
                {
                    foreach (Circle nextCircle in leftEdges[i])
                    {
                        if ((circle.Radius + nextCircle.Radius) * (circle.Radius + nextCircle.Radius)
                              > circle.GetSqrDistanceToObject(nextCircle))
                        {
                            if (circle.Radius >= nextCircle.Radius)
                                Absorb(circle, nextCircle);
                            else
                                Absorb(nextCircle, circle);
                        }
                    }
                }

                workList.AddRange(leftEdges[i]);
                workList.RemoveAll(circle => rightEdges[i].Contains(circle));
            }

            circles.RemoveAll(circle => circle.CircleType != CircleType.PlayerCircle && circle.Area <= 0);
        }

        private static void Absorb(Circle largerCircle, Circle smallerCircle)
        {
            double sqrDistance = largerCircle.GetSqrDistanceToObject(smallerCircle);
            double previousLargerCircleArea = largerCircle.Area;
            double previousSmallerCircleArea = smallerCircle.Area;

            if ((largerCircle.Radius - smallerCircle.Radius) * (largerCircle.Radius - smallerCircle.Radius) > sqrDistance)
            {
                largerCircle.SetRadiusByArea(largerCircle.Area + smallerCircle.Area);
                smallerCircle.Radius = 0;
            }
            else
            {
                smallerCircle.Radius = GetNewRadiusSmallerCircle(largerCircle.Radius,
                    smallerCircle.Radius, largerCircle.Radius + smallerCircle.Radius - Math.Sqrt(sqrDistance));
                largerCircle.SetRadiusByArea(largerCircle.Area + previousSmallerCircleArea - smallerCircle.Area);
            }

            // Momentum Conservation Principle
            double largerCircleVectorX =
                (smallerCircle.VectorX * (previousSmallerCircleArea - smallerCircle.Area) +
                 largerCircle.VectorX * previousLargerCircleArea) / largerCircle.Area;

            double largerCircleVectorY =
                (smallerCircle.VectorY * (previousSmallerCircleArea - smallerCircle.Area) +
                 largerCircle.VectorY * previousLargerCircleArea) / largerCircle.Area;

            largerCircle.SetNewVector(largerCircleVectorX, largerCircleVectorY);
        }

        // This is my analytical solution for correct circle's absorbing
        private static double GetNewRadiusSmallerCircle(double largerRadius, double smallerRadius, double valueOfIntersection)
        {
            double b = largerRadius - smallerRadius + valueOfIntersection;
            double c = valueOfIntersection * (valueOfIntersection - 2 * smallerRadius) / 2;
            double sqrtOfDiscriminant = Math.Sqrt(b * b - 4 * c);

            return smallerRadius - valueOfIntersection - (sqrtOfDiscriminant - b) / 2;
        }

        public void MakeShot(int cursorPositionX, int cursorPositionY)
        {
            if (delayOfShot <= 0)
            {
                circles.Add(PlayerCircle.GetNewEnemyCircle(cursorPositionX, cursorPositionY));
                delayOfShot = 20;
            }
        }

        public void ChangeGameMode(GameMode gameMode)
        {
            this.gameMode = gameMode;
            StartGame(gameFieldWidth, gameFieldHeight);
        }

        public void Draw(Graphics graphics)
        {
            DrawCircles(graphics);
            DrawInterface(graphics);
        }

        private void DrawCircles(Graphics graphics)
        {
            foreach (Circle circle in circles)
                switch (circle.CircleType)
                {
                    case CircleType.PlayerCircle:
                        circle.Draw(graphics, Brushes.Green);
                        break;
                    case CircleType.EnemyCircle:
                        circle.Draw(graphics, circle.Area <= PlayerCircle.Area ? Brushes.Blue : Brushes.Red);
                        break;
                }
        }

        private void DrawInterface(Graphics graphics)
        {
            graphics.DrawString("Summary area: " + (int)SummaryArea, font, Brushes.Black, gameFieldWidth - 250, 10);
            graphics.DrawString("Summary impX: " + (int)circles.Sum(circle => circle.ImpulseX), font, Brushes.Black, gameFieldWidth - 250, 30);
            graphics.DrawString("Summary impY: " + (int)circles.Sum(circle => circle.ImpulseY), font, Brushes.Black, gameFieldWidth - 250, 50);
            graphics.DrawString("FPS: " + (int)fps, font, Brushes.Black, gameFieldWidth - 250, 70);
        }
    }
}
