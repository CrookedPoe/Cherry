using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Cherry.Engine
{
    public class InputHandler
    {
        public class InputEvents
        {
            public Action<IKeyboard, Key, int> KeyDown { get; set; }
            public unsafe Action<IMouse, Vector2> OnMouseMove { get; set; }
            public Action<IMouse, ScrollWheel> OnMouseWheel { get; set; }
        }

        public IInputContext InputContext { get; private set; }
        public IKeyboard KeyInput { get; private set; }
        public IMouse MouseInput { get; private set; }
        public InputEvents Events { get; set; }

        public InputHandler(IWindow window)
        {
            InputContext = window.CreateInput();
            KeyInput = InputContext.Keyboards.FirstOrDefault();
            MouseInput = InputContext.Mice.FirstOrDefault();
            InitializeEvents();
        }

        public void InitializeEvents()
        {   
            Events = new InputEvents();
            if (KeyInput != null)
            {
                KeyInput.KeyDown += Events.KeyDown;
            }
            if (MouseInput != null)
            {
                MouseInput.MouseMove += Events.OnMouseMove;
                MouseInput.Scroll += Events.OnMouseWheel;
            }
        }
    }
}