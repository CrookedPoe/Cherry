using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

using Silk.NET.OpenGL;

using Cherry.Engine;

namespace Cherry.Engine.ModelHandler
{
    public class Material : IDisposable
    {
        public enum ModelType
        {
            Wavefront,
            F3DEX2
        };

        public Texture Texture { get; set; }
        public Shader Shader { get; set; }
        public Vector3 AmbientColor { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public float Shininess { get; set; }

        public Material(GL gl, Shader shader, ModelType type, dynamic data)
        {
            if (type == ModelType.Wavefront)
            {
                InitializeFromWavefrontMaterial(gl, shader, (Wavefront.Material)data);
            }
            else
            {
                Shader = shader;
                Texture = new Texture(gl, null);
            }
            //Initialize(gl);
        }

        private void InitializeFromWavefrontMaterial(GL gl, Shader shader, Wavefront.Material material)
        {
            Shader = shader;
            Texture = new Texture(gl, material.DiffuseTextureMap);
            AmbientColor = material.AmbientColor;
            DiffuseColor = material.DiffuseColor;
            SpecularColor = material.SpecularColor;
            Shininess = material.Shininess;
        }

        public void Bind(string textureName)
        {
            Texture.Bind(TextureUnit.Texture0);
            Shader.SetUniform("uTexel0", 0);
            Shader.SetUniform("material.Ka", AmbientColor);
            Shader.SetUniform("material.Kd", DiffuseColor);
            Shader.SetUniform("material.Ks", SpecularColor);
            Shader.SetUniform("material.Ns", Shininess);
        }

        public void Dispose()
        {
            Shader.Dispose();
            Texture.Dispose();
        }
    }
}