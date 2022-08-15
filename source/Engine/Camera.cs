using System;
using System.Numerics;

using Silk.NET.Input;

using Cherry.Math3D;

namespace Cherry.Engine
{
    public static class CameraEvents
    {
        public static void Move(this Camera cam, IKeyboard keyInput, double deltaTime)
        {
            float camCoeff = 8.0f;
            float moveSpeed = camCoeff * ((float)deltaTime);
            
            // Faster.
            if (keyInput.IsKeyPressed(Key.Space))
            {
                camCoeff = 16.0f;
                moveSpeed = camCoeff * ((float)deltaTime);
            }
            
            // Slower.
            if (keyInput.IsKeyPressed(Key.ShiftLeft))
            {
                camCoeff = 1.0f;
                moveSpeed = camCoeff * ((float)deltaTime);
            }
            
            // Move forward.
            if (keyInput.IsKeyPressed(Key.W))
            {
                cam.Eye += cam.Target * moveSpeed;
                //Console.WriteLine("Move Forward");
            }
            
            // Move backward.
            if (keyInput.IsKeyPressed(Key.S))
            {
                cam.Eye -= cam.Target * moveSpeed;
                //Console.WriteLine("Move Backward");
            }
            
            // Move left.
            if (keyInput.IsKeyPressed(Key.A))
            {
                cam.Eye -= Vector3.Normalize(Vector3.Cross(cam.Target, cam.Up)) * moveSpeed;
                //Console.WriteLine("Move Left");
            }
            
            // Move right.
            if (keyInput.IsKeyPressed(Key.D))
            {
                cam.Eye += Vector3.Normalize(Vector3.Cross(cam.Target, cam.Up)) * moveSpeed;
                //Console.WriteLine("Move Right");
            }
        }

        public static void ModifyZoom(this Camera cam, float zoomAmount)
        {
            cam.Zoom = Math.Clamp(cam.Zoom - zoomAmount, 1.0f, 45.0f);
        }

        public static void ModifyDirection(this Camera cam, float xOffset, float yOffset)
        {   
            cam.Yaw += xOffset;
            cam.Pitch -= yOffset;
            cam.Pitch = Math.Clamp(cam.Pitch, -89.0f, 89.0f);

            Vector3 camDir = cam.Target;
            camDir.X = MathF.Cos(cam.Yaw.ToRadians()) * MathF.Cos(cam.Pitch.ToRadians());
            camDir.Y = MathF.Sin(cam.Pitch.ToRadians());
            camDir.Z = MathF.Sin(cam.Yaw.ToRadians()) * MathF.Cos(cam.Pitch.ToRadians());

            cam.Target = Vector3.Normalize(camDir);
        }

        public static void ModifyPosition(this Camera cam, float xOffset, float yOffset)
        {
            Vector3 camRight = Vector3.Normalize(Vector3.Cross(cam.Target, cam.Up));
            cam.Eye += xOffset * camRight;
            cam.Eye += yOffset * cam.Up;
        }

        public static Matrix4x4 GetViewMatrix(this Camera cam)
        {
            return Matrix4x4.CreateLookAt(cam.Eye, cam.Eye + cam.Target, cam.Up);
        }
    }
    public class Camera
    {
        // Position
        public Vector3 Eye { get; set; }
        // Front
        public Vector3 Target { get; set; }
        public Vector3 Up { get; set; }
        public Matrix4x4 View { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
        public float Zoom {get; set;}
        public Vector2 oldMousePos { get; set; }

        public Camera(Vector3 eye, Vector3 target, Vector3 up)
        {
            Eye = eye;
            Target = target;
            Up = up;
        }

        public static Camera Initialize()
        {
            Vector3 eye = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 target = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

            return new Camera(eye, target, up);
        }
    }
}