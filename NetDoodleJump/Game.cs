﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetDoodleJump
{
    public class Game
    {
        protected Bitmap texture;
        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }
        public bool StopMove
        {
            get;
            set;
        }
    }
    public class Player:Game
    {
        private static int width, height;
        private int score = 0;

        public static int Width
        {
            get { return width; }
            private set { width = value; }
        }
        public static int Height
        {
            get { return height; }
            private set { height = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public bool IsGameOver
        {
            get; private set;
        }
        public bool isGravityOn = true;
        
        public Player(int x, int y, Bitmap texture)
        {
            IsGameOver = false;
            Score = 0;
            X = x;
            Y = y;
            this.texture = texture;
            Width = texture.Width;
            Height = texture.Height;

        }
        public void Jump(Edge[] edges, int length)
        {
            isGravityOn = false;
            for (int i = 1; i <= 500; i++)
            {
                Y--;
                i++;
                Thread.Sleep(1);
            }
        }
        public void Move(Keys key)
        {
            StopMove = false;
            switch (key)
            {
                case Keys.Right:
                    do
                    {
                        X += 2;
                        Thread.Sleep(3);
                    } while (!StopMove && ((X + Width) < GameWindow.formWidth));
                    StopMove = false;

                    break;
                case Keys.Left:
                    do
                    {
                        X -= 2;
                        Thread.Sleep(3);
                    } while (!StopMove && X > 0);
                    StopMove = false;

                    break;
            }
        }
        public void Gravity(Edge[] edges, int length)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Y + 2 * Height >= GameWindow.formHeight)
                {
                    isGravityOn = true;
                    IsGameOver = true;
                    return;
                }
                if (IsStayOrHitOnEdge(edges, length, false))
                {
                    isGravityOn = true;
                    return;
                }
                Y++;
            }
        }
        private bool IsStayOrHitOnEdge(Edge[] edges, int length, bool isJump)
        {
            // обработка столкновений с гранями , снизу для прыжка или сверху при падении
            foreach (Edge e in edges)
            {
                if (!isJump)
                {
                    if (Y + Height == e.Y && ((Math.Abs(X - e.X) < 40) || (Math.Abs((X + Width) - (e.X + length)) < 40)))
                    {
                        if (!e.Counted)
                        {
                            e.Counted = true;
                            Score++;
                        }
                        return true;
                    }
                }
                else
                {
                    if (Y == e.Y + 1 && ((Math.Abs(X - e.X) < 40) || (Math.Abs((X + Width) - (e.X + length)) < 40)))
                        return true;
                }
            }
            return false;
        }
        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(texture, new Rectangle(X, Y, Width, Height));
        }
    }
    public class Edge:Game
    {
        public bool Counted { get; set; } 
        private static int width, height;

        public static int Width
        {
            get { return width; }
            private set { width = value; }
        }
        public static int Height
        {
            get { return height; }
            private set { height = value; }
        }
        public Edge(int x, int y)
        {
            X = x;
            Y = y;
            texture = Resources.edgeTexture;
            Width = texture.Width;
            Height = texture.Height;
        }
        public void Move()
        { 
            Y++;
        }
        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(texture, new Rectangle(X, Y, Width, Height));
        }
    }
}
