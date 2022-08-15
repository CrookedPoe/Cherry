using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;

using Cherry.Engine;
using Cherry.Engine.ModelHandler;
using Cherry.World;
using Cherry.Math3D;

namespace Cherry
{
    public static class Configuration
    {
        public static int gWindowWidth = 1280;
        public static int gWindowHeight = 720;
        public static float[] gBackgroundColor = new float[4] { 0.16f, 0.16f, 0.16f, 1.0f };
        public static Matrix4x4 gModelMatrix = Matrix4x4.Identity;
        public static Matrix4x4 gViewMatrix = Matrix4x4.Identity;
        public static Matrix4x4 gProjectionMatrix = Matrix3D.Matrix44.Projection(gWindowWidth, gWindowHeight, 0.1f, 500.0f).MtxF4x4;
    }
    class Program
    {
        private static GL gl;
        private static IWindow mainWindow;
        private static IKeyboard mainKeyInput;
        private static IMouse mainMouseInput;
        private static DateTime aliveTime;
        private static World.FloorGrid floorGrid;
        private static Engine.Camera mainCamera;
        private static Cherry.Engine.ModelHandler.Wavefront.OpenGLObject OBJ;

        public class WindowEvents
        {
            public static void Initialize()
            {
                //Console.WriteLine("Initializing Window");
                aliveTime = DateTime.UtcNow;
                mainWindow.SetIcon("icon.png");
                
                #region Input
                IInputContext gInput = mainWindow.CreateInput();
                mainKeyInput = gInput.Keyboards.FirstOrDefault();
                mainMouseInput = gInput.Mice.FirstOrDefault();

                if (mainKeyInput == null)
                {
                    Console.WriteLine("No Keyboard Found");
                    return;
                }
                else
                {
                    mainKeyInput.KeyDown += InputEvents.KeyDown;
                }

                if (mainMouseInput == null)
                {
                    Console.WriteLine("No Mouse Found");
                    return;
                }
                else
                {
                    mainMouseInput.Cursor.CursorMode = CursorMode.Normal;
                    mainMouseInput.MouseMove += InputEvents.OnMouseMove;
                    mainMouseInput.Scroll += InputEvents.OnMouseWheel;
                }
                #endregion

                
                gl = GL.GetApi(mainWindow);
                gl.ClearColor(0.27f, 0.27f, 0.27f, 1.0f);

                // Initialize Floor Grid
                floorGrid = new World.FloorGrid(gl);

                // Initialize Object
                OBJ = new Cherry.Engine.ModelHandler.Wavefront.OpenGLObject(gl, "Z:\\source\\repos\\Cherry\\objects\\multitex\\multitex.obj");

                // Initialize Camera
                mainCamera = Engine.Camera.Initialize();
            }
            public static unsafe void OnUpdate(double deltaTime)
            {
                //Console.WriteLine("OnUpdate");
                mainCamera.Move(mainKeyInput, deltaTime);
            }
            public static unsafe void OnRender(double deltaTime)
            {
                //Console.WriteLine("OnRender");
                gl.Enable(EnableCap.DepthTest);
                gl.DepthFunc(DepthFunction.Less);

                gl.Enable(EnableCap.Blend);
                gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                floorGrid.Draw(gl, Matrix4x4.Identity, mainCamera.GetViewMatrix(), Configuration.gProjectionMatrix);

            }
            public static void OnResize(Vector2D<int> clientSize)
            {
                //Console.WriteLine("OnResize");
                gl.Viewport(0, 0, (uint)clientSize.X, (uint)clientSize.Y);
                Configuration.gProjectionMatrix = Matrix3D.Matrix44.Projection(clientSize.X, clientSize.Y, 0.1f, 500.0f).MtxF4x4;
            }
            public static void OnClose()
            {
                Console.WriteLine("OnClose");
                OBJ.Dispose();
                floorGrid.Dispose();
            }
        }

        public class InputEvents
        {
            public static void KeyDown(IKeyboard sender, Key key, int arg3)
            {
                switch(key)
                {
                    case Key.Escape:
                        mainWindow.Close();
                        break;
                }
            }
            public static unsafe void OnMouseMove(IMouse mouse, Vector2 newMousePos)
            {
                //Console.WriteLine("OnMouseMove");
                float moveSensitivity = 0.1f;
                Vector2 delta = Vector2.Zero;

                if (mainCamera.oldMousePos == default)
                {
                    mainCamera.oldMousePos = newMousePos;
                }
                else
                {
                        delta = newMousePos - mainCamera.oldMousePos;
                        mainCamera.oldMousePos = newMousePos;
                }

                // Orbit Camera
                if (mouse.IsButtonPressed(MouseButton.Middle))
                {
                    mainCamera.ModifyDirection(delta.X * moveSensitivity, delta.Y * moveSensitivity);
                }

                // Pan Camera
            }
            public static void OnMouseWheel(IMouse mouse, ScrollWheel scroll)
            {
                //Console.WriteLine("OnMouseWheel");
                // Zoom Camera
                mainCamera.ModifyZoom(scroll.Y);
            }
        }
        static void Main(string[] args)
        {
            // Initialize and Run Window
            WindowOptions winOptions = WindowOptions.Default;
            winOptions.Size = new Vector2D<int>(Configuration.gWindowWidth, Configuration.gWindowHeight);
            winOptions.Title = "Cherry";
            mainWindow = Window.Create(winOptions);

            // Assign Window Events
            mainWindow.Load += WindowEvents.Initialize;
            mainWindow.Update += WindowEvents.OnUpdate;
            mainWindow.Render += WindowEvents.OnRender;
            mainWindow.Resize += WindowEvents.OnResize;
            mainWindow.Closing += WindowEvents.OnClose;

            // Run Window
            mainWindow.Run();
        }
    }
}