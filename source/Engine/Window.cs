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
    public class Window
    {
        public IWindow WindowHandle { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float[] ClearColor { get; private set; } = new float[] { 0.4f, 0.4f, 0.4f, 1.0f };
        public Action LoadEvent { get; set; }
        public unsafe Action<double> UpdateEvent { get; set; }
        public unsafe Action<double> RenderEvent { get; set; }
        public Action<Vector2D<int>> ResizeEvent { get; set; }
        public Action CloseEvent { get; set; }


        public Window(string title, int width, int height)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = title;
            Width = width;
            Height = height;
            options.Size = new Vector2D<int>(width, height);
            WindowHandle = Silk.NET.Windowing.Window.Create(options);
        }

        public void Run()
        {
            WindowHandle.Load += LoadEvent;
            WindowHandle.Update += UpdateEvent;
            WindowHandle.Render += RenderEvent;
            WindowHandle.Resize += ResizeEvent;
            WindowHandle.Closing += CloseEvent;

            WindowHandle.Run();
        }

        public void SetIcon(string filepath)
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
            WindowHandle.SetWindowIcon(new[] {icon});
        }
    }
}