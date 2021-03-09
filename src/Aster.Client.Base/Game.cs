using System;
using Aster.Client.Base.UI;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Serilog;

namespace Aster.Client.Base
{
    public class Game : IDisposable
    {
        private readonly IShaderFactory _shaderFactory;
        private ImGuiController _imGuiController;

        protected Game(
            ILogger logger,
            GameSettings gameSettings,
            IShaderFactory shaderFactory)
        {
            _shaderFactory = shaderFactory;
            Logger = logger.ForContext<Game>();

            var gameWindowSettings = GameWindowSettings.Default;

            Window = new GameWindow(gameWindowSettings, gameSettings._nativeWindowSettings);
            Window.Load += Load;
            Window.Unload += Unload;
            Window.UpdateFrame += Update;
            Window.RenderFrame += Render;
            Window.VSync = VSyncMode.Off;

            var monitorHandle = Window.FindMonitor();
            if (Monitors.TryGetMonitorInfo(monitorHandle, out var monitorInfo))
            {
                var windowSize = monitorInfo.ClientArea.Size * 90 / 100;
                var windowLocation = monitorInfo.ClientArea.HalfSize - windowSize / 2;

                Window.Location = windowLocation;
                Window.Size = windowSize;
            }

            Camera = new Camera(CameraMode.Orthogonal, new Vector3(0, 0, 256), Window.Size.X / (float)Window.Size.Y);
        }

        protected ILogger Logger { get; }

        protected Camera Camera { get; }

        protected GameWindow Window { get; }

        public void Dispose()
        {
            Window?.Dispose();
        }

        public void Run()
        {
            Window.Run();
        }

        protected virtual void Load()
        {
            _imGuiController = new ImGuiController(_shaderFactory, Window.Size.X, Window.Size.Y);
        }

        protected virtual void Unload()
        {
            _imGuiController.Dispose();
        }

        protected virtual void Update(FrameEventArgs e)
        {
            _imGuiController.Update(Window, (float)e.Time);
        }

        protected virtual void Render(FrameEventArgs e)
        {
            _imGuiController.Render();
            Window.SwapBuffers();
        }
    }
}
