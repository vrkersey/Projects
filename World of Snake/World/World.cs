using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;
using WebServer;

namespace SnakeGame
{

    /// <summary>
    /// This represents a simple demo of a world that just contains one "dot"
    /// A more interesting SnakeWorld would contain multiple snakes and food
    /// </summary>
    public class World
    {
        // Determines the size in pixels of each grid cell in the world
        public const int pixelsPerCell = 5;

        private Dictionary<int, Food> foods = new Dictionary<int, Food>();
        private Dictionary<int, Snake> snakes = new Dictionary<int, Snake>();

        // Lock objects 
        private object foodLock = new object();
        private object snakeLock = new object();

        // object used for generating a random number
        private Random r = new Random();

        // Server side variables
        object[,] world;
        private int foodDensity;
        private float snakeRecycleRate;
        private int foodID = 0;
        private bool shrinkMode;
        List<Food> garbageFood = new List<Food>();
        List<Snake> garbageSnake = new List<Snake>();

        /// <summary>
        /// way of returning the entire 2d array as a string.
        /// </summary>
        public string getWorld
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for(int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        object o = world[i, j];
                        if (o == null)
                        {
                            sb.Append("_");
                        }
                        else if (object.ReferenceEquals(o.GetType(), typeof(Food)))
                        {
                            sb.Append("f");
                        }
                        else if (object.ReferenceEquals(o.GetType(), typeof(Snake)))
                        {
                            sb.Append("s");
                        }
                    }
                    sb.Append("\n");
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// add a food to the world.
        /// if the food is already in the world use the one from the dictionary
        /// </summary>
        /// <param name="f"></param>
        public void addFood(Food f)
        {
            lock (foodLock)
            {
                if (f.Location.X == -1) //food has been eaten
                {
                    foods.Remove(f.Id);
                }
                else 
                {
                    if (!foods.ContainsKey(f.Id))
                    {
                        f.setColor(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
                        foods.Add(f.Id, f);
                    }
                }

            }
        }

        /// <summary>
        /// add a snake to the world.
        /// if the snake is already in the world use the one from the dictionary
        /// </summary>
        /// <param name="s"></param>
        /// <param name="player">is this snake the player?</param>
        /// <param name="playerColor">player's color</param>
        public void addSnake(Snake s, bool player, Color playerColor)
        {
            lock (snakeLock)
            {
                if (s.Verts.Count()!=0 && s.Verts[0].X == -1) //snake died
                    snakes.Remove(s.Id);
                else
                {
                    if (snakes.ContainsKey(s.Id))
                    {
                        snakes[s.Id].Verts = s.Verts;
                    }
                    else
                    {
                        if (player && playerColor != null) // if the snake is the player use the player color
                        {
                            s.setColor(playerColor.R, playerColor.G, playerColor.B);
                        }
                        else
                        {
                            s.setColor(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
                        }

                        snakes.Add(s.Id, s);
                    }
                }
            }
        }

        /// <summary>
        /// Servers way to add new snakes
        /// </summary>
        /// <param name="s"></param>
        public void addSnake(Snake s)
        {
            s.Direction = r.Next(1, 5);
            Model.Point head = null;
            Model.Point tail = null;
            bool space = false; //variable used to identify if there is space for the snake to be placed without touching anything

            //length of snake       if shrinkmode is enabled lenght is 11/20 of the size            otherwise it starts at 15
            int initialSnakeSize = shrinkMode ? (int)(11 * (width > height ? height : width) / 20) : 15;

            // head is moved around until the snake can safely fit in the world
            while (!space)
            {
                space = true;

                switch (s.Direction)
                {
                    case 1:
                        head = new Model.Point(r.Next(1, width - 1), r.Next((int)(2 * height / 5), height - initialSnakeSize));
                        for (int i = 0; i <= initialSnakeSize; i++)
                            if (world[head.X, head.Y + i] != null)
                                space = false;
                        if (head.Y + initialSnakeSize >= height)
                            space = false;
                        tail = new Model.Point(head.X, head.Y + initialSnakeSize);
                        break;
                    case 2:
                        head = new Model.Point(r.Next(initialSnakeSize, (int)(3 * width / 5)), r.Next(1, width - 1));
                        for (int i = 0; i <= initialSnakeSize; i++)
                            if (world[head.X - i, head.Y] != null)
                                space = false;
                        if (head.X - initialSnakeSize <= 0)
                            space = false;
                        tail = new Model.Point(head.X - initialSnakeSize, head.Y);
                        break;
                    case 3:
                        head = new Model.Point(r.Next(1, width - 1), r.Next(initialSnakeSize, (int)(3 * height / 5)));
                        for (int i = 0; i <= initialSnakeSize; i++)
                            if (world[head.X, head.Y - i] != null)
                                space = false;
                        if (head.Y + initialSnakeSize <= 0)
                            space = false;
                        tail = new Model.Point(head.X, head.Y - initialSnakeSize);
                        break;
                    default:
                        head = new Model.Point(r.Next((int)(2 * width / 5), width - initialSnakeSize), r.Next(1, width - 1));
                        for (int i = 0; i <= initialSnakeSize; i++)
                            if (world[head.X + i, head.Y] != null)
                                space = false;
                        if (head.X + initialSnakeSize >= width)
                            space = false;
                        tail = new Model.Point(head.X + initialSnakeSize, head.Y);
                        break;
                }
            }

            // update the snake verts
            s.Verts = new Model.Point[2] { tail, head };

            // add the snake to snakes
            lock (snakeLock)
                snakes.Add(s.Id, s);

            // fills in all snake cells in 2d world array
            switch (s.Direction)
            {
                case 1:
                    for (int i = 0; i <= initialSnakeSize; i++)
                        world[head.X, head.Y + i] = s;
                    break;
                case 2:
                    for (int i = 0; i <= initialSnakeSize; i++)
                        world[head.X - i, head.Y] = s;
                    break;
                case 3:
                    for (int i = 0; i <= initialSnakeSize; i++)
                        world[head.X, head.Y - i] = s;
                    break;
                default:
                    for (int i = 0; i <= initialSnakeSize; i++)
                        world[head.X + i, head.Y] = s;
                    break;
            }

        }

        /// <summary>
        /// returns a collection of all snakes
        /// </summary>
        /// <returns></returns>
        public ICollection<Snake> getSnakes()
        {
            lock(snakeLock)
            {
                return new HashSet<Snake>(snakes.Values);
            }
        }

        // Width of the world in cells (not pixels)
        public int width
        {
            get;
            private set;
        }

        // Height of the world in cells (not pixels)
        public int height
        {
            get;
            private set;
        }

        /// <summary>
        /// initailize a new world
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public World(int w, int h)
        {
            width = w;
            height = h;
        }

        /// <summary>
        /// returns a snake given an id, if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Snake findSnake(int id)
        {
            if (snakes.ContainsKey(id))
                return snakes[id];
            else
                return null;
        }

        /// <summary>
        /// Method for drawing all the food in the world
        /// </summary>
        /// <param name="e"></param>
        public void drawFood(PaintEventArgs e)
        {
            lock (foodLock)
            {
                foreach (Food f in foods.Values)
                {
                    using (System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(f.getColor()))
                    {
                        Rectangle dot = new Rectangle(f.Location.X * pixelsPerCell, f.Location.Y * pixelsPerCell, pixelsPerCell, pixelsPerCell);
                        e.Graphics.FillEllipse(drawBrush, dot);
                    }
                }
            }
        }

        /// <summary>
        /// method for drawing all the snakes in the world
        /// </summary>
        /// <param name="e"></param>
        public void drawSnakes(PaintEventArgs e)
        {
            lock (snakeLock)
            {
                foreach (Snake s in snakes.Values)
                {
                    Model.Point[] points = s.Verts;
                    for (int i = 0; i < s.VertsCount()-1; i++)
                    {
                        using (System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(s.getColor()))
                        {
                            Rectangle rect;
                            int width = (points[i].X - points[i + 1].X);
                            int height = (points[i].Y - points[i + 1].Y);

                            if (height == 0) //Moving horizontally
                            {
                                if (width > 0) //moving to the left?
                                    rect = new Rectangle(points[i + 1].X * pixelsPerCell, points[i + 1].Y * pixelsPerCell, width * pixelsPerCell, pixelsPerCell);
                                else //moving to the right?
                                    rect = new Rectangle(points[i].X * pixelsPerCell, points[i].Y * pixelsPerCell, -width * pixelsPerCell, pixelsPerCell);
                            }
                            else //Moving Vertically
                            {
                                if (height > 0) //moving to the up?
                                    rect = new Rectangle(points[i + 1].X * pixelsPerCell, points[i + 1].Y * pixelsPerCell, pixelsPerCell, height * pixelsPerCell);
                                else //moving to the down?
                                    rect = new Rectangle(points[i].X * pixelsPerCell, points[i].Y * pixelsPerCell, pixelsPerCell, -height * pixelsPerCell);
                            }
                            
                            e.Graphics.FillRectangle(drawBrush, rect);
                            e.Graphics.FillRectangle(drawBrush, new Rectangle(points[i].X * pixelsPerCell, points[i].Y * pixelsPerCell, pixelsPerCell, pixelsPerCell));
                            e.Graphics.FillRectangle(drawBrush, new Rectangle(points[i+1].X * pixelsPerCell, points[i+1].Y * pixelsPerCell, pixelsPerCell, pixelsPerCell));

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method for DrawingPanel
        /// Given the PaintEventArgs that comes from DrawingPanel, draw the contents of the world on to the panel.
        /// </summary>
        /// <param name="e"></param>
        public void DrawWalls(PaintEventArgs e)
        {
            using (System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(Color.Black))
            {

                // Draw the top wall
                Rectangle topWall = new Rectangle(0, 0, width * pixelsPerCell, pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw the right wall
                Rectangle rightWall = new Rectangle((width) * pixelsPerCell - pixelsPerCell, 0, pixelsPerCell, height * pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw the Bottom wall
                Rectangle bottomWall = new Rectangle(0, height * pixelsPerCell - pixelsPerCell, width * pixelsPerCell, pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw the left wall
                Rectangle leftWall = new Rectangle(0, 0, pixelsPerCell, height * pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);

                drawBrush.Color = Color.Gray;

                System.Drawing.Point p1 = new System.Drawing.Point(-width * pixelsPerCell, -height * pixelsPerCell);
                System.Drawing.Point p2 = new System.Drawing.Point(-width * pixelsPerCell, height * pixelsPerCell);
                System.Drawing.Point p3 = new System.Drawing.Point(width * pixelsPerCell, -height * pixelsPerCell);

                // Draw the top wall
                Rectangle topBox = new Rectangle(p1, new Size(3 * width * pixelsPerCell, height * pixelsPerCell));
                e.Graphics.FillRectangle(drawBrush, topBox);

                // Draw the right wall
                Rectangle rightBox = new Rectangle(p3, new Size(width * pixelsPerCell, 3 * height * pixelsPerCell));
                e.Graphics.FillRectangle(drawBrush, rightBox);

                // Draw the Bottom wall
                Rectangle bottomBox = new Rectangle(p2, new Size(3 * width * pixelsPerCell, height * pixelsPerCell));
                e.Graphics.FillRectangle(drawBrush, bottomBox);

                // Draw the left wall
                Rectangle leftBox = new Rectangle(p1, new Size(width * pixelsPerCell, 3 * height * pixelsPerCell));
                e.Graphics.FillRectangle(drawBrush, leftBox);

            }
        }

        /// <summary>
        /// server world constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="foodDensity"></param>
        /// <param name="snakeRecycleRate"></param>
        /// <param name="shrinkMode"></param>
        public World(int width, int height, int foodDensity, float snakeRecycleRate, bool shrinkMode)
        {
            world = new object[width, height];

            this.width = width;
            this.height = height;
            this.foodDensity = foodDensity;
            this.snakeRecycleRate = snakeRecycleRate;
            this.shrinkMode = shrinkMode;
        }

        /// <summary>
        /// advances the world by one step
        /// </summary>
        public void advanceWorld()
        {
            // remove any snakes or food that has been marked for garbage collection
            foreach ( Food f in garbageFood)
                foods.Remove(f.Id);

            lock(snakeLock)
                foreach (Snake s in garbageSnake)
                    snakes.Remove(s.Id);

            garbageFood.Clear();
            garbageSnake.Clear();

            // move the snakes around
            lock(snakeLock)
                foreach (Snake s in snakes.Values)
                {
                    s.advanceHead();
                    int colValue = collisionCheck(s);
                    if (colValue == 0)
                        s.removeTail();
                    else if(colValue == 2)
                    {
                        s.removeTail();
                        s.removeTail();
                    }
                }
            
            // re-add any food that is missing
            while (foods.Count < foodDensity * snakes.Count)
            {
                int id = foodID++;
                int x = r.Next(1, width - 1);
                int y = r.Next(1, height - 1);

                // make sure the random cell is not already occupied
                if (world[x, y] == null)
                {
                    Food f = new Food(id, new Model.Point(x, y));
                    foods.Add(id, f);
                    world[x, y] = f;
                }
            }
            
        }

        // helper method for detecting any type of collision 
        // return:
        //         0: no collision
        //         1: Food eaten in regularmode
        //         2: Food eaten in shrinkmode
        private int collisionCheck(Snake s)
        {
            Model.Point head = s.Head();
            // Check if the head has made contact with any wall
            if (head.X == 0 || head.X == width-1 || head.Y == 0 || head.Y == height-1)
            {
                snakeDeath(s);
                return 1;
            }
            else
            {
                object cellvalue = world[head.X, head.Y];

                // check if the head is on an empty cell
                if (cellvalue == null)
                {
                    world[head.X, head.Y] = s;
                    world[s.Verts[0].X, s.Verts[0].Y] = null;
                    return 0;
                }
                // check if the head is on a cell with food in it
                else if (object.ReferenceEquals(cellvalue.GetType(), typeof(Food)))
                {
                    world[head.X, head.Y] = s;
                    Food f = (Food)cellvalue;
                    f.Location = new Model.Point(-1, -1);
                    garbageFood.Add(f);

                    // shrink mode has to null the last two cells on the snake
                    if (shrinkMode)
                    {
                        if (s.Length > 4)
                        {
                            world[s.Verts[0].X, s.Verts[0].Y] = null;

                            if (s.Verts[0].X == s.Verts[1].X)
                            {
                                if (s.Verts[0].Y < s.Verts[1].Y)
                                {
                                    world[s.Verts[0].X, s.Verts[0].Y + 1] = null;
                                }
                                else
                                {
                                    world[s.Verts[0].X, s.Verts[0].Y - 1] = null;
                                }
                            }
                            else
                            {
                                if (s.Verts[0].X < s.Verts[1].X)
                                {
                                    world[s.Verts[0].X + 1, s.Verts[0].Y] = null;
                                }
                                else
                                {
                                    world[s.Verts[0].X - 1, s.Verts[0].Y] = null;
                                }
                            }
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
                // check if the head is in a cell occupied by another snake
                else if (object.ReferenceEquals(cellvalue.GetType(), typeof(Snake)))
                {
                    snakeDeath(s);
                    return 1;
                }

                return 0;
            }
        }

        // Helper method for killing a snake
        private void snakeDeath(Snake s)
        {
            for (int i = 1; i < s.Verts.Count(); i++)
            {
                Model.Point p1 = s.Verts[i - 1];
                Model.Point p2 = s.Verts[i];

                if (p1.X == p2.X) //moving vertically
                {
                    if (p1.Y > p2.Y) //moving down
                    {
                        for(int j = p1.Y; j > p2.Y; j--)
                        {
                            if (r.Next(0, 100) < snakeRecycleRate * 100)
                            {
                                int id = foodID++;
                                Food f = new Food(id, new Model.Point(p1.X, j));
                                foods.Add(id, f);
                                world[p1.X, j] = f;
                            }
                            else
                            {
                                world[p1.X, j] = null;
                            }
                        }
                    }
                    else //moving up
                    {
                        for (int j = p1.Y; j < p2.Y; j++)
                        {
                            if (r.Next(0, 100) < snakeRecycleRate * 100)
                            {
                                int id = foodID++;
                                Food f = new Food(id, new Model.Point(p1.X, j));
                                foods.Add(id, f);
                                world[p1.X, j] = f;
                            }
                            else
                            {
                                world[p1.X, j] = null;
                            }
                        }
                    }
                }
                else //Moving horizontally
                {
                    if (p1.X > p2.X) //moving left
                    {
                        for (int j = p1.X; j > p2.X; j--)
                        {
                            if (r.Next(0, 100) < snakeRecycleRate * 100)
                            {
                                int id = foodID++;
                                Food f = new Food(id, new Model.Point(j,p1.Y));
                                foods.Add(id, f);
                                world[j, p1.Y] = f;
                            }
                            else
                            {
                                world[j,p1.Y] = null;
                            }
                        }
                    }
                    else //moving right
                    {
                        for (int j = p1.X; j < p2.X; j++)
                        {
                            if (r.Next(0, 100) < snakeRecycleRate * 100)
                            {
                                int id = foodID++;
                                Food f = new Food(id, new Model.Point(j,p1.Y));
                                foods.Add(id, f);
                                world[j, p1.Y] = f;
                            }
                            else
                            {
                                world[j, p1.Y] = null;
                            }
                        }
                    }
                }

            }

            // the head of the snake is always set to null on snake death
            if (world[s.Head().X, s.Head().Y] != null)
                if (object.ReferenceEquals(world[s.Head().X, s.Head().Y].GetType(), typeof(Snake)))
                    world[s.Head().X, s.Head().Y] = null;

            //send data to sql server
            int length = s.Length;
            Thread t1 = new Thread(() => HighScore.addEntry(s.Name, s.AliveDuration.ToString(), length));
            t1.Start();

            // set the verts to -1, -1
            s.Verts = new Model.Point[2] { new Model.Point(-1, -1), new Model.Point(-1, -1) };
            // add the snake to the garbage
            garbageSnake.Add(s);
        }


        /// <summary>
        /// Change the direction of a snake's head
        /// </summary>
        /// <param name="snakeID"></param>
        /// <param name="direction"></param>
        public void changeDirection(int snakeID, int direction)
        {
            Snake s;
            lock (snakeLock)
            {
                int trueDirection;
                if (snakes.TryGetValue(snakeID, out s))
                {
                    if (s.Verts[s.Verts.Count() - 1].X == s.Verts[s.Verts.Count() - 2].X)
                    {
                        if (s.Verts[s.Verts.Count() - 1].Y < s.Verts[s.Verts.Count() - 2].Y) //Moving up
                        {
                            trueDirection = 1;
                        }
                        else //moving down
                        {
                            trueDirection = 3;
                        }
                    }
                    else
                    {
                        if (s.Verts[s.Verts.Count() - 1].X < s.Verts[s.Verts.Count() - 2].X) //Moving left
                        {
                            trueDirection = 4;
                        }
                        else //Moving right
                        {
                            trueDirection = 2;
                        }
                    }

                    // prevents a user from going a direction that is dumb
                    if (trueDirection % 2 != direction % 2)
                        s.Direction = direction;
                }
            }
        }

        /// <summary>
        /// returns a string containing the JSON string of all objects in the world
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Snake s in snakes.Values)
                sb.Append(s.ToString() + "\n");
            foreach (Food f in foods.Values)
                sb.Append(f.ToString() + "\n");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }

}
