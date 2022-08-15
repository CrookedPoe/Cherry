using System;
using System.IO;
using System.Numerics;

using Silk.NET.OpenGL;

namespace Cherry.Engine
{
    public class Shader : IDisposable
    {
        private uint _id;
        private GL _gl;

        public Shader(GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            _id = gl.CreateProgram();
            _gl.AttachShader(_id, vertex);
            _gl.AttachShader(_id, fragment);
            _gl.LinkProgram(_id);
            _gl.GetProgram(_id, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_id)}");
            }
            _gl.DetachShader(_id, vertex);
            _gl.DetachShader(_id, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public uint LoadShader(ShaderType type, string path)
        {
            string source = File.ReadAllText(path);
            uint id = _gl.CreateShader(type);
            _gl.ShaderSource(id, source);
            _gl.CompileShader(id);

            string infoLog = _gl.GetShaderInfoLog(id);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"{type} Shader Compile Error: {infoLog}");
            }

            return id;
        }

        public void SetUniform(string name, int value)
        {
            int location = _gl.GetUniformLocation(_id, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = _gl.GetUniformLocation(_id, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            int location = _gl.GetUniformLocation(_id, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform2(location, value.X, value.Y);
        }

        public void SetUniform(string name, Vector3 value)
        {
            int location = _gl.GetUniformLocation(_id, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform3(location, value.X, value.Y, value.Z);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            int location = _gl.GetUniformLocation(_id, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.UniformMatrix4(location, 1, false, (float*) &value);
        }

        public void Use()
        {
            _gl.UseProgram(_id);
        }

        public void Dispose()
        {
            _gl.DeleteProgram(_id);
        }
    }
}