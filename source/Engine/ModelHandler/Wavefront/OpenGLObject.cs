using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

using Silk.NET.OpenGL;

namespace Cherry.Engine.ModelHandler.Wavefront
{
    public class OpenGLObject : IDisposable
    {
        private GL _gl;
        private BufferObject<float> _VBO;
        private BufferObject<uint> _EBO;
        private VertexArrayObject<float, uint> _VAO;
        private Shader _shader;
        public Wavefront.Object OBJ { get; set; }
        public Dictionary<string, Cherry.Engine.ModelHandler.Material> Materials { get; set; }

        public OpenGLObject(GL gl, string filepath)
        {
            _gl = gl;
            _shader = new Shader(gl, "Shaders/default/vertex.glsl", "Shaders/default/fragment.glsl");
            OBJ = new Wavefront.Object(filepath);
            Materials = new Dictionary<string, Cherry.Engine.ModelHandler.Material>();
            
            float[] VertexBuffer = new float[0]; // TODO
            uint[] IndexBuffer = new uint[0]; // TODO

            // Populate Materials
            foreach (var material in OBJ.MaterialLibrary.Materials)
            {
                var materialSource = new Cherry.Engine.ModelHandler.Material(_gl, _shader, ModelHandler.Material.ModelType.Wavefront, OBJ.MaterialLibrary.Materials[material.Key]);
                Materials.Add(material.Key, materialSource);
            }

            // Initialize VAO
            _VBO = new BufferObject<float>(gl, VertexBuffer, BufferTargetARB.ArrayBuffer);
            _EBO = new BufferObject<uint>(gl, IndexBuffer, BufferTargetARB.ElementArrayBuffer);
            _VAO = new VertexArrayObject<float, uint>(gl, _VBO, _EBO);
            _VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 0); // Position
            _VAO.VertexAttributePointer(0, 2, VertexAttribPointerType.Float, 8, 3); // Texture Coordinates
            _VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 5); // Normals
        }

        public void Render(GL gl, Camera camera, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
        {
            _VAO.Bind();
            _shader.Use();
            _shader.SetUniform("uModel", model);
            _shader.SetUniform("uView", view);
            _shader.SetUniform("uProjection", projection);
            _shader.SetUniform("cameraPosition", camera.Eye);

            // Drawing Logic / TODO

            Vector3 lightColor = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 lightPosition = new Vector3(1.0f, 1.0f, 10.0f);
            _shader.SetUniform("light.ambient", lightColor * 0.5f);
            _shader.SetUniform("light.diffuse", lightColor);
            _shader.SetUniform("light.specular", lightColor);
            _shader.SetUniform("light.position", lightPosition);

        }

        public void Dispose()
        {
            _VBO.Dispose();
            _EBO.Dispose();
            _VAO.Dispose();
            foreach (var material in Materials)
            {
                material.Value.Dispose();
            }
        }
    }
}