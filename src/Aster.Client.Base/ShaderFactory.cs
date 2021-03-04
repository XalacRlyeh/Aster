using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using Serilog;

namespace Aster.Client.Base
{
    public sealed class ShaderFactory : IShaderFactory
    {
        private readonly ILogger _logger;

        public ShaderFactory(ILogger logger)
        {
            _logger = logger.ForContext<ShaderFactory>();
        }

        public ShaderProgram CreateFromFile(
            string name,
            string vertexShaderFilePath,
            string fragmentShaderFilePath,
            out VertexType allowedVertexType)
        {
            var vertexShaderSource = File.ReadAllText(vertexShaderFilePath);
            var fragmentShaderSource = File.ReadAllText(fragmentShaderFilePath);

            return CreateFromString(name, vertexShaderSource, fragmentShaderSource, out allowedVertexType);
        }

        public ShaderProgram CreateFromString(
            string name,
            string vertexShaderSource,
            string fragmentShaderSource,
            out VertexType allowedVertexType)
        {
            var program = CreateProgram(
                vertexShaderSource,
                fragmentShaderSource,
                out var attributeNames,
                out var uniforms);

            if (program == -1)
            {
                allowedVertexType = VertexType.Unknown;
                return null;
            }

            allowedVertexType = InputLayoutMapper.Match(attributeNames);

            return new ShaderProgram(program, name, uniforms, allowedVertexType);
        }

        private static bool CompileShader(int shader, out string errorMessage)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var result);
            if (result == (int)All.True)
            {
                errorMessage = null;
                return true;
            }

            errorMessage = GL.GetShaderInfoLog(shader);
            return false;
        }

        private static bool CanLinkProgram(int program, out string errorMessage)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var result);
            if (result == (int)All.True)
            {
                errorMessage = null;
                return true;
            }

            errorMessage = GL.GetProgramInfoLog(program);
            return false;
        }

        private int CreateProgram(
            string vertexShaderSource,
            string fragmentShaderSource,
            out string[] attributeNames,
            out IReadOnlyDictionary<string, ShaderUniform> uniforms)
        {
            var program = GL.CreateProgram();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            if (!CompileShader(vertexShader, out var errorMessage))
            {
                _logger.Error("Unable to compile vertex shader: {@ErrorMessage}", errorMessage);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            if (!CompileShader(fragmentShader, out errorMessage))
            {
                _logger.Error("Unable to compile fragment shader: {@ErrorMessage}", errorMessage);
            }

            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            if (!CanLinkProgram(program, out errorMessage))
            {
                _logger.Error("Unable to link program: {@ErrorMessage}", errorMessage);

                attributeNames = null;
                uniforms = null;
                return -1;
            }

            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            attributeNames = ExtractAttributeNames(program);
            uniforms = ExtractUniforms(program);

            return program;
        }

        private static string[] ExtractAttributeNames(int program)
        {
            var attributeNames = new List<string>();
            GL.GetProgram(program, GetProgramParameterName.ActiveAttributes, out var attributeCount);
            for (var attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
            {
                GL.GetActiveAttrib(program, attributeIndex, 64, out _, out _, out _, out var attributeName);
                attributeNames.Add(attributeName);
            }

            return attributeNames.ToArray();
        }

        private static IReadOnlyDictionary<string, ShaderUniform> ExtractUniforms(int program)
        {
            var uniforms = new Dictionary<string, ShaderUniform>();
            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out var uniformCount);
            for (var uniformIndex = 0; uniformIndex < uniformCount; uniformIndex++)
            {
                GL.GetActiveUniform(program, uniformIndex, 64, out _, out var uniformSize, out var uniformType, out var uniformName);
                uniforms.Add(uniformName, new ShaderUniform(uniformName, uniformIndex, uniformType, uniformSize));
            }

            return uniforms;
        }
    }
}
