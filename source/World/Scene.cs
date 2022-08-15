using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Silk.NET.OpenGL;

using Cherry.Engine;

namespace Cherry.World
{
    public class Scene
    {
        public FloorGrid Grid { get; set; }
        public Camera Camera { get; set; }

        public Scene(GL gl)
        {
            // Initialize Grid
            Grid = new FloorGrid(gl);

            // Initialize Camera
            Camera = Camera.Initialize();
        }
    }
}