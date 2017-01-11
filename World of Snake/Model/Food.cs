using Newtonsoft.Json;
using System.Drawing;


namespace Model
{
    /// <summary>
    /// Food class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Food
    {
        // ID of the food
        [JsonProperty]
        private int ID;

        // x,y location of the food
        [JsonProperty]
        private Point loc;

        // color of the food
        private Color color;
        
        /// <summary>
        /// initialize a new food 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="location"></param>
        public Food(int ID, Point location)
        {
            this.ID = ID;
            loc = location;
        }
        
        /// <summary>
        /// sets the color of the food based on RGB values provided
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void setColor(int Red, int Green, int Blue)
        {
            color = Color.FromArgb(Red, Green, Blue);
        }

        /// <summary>
        /// retrieve the color of the food
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return color;
        }

        /// <summary>
        /// get and set the ID
        /// </summary>
        public int Id { get { return ID; } }

        /// <summary>
        /// get and set the Location
        /// </summary>
        public Point Location
        {
            get
            {
                return loc;
            }
            set
            {
                loc = value;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
