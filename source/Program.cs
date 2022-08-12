using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;

namespace Cherry
{
    internal class Program
    {
        private static GL gl;
        private static Engine.Window gWindow;
        private static IKeyboard gKeyInput;
        private static IMouse gMouseInput;
        private static DateTime gElapsedTime;
        
        static void Main(string[] args)
        {
            gWindow = new Engine.Window("Cherry", 1280, 720);

            gWindow.LoadEvent = () => {
                gElapsedTime = DateTime.UtcNow;
                gWindow.SetIcon("icon.png");
                gl = GL.GetApi(gWindow.WindowHandle);
                gl.ClearColor(gWindow.ClearColor[0], gWindow.ClearColor[1], gWindow.ClearColor[2], gWindow.ClearColor[3]);
            };

            gWindow.UpdateEvent = (double deltaTime) => {
            };

            gWindow.RenderEvent = (double deltaTime) => {
                gl.Clear(ClearBufferMask.ColorBufferBit);
            };

            gWindow.ResizeEvent = (Vector2D<int> size) => {
                gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
            };

            gWindow.CloseEvent = () => {
                Environment.Exit(0);
            };

            gWindow.Run();
        }
    }
}