using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Silk.NET.OpenGL;

using Cherry.Engine;

namespace Cherry.World
{
    public class GridVertex
    {
        public Vector3 Position { get; set; }
        public Vector4 Color { get; set; }
        public float[] Buffer => new float[] { Position.X, Position.Y, Position.Z, Color.X, Color.Y, Color.Z, Color.W };

        public GridVertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
    public class FloorGrid : IDisposable
    {
        List<GridVertex> gridVertices { get; set; }
        public float[] VertexBuffer { get; set; }
        public uint[] IndexBuffer { get; set; }
        public BufferObject<float> VBO;
        public BufferObject<uint> EBO;
        public VertexArrayObject<float, uint> VAO;
        public Cherry.Engine.Shader Shader;

        public FloorGrid(GL gl)
        {
            Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
            Vector4 redColor = new Vector4(1.0f, 0.3f, 0.3f, 1.0f);
            Vector4 blueColor = new Vector4(0.3f, 0.3f, 1.0f, 1.0f);
            gridVertices = new List<GridVertex>();

            for (int i = -8; i <= 8; i++)
            {
                Vector4 color;

                if (i == 0)
                    color = redColor;
                else
                    color = defaultColor;
                gridVertices.Add(new GridVertex(new Vector3((float)i, 0.0f, -8.0f), color));
                gridVertices.Add(new GridVertex(new Vector3((float)i, 0.0f, 8.0f), color));
                if (i == 0)
                    color = blueColor;
                else
                    color = defaultColor;
                gridVertices.Add(new GridVertex(new Vector3(-8.0f, 0.0f, (float)i), color));
                gridVertices.Add(new GridVertex(new Vector3(8.0f, 0.0f, (float)i), color));
            }

            List<float[]> v = new List<float[]>();
            for (int i = 0; i < gridVertices.Count; i++)
            {
                v.Add(gridVertices[i].Buffer);
            }
            VertexBuffer = v.SelectMany(x => x).ToArray();

            IndexBuffer = Enumerable.Range(0, VertexBuffer.Length / 2).Select(x => (uint)x).ToArray();

            // Initialize Buffer Objects
            VBO = new BufferObject<float>(gl, VertexBuffer, BufferTargetARB.ArrayBuffer);
            EBO = new BufferObject<uint>(gl, IndexBuffer, BufferTargetARB.ElementArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(gl, VBO, EBO);
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 7, 0);
            VAO.VertexAttributePointer(1, 4, VertexAttribPointerType.Float, 7, 3);

            // Initialize Shader
            Shader = new Cherry.Engine.Shader(gl, "Shaders/grid/vertex.glsl", "Shaders/grid/fragment.glsl");
        }

        public void Draw(GL gl, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
        {
            VAO.Bind();
            Shader.Use();
            Shader.SetUniform("uModel", model);
            Shader.SetUniform("uView", view);
            Shader.SetUniform("uProjection", projection);
            gl.LineWidth(1.0f);
            gl.DrawArrays(PrimitiveType.Lines, 0, (uint)VertexBuffer.Length);
        }

        public void Dispose()
        {
            VAO.Dispose();
            VBO.Dispose();
            EBO.Dispose();
            Shader.Dispose();
        }
    }
}