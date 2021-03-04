using OpenTK.Graphics.OpenGL4;

namespace Aster.Client.Base
{
    public class ShaderUniform
    {
        public ShaderUniform(
            string uniformName,
            int location,
            ActiveUniformType uniformType,
            int uniformSize)
        {
            UniformName = uniformName;
            Location = location;
            UniformType = uniformType;
            UniformSize = uniformSize;
        }

        public string UniformName { get; }

        public int Location { get; }

        public ActiveUniformType UniformType { get; }

        public int UniformSize { get; }
    }
}
