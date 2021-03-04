using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Aster.Client.Base
{
    public class ShaderProgram : IDisposable
    {
        private readonly int _nativeShaderProgram;
        private readonly IReadOnlyDictionary<string, ShaderUniform> _uniforms;

        internal ShaderProgram(
            int nativeShaderProgram,
            string name,
            IReadOnlyDictionary<string, ShaderUniform> uniforms,
            VertexType allowedVertexType)
        {
            _nativeShaderProgram = nativeShaderProgram;
            _uniforms = uniforms;
            AllowedVertexType = allowedVertexType;

            GL.ObjectLabel(ObjectLabelIdentifier.Shader, nativeShaderProgram, name.Length, name);
        }

        public VertexType AllowedVertexType { get; }

        public void Dispose()
        {
            GL.DeleteProgram(_nativeShaderProgram);
        }

        public void SetSampler(string uniformName, int value)
        {
            if (_uniforms.TryGetValue(uniformName, out var uniform) && uniform.UniformType == ActiveUniformType.Sampler2D)
            {
                GL.Uniform1(uniform.Location, value);
                return;
            }

            System.Diagnostics.Debug.WriteLine($"{uniformName} not found in shader");
        }

        public void SetUniform(string uniformName, float value)
        {
            if (_uniforms.TryGetValue(uniformName, out var uniform) && uniform.UniformType == ActiveUniformType.Float)
            {
                GL.Uniform1(uniform.Location, value);
                return;
            }

            System.Diagnostics.Debug.WriteLine($"{uniformName} not found in shader");
        }

        public void SetUniform(string uniformName, Vector4 value)
        {
            if (_uniforms.TryGetValue(uniformName, out var uniform) && uniform.UniformType == ActiveUniformType.FloatVec4)
            {
                GL.Uniform4(uniform.Location, value.X, value.Y, value.Z, value.W);
                return;
            }

            System.Diagnostics.Debug.WriteLine($"{uniformName} not found in shader");
        }

        public void SetUniform(string uniformName, Matrix4 value)
        {
            if (_uniforms.TryGetValue(uniformName, out var uniform) && uniform.UniformType == ActiveUniformType.FloatMat4)
            {
                GL.UniformMatrix4(uniform.Location, true, ref value);
                return;
            }

            System.Diagnostics.Debug.WriteLine($"{uniformName} not found in shader");
        }

        public void Use()
        {
            GL.UseProgram(_nativeShaderProgram);
        }
    }
}
