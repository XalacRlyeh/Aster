namespace Aster.Client.Base
{
    public interface IShaderFactory
    {
        ShaderProgram CreateFromFile(
            string name,
            string vertexShaderFilePath,
            string fragmentShaderFilePath,
            out VertexType allowedVertexType);

        ShaderProgram CreateFromString(
            string name,
            string vertexShaderSource,
            string fragmentShaderSource,
            out VertexType vertexType);
    }
}
