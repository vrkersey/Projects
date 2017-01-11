using System.Windows.Forms;
using SnakeGame;

namespace DrawingPanel
{


    /// <summary>
    /// This is a helper class for drawing a world
    /// </summary>
    public class DrawingPanel : Panel
    {
        public float zoom;
        public Model.Point playerHead;

        // We need a reference to the world, so we can draw the objects in it
        private World world;


            public DrawingPanel()
        {
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Pass in a reference to the world, so we can draw the objects in it
        /// </summary>
        /// <param name="_world"></param>
        public void SetWorld(World _world)
        {
            world = _world;
        }

        /// <summary>
        /// Override the behavior when the panel is redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // If we don't have a reference to the world yet, nothing to draw.
            if (world == null)
                return;

            // Zoom and center Transform
            if (playerHead != null && zoom != 0)
            {
                float zoomfactorX = (float)world.width / zoom;
                float zoomfactorY = (float)world.height / zoom;
                e.Graphics.ScaleTransform(zoomfactorX, zoomfactorY);
                if (world.width / zoom > 1.0)
                    e.Graphics.TranslateTransform((float)world.width / (float)(2) * (float)World.pixelsPerCell / zoomfactorX - (float)playerHead.X * (float)World.pixelsPerCell,
                        (float)world.height / (float)(2) * (float)World.pixelsPerCell / zoomfactorY - (float)playerHead.Y * (float)World.pixelsPerCell);
                else
                    e.Graphics.ResetTransform();
            }
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // redraw the components of the world
            world.DrawWalls(e);
            world.drawFood(e);
            world.drawSnakes(e);
            
        }
    }
}

