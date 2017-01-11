using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model
{
    /// <summary>
    /// Custom Point class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Point
    {
        // x component of the point
        [JsonProperty]
        private int x;

        // y component of the point
        [JsonProperty]
        private int y;

        /// <summary>
        /// initailize a new point
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        /// <summary>
        /// returns the X component of the point
        /// </summary>
        public int X{ get { return x; } }

        /// <summary>
        /// returns the Y component of the point
        /// </summary>
        public int Y { get { return y; } }

        /// <summary>
        /// Json serialized object string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
