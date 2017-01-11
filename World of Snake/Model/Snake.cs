
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Model
{
    /// <summary>
    /// Snake class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Snake
    {
        // Id of the snake
        [JsonProperty]
        private int ID;

        // name of the snake
        [JsonProperty]
        private string name;

        // vertices of the snake
        [JsonProperty]
        private List<Point> vertices;

        //color of the snake
        private Color color;

        private DateTime startTime;

        /// <summary>
        /// initalize a new snake
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        public Snake(int ID, string name)
        {
            this.ID = ID;
            this.name = name;
            vertices = new List<Point>();
            color = Color.Black;
            startTime = DateTime.Now;
        }

        /// <summary>
        /// method for retrieving the heads location
        /// </summary>
        /// <returns></returns>
        public Point Head()
        {
            return vertices.Last();
        }

        public TimeSpan AliveDuration { get { return DateTime.Now - startTime; } }

        public int Direction { get; set; }

        /// <summary>
        /// method for setting the color based on RGB provided
        /// </summary>
        /// <param name="Red"></param>
        /// <param name="Green"></param>
        /// <param name="Blue"></param>
        public void setColor(int Red, int Green, int Blue)
        {
            color = Color.FromArgb(Red, Green, Blue);
        }

        /// <summary>
        /// method for getting the color fo the snake
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return color;
        }

        /// <summary>
        /// method for getting the size of vertices
        /// </summary>
        /// <returns></returns>
        public int VertsCount()
        {
            return vertices.Count;
        }

        /// <summary>
        /// method for getting the vertices
        /// </summary>
        public Point[] Verts
        {
            get
            {
                return vertices.ToArray();
            }
            set
            {
                vertices = value.ToList();
            }
        }

        /// <summary>
        /// ID property of Snake
        /// </summary>
        public int Id { get { return ID; } }

        /// <summary>
        /// Name property of Snake
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// total length of the snake
        /// </summary>
        public int Length
        {
            get
            {
                lock (this)
                {
                    int l = 0;
                    Point[] points = vertices.ToArray();
                    for (int i = 0; i < VertsCount() - 1; i++)
                    {
                        l += Math.Abs(points[i].X - points[i + 1].X) + Math.Abs(points[i].Y - points[i + 1].Y);
                    }
                    return l;
                }
            }
        }

        /// <summary>
        /// Moves the head one cell in the direction of the snake
        /// </summary>
        public void advanceHead()
        {
            // is the first segment of the snake horizontal?
            bool horizontal = vertices[vertices.Count-1].Y == vertices[vertices.Count-2].Y ? true : false;

            if (Direction == 1)
            {
                if (horizontal)
                {
                    vertices.Add(new Point(vertices.Last().X, vertices.Last().Y - 1));
                }
                else
                {
                    vertices[vertices.Count - 1] = new Point(vertices.Last().X, vertices.Last().Y - 1);
                }
            }
            else if (Direction == 2)
            {
                if (!horizontal)
                {
                    vertices.Add(new Point(vertices.Last().X + 1, vertices.Last().Y));
                }
                else
                {
                    vertices[vertices.Count - 1] = new Point(vertices.Last().X + 1, vertices.Last().Y);
                }
            }
            else if (Direction == 3)
            {
                if (horizontal)
                {
                    vertices.Add(new Point(vertices.Last().X, vertices.Last().Y + 1));
                }
                else
                {
                    vertices[vertices.Count - 1] = new Point(vertices.Last().X, vertices.Last().Y + 1);
                }
            }
            else if (Direction == 4)
            {
                if (!horizontal)
                {
                    vertices.Add(new Point(vertices.Last().X - 1, vertices.Last().Y));
                }
                else
                {
                    vertices[vertices.Count - 1] = new Point(vertices.Last().X - 1, vertices.Last().Y);
                }
            }

        }

        /// <summary>
        /// Removes the last cell on the snake
        /// </summary>
        public void removeTail()
        {
            // is the last segment of the snake horizontal?
            bool horizontal = vertices[0].Y == vertices[1].Y ? true : false;

            if (horizontal)
            {
                if (vertices[0].X < vertices[1].X) //moving left
                {
                    if (Math.Abs(vertices[0].X - vertices[1].X) <= 1) //if the last segment is only one cell long
                    {
                        vertices.RemoveAt(0);
                    }
                    else
                    {
                        vertices[0] = new Point(vertices[0].X+1, vertices[0].Y);
                    }
                }
                else //moving right
                {
                    if (Math.Abs(vertices[0].X - vertices[1].X) <= 1) //if the last segment is only one cell long
                    {
                        vertices.RemoveAt(0);
                    }
                    else
                    {
                        vertices[0] = new Point(vertices[0].X - 1, vertices[0].Y);
                    }
                }
            }
            else
            {
                if (vertices[0].Y > vertices[1].Y) //moving up
                {
                    if (Math.Abs(vertices[0].Y - vertices[1].Y) <= 1) //if the last segment is only one cell long
                    {
                        vertices.RemoveAt(0);
                    }
                    else
                    {
                        vertices[0] = new Point(vertices[0].X, vertices[0].Y - 1);
                    }
                }
                else //moving down
                {
                    if (Math.Abs(vertices[0].Y - vertices[1].Y) <= 1) //if the last segment is only one cell long
                    {
                        vertices.RemoveAt(0);
                    }
                    else
                    {
                        vertices[0] = new Point(vertices[0].X, vertices[0].Y + 1);
                    }
                }
            }
        }

        /// <summary>
        /// returns the JSON string representation of the snake
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}