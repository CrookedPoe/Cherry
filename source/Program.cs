using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

using Silk.NET.Input;
using Silk.NET.OpenGL;
//using Silk.NET.Windowing;
using Silk.NET.Maths;

using Cherry.Engine.WindowHandler;

namespace Cherry
{
    internal class Program
    {
        private static GL gl;
        private static WindowHandler gWindowContext;
        //private static IKeyboard gKeyInput;
        //private static IMouse gMouseInput;
        private static DateTime gElapsedTime;
        
        static void Main(string[] args)
        {
            gWindowContext = new WindowHandler("Cherry", 1280, 720);

            gWindowContext.Events.LoadEvent = () => {
                gElapsedTime = DateTime.UtcNow;
                gWindowContext.SetIcon("icon.png");
                gl = GL.GetApi(gWindowContext.Window);
                //gl.ClearColor(gWindow.ClearColor[0], gWindow.ClearColor[1], gWindow.ClearColor[2], gWindow.ClearColor[3]);
            };

            gWindowContext.Events.UpdateEvent = (double deltaTime) => {
            };

            gWindowContext.Events.RenderEvent = (double deltaTime) => {
                gl.Clear(ClearBufferMask.ColorBufferBit);
            };

            gWindowContext.Events.ResizeEvent = (Vector2D<int> size) => {
                gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
            };

            gWindowContext.Events.CloseEvent = () => {
                Environment.Exit(0);
            };

            gWindowContext.Run();
        }
    }
}