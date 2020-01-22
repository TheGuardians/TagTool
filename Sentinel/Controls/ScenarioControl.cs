using TagTool.Cache;
using Sentinel.Render;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Sentinel.Controls
{
    public partial class ScenarioControl : UserControl
    {
        public GameCache Cache { get; }
        public Scenario Definition { get; }
        public bool Initialized { get; private set; } = false;

        public Dictionary<int, RenderObject> Objects { get; set; } = new Dictionary<int, RenderObject>();

        public RenderCamera Camera { get; set; } = null;
        public Direct3D.Device Device { get; set; } = null;

        public Timer RenderTimer { get; set; }

        public ScenarioControl()
        {
            InitializeComponent();
        }

        public ScenarioControl(GameCache cache, Scenario definition) :
            this()
        {
            Cache = cache;
            Definition = definition;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Initialized)
                return;

            var presentParams = new PresentParameters
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard,
                AutoDepthStencilFormat = DepthFormat.D16,
                EnableAutoDepthStencil = true,
                PresentationInterval = PresentInterval.One
            };

            var enumerator = Manager.Adapters.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var adapterInfo = enumerator.Current as AdapterInformation;

                var d3dDeviceCaps = Manager.GetDeviceCaps(adapterInfo.Adapter, DeviceType.Hardware);

                var flags = d3dDeviceCaps.DeviceCaps.SupportsHardwareTransformAndLight ?
                    CreateFlags.HardwareVertexProcessing :
                    CreateFlags.SoftwareVertexProcessing;

                Device = new Direct3D.Device(adapterInfo.Adapter, d3dDeviceCaps.DeviceType, this, flags, presentParams);

                if (Device != null)
                    break;
            }

            Device.DeviceReset += new EventHandler(OnResetDevice);
            OnResetDevice(Device, null);

            // TODO: load instances here...

            if (Camera != null)
                Camera.Dispose();

            Camera = new RenderCamera(this)
            {
                Speed = 0.005f,
                Position = new Vector3(),
                VerticalRadians = 6.161014f,
                HorizontalRadians = 3.14159f
            };

            Camera.ComputePosition();

            RenderTimer = new Timer
            {
                Interval = 10
            };

            RenderTimer.Tick += OnRender;
            RenderTimer.Start();

            Initialized = true;
        }

        private void OnRender(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            if (!CanFocus)
            {
                RenderTimer.Stop();
                Camera.Device.Dispose();
                Camera.Device = null;
                Device.Dispose();
                Device = null;
            }

            if (!RenderTimer.Enabled || Device == null)
                return;

            Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, BackColor, 1.0f, 0);
            Device.BeginScene();

            Camera.Move();

            Device.Transform.World = Matrix.Identity;
            Device.Transform.View = Matrix.LookAtRH(Camera.Position, Camera.Target, Camera.UpVector);
            Device.Transform.Projection = Matrix.PerspectiveFovRH(0.7f, ((float)Width / (float)Height), 0.01f, 1000.0f);

            //Instance.Render();

            Device.EndScene();
            Device.Present();
        }

        private void OnResetDevice(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            var device = (Direct3D.Device)sender;

            device.RenderState.CullMode = Cull.None;
            device.RenderState.Lighting = false;
            device.RenderState.ZBufferEnable = true;
            device.RenderState.FillMode = FillMode.Solid;

            if (device.DeviceCaps.SourceBlendCaps.SupportsInverseSourceAlpha && device.DeviceCaps.SourceBlendCaps.SupportsSourceAlpha)
            {
                device.RenderState.SourceBlend = Blend.SourceAlpha;
                device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            }

            if (device.DeviceCaps.TextureFilterCaps.SupportsMinifyLinear)
            {
                device.SamplerState[0].MinFilter = TextureFilter.Linear;
                device.SamplerState[1].MinFilter = TextureFilter.Linear;
                device.SamplerState[2].MinFilter = TextureFilter.Linear;
                device.SamplerState[3].MinFilter = TextureFilter.Linear;
            }

            if (device.DeviceCaps.TextureFilterCaps.SupportsMagnifyLinear)
            {
                device.SamplerState[0].MagFilter = TextureFilter.Linear;
                device.SamplerState[1].MagFilter = TextureFilter.Linear;
                device.SamplerState[2].MagFilter = TextureFilter.Linear;
                device.SamplerState[3].MagFilter = TextureFilter.Linear;
            }

            if (device.DeviceCaps.TextureFilterCaps.SupportsMipMapLinear)
            {
                device.SamplerState[0].MipFilter = TextureFilter.Linear;
                device.SamplerState[1].MipFilter = TextureFilter.Linear;
                device.SamplerState[2].MipFilter = TextureFilter.Linear;
                device.SamplerState[3].MipFilter = TextureFilter.Linear;
            }

            device.RenderState.Ambient = Color.DarkGray;
        }

        private void ScenarioControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Initialized || !RenderTimer.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                Camera.OldX = e.X;
                Camera.OldY = e.Y;
            }
        }

        private void ScenarioControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Initialized || !RenderTimer.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
                Camera.Update(e.X, e.Y);
        }
    }
}