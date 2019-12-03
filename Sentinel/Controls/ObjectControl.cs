using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using D3DDevice = Microsoft.DirectX.Direct3D.Device;
using Sentinel.Render;
using Microsoft.DirectX;

namespace Sentinel.Controls
{
    public partial class ObjectControl : UserControl
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public GameObject Definition { get; }
        public bool Initialized { get; private set; } = false;

        public RenderObject Instance { get; set; } = null;

        public RenderCamera Camera { get; set; } = null;
        public D3DDevice Device { get; set; } = null;

        public Timer RenderTimer { get; set; }

        public ObjectControl()
        {
            InitializeComponent();
        }

        public ObjectControl(HaloOnlineCacheContext cacheContext, GameObject definition) :
            this()
        {
            CacheContext = cacheContext;
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

                Device = new D3DDevice(adapterInfo.Adapter, d3dDeviceCaps.DeviceType, this, flags, presentParams);

                if (Device != null)
                    break;
            }

            Device.DeviceReset += new EventHandler(OnResetDevice);
            OnResetDevice(Device, null);

            Instance = new RenderObject(Device, CacheContext, Definition, new TagTool.Common.RealPoint3d(), new TagTool.Common.RealEulerAngles3d());

            if (Camera != null)
                Camera.Dispose();

            var compression = Instance.RenderModel.Geometry.Compression.Count > 0 ?
                Instance.RenderModel.Geometry.Compression[0] :
                new TagTool.Geometry.RenderGeometryCompression
                {
                    X = new TagTool.Common.Bounds<float>(float.MinValue, float.MaxValue),
                    Y = new TagTool.Common.Bounds<float>(float.MinValue, float.MaxValue),
                    Z = new TagTool.Common.Bounds<float>(float.MinValue, float.MaxValue),
                    U = new TagTool.Common.Bounds<float>(float.MinValue, float.MaxValue),
                    V = new TagTool.Common.Bounds<float>(float.MinValue, float.MaxValue)
                };

            Camera = new RenderCamera(this)
            {
                Speed = 0.005f,
                Position = new Vector3(compression.X.Length + (Instance.Object.BoundingRadius * 2f), 0f, Instance.RenderModel.Nodes.FirstOrDefault().DefaultTranslation.Z),
                VerticalRadians = 6.161014f,
                HorizontalRadians = 3.14159f
            };

            Camera.ComputePosition();

            RenderTimer = new Timer
            {
                Interval = 10
            };

            RenderTimer.Tick += OnRender;

            Initialized = true;

            RenderTimer.Start();
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

            Instance.Render();

            Device.EndScene();
            Device.Present();
        }

        public void OnResetDevice(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            var device = (D3DDevice)sender;

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

        private void ObjectControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Initialized || !RenderTimer.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                Camera.OldX = e.X;
                Camera.OldY = e.Y;
            }
        }

        private void ObjectControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Initialized || !RenderTimer.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
                Camera.Update(e.X, e.Y);
        }
    }
}