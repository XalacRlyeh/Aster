using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Aster.Client.Base;
using Aster.Client.Extensions;
using Aster.Client.World;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Serilog;
using Num = System.Numerics;

namespace Aster.Client
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct InputBuffer
    {
        public Matrix4 ModelViewProjectionMatrix;
        public Matrix4 Padding1;
        public Matrix4 Padding2;
    }

    internal sealed class ClientGame : Game
    {
        private readonly IShaderFactory _shaderFactory;
        private readonly IChunkProvider _chunkProvider;
        private readonly Color4 _clearColor;

        private IInputLayoutFactory _inputLayoutFactory;
        private InputLayout _chunkMeshInputLayout;
        private ShaderProgram _simpleShader;
        private TextureAtlas _textureAtlas;

        private ChunkMeshFactory _chunkMeshFactory;
        private Mesh _chunkMesh;
        private int _visibleChunksHashCode;
        private IMeshFactory _meshFactory;
        private Mesh _cubeMesh;
        private InputLayout _cubeMeshLayout;
        private ConstantBuffer _chunkMeshMatrixBuffer;
        private ConstantBuffer _cubeMeshMatrixBuffer;
        private InputBuffer _chunkMeshMvp;
        private InputBuffer _cubeMeshMvp;

        private Num.Vector3 _cameraPosition;
        private Num.Vector3 _cameraPositionRaw;
        private Num.Vector2 _chunkPosition;

        public ClientGame(
            ILogger logger,
            GameSettings gameSettings,
            IShaderFactory shaderFactory,
            IChunkProvider chunkProvider)
            : base(logger, gameSettings, shaderFactory)
        {
            _shaderFactory = shaderFactory;
            _chunkProvider = chunkProvider;
            Window.Title = "Ashoka";

            _clearColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);

            _inputLayoutFactory = new InputLayoutFactory();
            _textureAtlas = new TextureAtlas(32, "Content/Textures/LandAtlas.png");
            _chunkMeshFactory = new ChunkMeshFactory(_textureAtlas);
            _meshFactory = new MeshFactory();
        }

        protected override void Load()
        {
            base.Load();

            _chunkMeshMvp = new InputBuffer
            {
                ModelViewProjectionMatrix = Camera.GetViewMatrix() *
                                            Camera.GetProjectionMatrix(Window.Size.X, Window.Size.Y)
            };
            _chunkMeshMatrixBuffer = ConstantBuffer.Create(_chunkMeshMvp);
            _cubeMeshMvp = new InputBuffer
            {
                ModelViewProjectionMatrix = Matrix4.CreateTranslation(0, 0, 0) *
                                            Camera.GetViewMatrix() *
                                            Camera.GetProjectionMatrix(Window.Size.X, Window.Size.Y)
            };
            _cubeMeshMatrixBuffer = ConstantBuffer.Create(_cubeMeshMvp);

            _chunkMeshInputLayout = _inputLayoutFactory.CreateInputLayout(VertexType.PositionColorTexture);
            _cubeMeshLayout = _inputLayoutFactory.CreateInputLayout(VertexType.PositionColor);
            _simpleShader = _shaderFactory.CreateFromFile(
                "Simple",
                "Content/Shaders/Simple.vs.glsl",
                "Content/Shaders/Simple.fs.glsl", out var allowedVertexType);
            _textureAtlas.Load();

            _cubeMesh = _meshFactory.CreateUnitCubeMesh();

            GL.Disable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Blend);
        }

        private float angle = 0.0f;

        protected override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, Window.Size.X, Window.Size.Y);
            GL.ClearColor(_clearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _chunkMeshMvp.ModelViewProjectionMatrix = Matrix4.CreateScale(1, 1, 1) *
                                                      Camera.GetViewMatrix() *
                                                      Camera.GetProjectionMatrix(Window.Size.X, Window.Size.Y);
            _cubeMeshMvp.ModelViewProjectionMatrix = Matrix4.CreateTranslation(Camera.Position) *
                                                     Matrix4.CreateScale(2, 2, 2) *
                                                     Camera.GetViewMatrix() *
                                                     Camera.GetProjectionMatrix(Window.Size.X, Window.Size.Y);

            _chunkMeshMatrixBuffer.Bind(0);
            _chunkMeshMatrixBuffer.UpdateBuffer(_chunkMeshMvp);

            _simpleShader.Use();
            GL.BindTextureUnit(0, _textureAtlas.AtlasTexture.GLTexture);

            _chunkMeshInputLayout.Use(_chunkMesh);
            _chunkMesh.Draw();

            _cubeMeshLayout.Use(_cubeMesh);
            _cubeMeshMatrixBuffer.Bind(0);
            _cubeMesh.Draw();

            if (ImGui.Begin("TestWindow"))
            {
                if (ImGui.InputFloat3("Camera Raw", ref _cameraPositionRaw))
                {
                }

                if (ImGui.InputFloat3("Camera", ref _cameraPosition))
                {
                }

                if (ImGui.InputFloat2("Chunk Position", ref _chunkPosition))
                {
                }

                if (ImGui.Button("Close"))
                {
                    Window.Close();
                }

                ImGui.End();
            }

            base.Render(e);
        }

        protected override void Unload()
        {
            _chunkMeshInputLayout.Dispose();
            _simpleShader.Dispose();
            base.Unload();
        }

        protected override void Update(FrameEventArgs e)
        {
            angle = angle + 0.0005f;
            Vector3 direction = Vector3.Zero;

            var keyboardState = Window.KeyboardState.GetSnapshot();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                direction += new Vector3(0, 1, 0);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                direction += new Vector3(0, -1, 0);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                direction += new Vector3(-1, 0, 0);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                direction += new Vector3(1, 0, 0);
            }

            Camera.Position += direction * 0.1f;

            Vector2i ChunkPositionFromCameraPosition(Vector3 cameraPosition)
            {
                var cx = (int)(cameraPosition.X / Chunk.ChunkSize / Tile.TileSize);
                var cy = (int)(cameraPosition.Y / Chunk.ChunkSize / Tile.TileSize);
                if (cameraPosition.X < 0)
                {
                    cx--;
                }

                if (cameraPosition.Y < 0)
                {
                    cy--;
                }

                return new Vector2i(cx, cy);
            }

            var chunkPosition = ChunkPositionFromCameraPosition(Camera.Position);
            var visibleChunks = CalculateVisibleChunks(chunkPosition);
            var visibleChunksHashCode = visibleChunks.GetSequenceHashCode();
            if (_visibleChunksHashCode != visibleChunksHashCode)
            {
                _chunkMesh = _chunkMeshFactory.CreateChunkMesh(visibleChunks);
                _visibleChunksHashCode = visibleChunksHashCode;
            }

            _chunkPosition = new Num.Vector2(chunkPosition.X, chunkPosition.Y);
            _cameraPositionRaw = new Num.Vector3(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
            _cameraPosition = new Num.Vector3(Camera.Position.X, Camera.Position.Y, 0.0f);

            base.Update(e);
        }

        private IReadOnlyCollection<Chunk> CalculateVisibleChunks(Vector2i chunkPosition)
        {
            var radius = 2;
            var visibleChunks = new List<Chunk>();

            for (var y = chunkPosition.Y - radius; y <= chunkPosition.Y + radius; ++y)
            {
                for (var x = chunkPosition.X - radius; x <= chunkPosition.X + radius; ++x)
                {
                    visibleChunks.Add(_chunkProvider.GetChunk(Guid.Empty, chunkPosition + new Vector2i(x, y)));
                }
            }

            return visibleChunks;
        }
    }
}
