using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Silk.NET.Windowing;
using Silk.NET.Maths;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cherry.Engine
{
    public static class Extensions
    {
        public static void SetIcon(this IWindow window, string filepath)
        {
            // Load Image
            var img = Image.Load<Rgba32>(filepath);
            Span<Rgba32> iconData = new Span<Rgba32>(new Rgba32[img.Width * img.Height]);
            img.CopyPixelDataTo(iconData);

            // Copy Image to Byte Array
            byte[] pixels = new byte[img.Width * img.Height * 4];
            for (int i = 0; i < iconData.Length; i++)
            {
                pixels[i * 4 + 0] = iconData[i].R;
                pixels[i * 4 + 1] = iconData[i].G;
                pixels[i * 4 + 2] = iconData[i].B;
                pixels[i * 4 + 3] = iconData[i].A;
            }

            // Create Icon
            Silk.NET.Core.RawImage icon = new Silk.NET.Core.RawImage(img.Width, img.Height, new Memory<byte>(pixels));
            window.SetWindowIcon(new[] {icon});
        }
    }
    public class WindowHandler
    {
        public class WindowEvents
        {
            public Action LoadEvent { get; set; }
            public unsafe Action<double> UpdateEvent { get; set; }
            public unsafe Action<double> RenderEvent { get; set; }
            public Action<Vector2D<int>> ResizeEvent { get; set; }
            public Action CloseEvent { get; set; }
        }

        public IWindow Window { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public WindowEvents Events { get; set; }

        public WindowHandler(string title, int width, int height)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = title;
            options.Size = new Vector2D<int>(width, height);
            Window = Silk.NET.Windowing.Window.Create(options);
            Width = options.Size.X;
            Height = options.Size.Y;
            Events = new WindowEvents();
        }

        public void Run()
        {
            Window.Load += Events.LoadEvent;
            //Window.FileDrop
            //Window.StateChanged
            //Window.Move
            //Window.FramebufferResize
            //Window.FocusChanged
            Window.Update += Events.UpdateEvent;
            Window.Render += Events.RenderEvent;
            Window.Resize += Events.ResizeEvent;
            Window.Closing += Events.CloseEvent;

            Window.Run();
        }
    }
}