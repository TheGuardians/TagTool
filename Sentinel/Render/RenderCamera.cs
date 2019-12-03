using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Windows.Forms;
using Direct3D = Microsoft.DirectX.Direct3D;
using DirectInput = Microsoft.DirectX.DirectInput;

namespace Sentinel.Render
{
    public class RenderCamera : IDisposable
    {
        public DirectInput.Device Device;
        private Plane[] Frustum = new Plane[6];

        public Vector3 Position = new Vector3(0f, 0f, 0f);
        public Vector3 Target = new Vector3(0, 0, 0f);
        public Vector3 UpVector = new Vector3(0, 0, 1);
        public float Radius = 1.0f;
        public float Speed = 0.5f;

        public float HorizontalRadians;
        public float VerticalRadians;
        public bool FixedRotation = false;
        public int OldX = 0;
        public int OldY = 0;

        public RenderCamera(Control control)
        {
            Device = new DirectInput.Device(SystemGuid.Keyboard);
            Device.SetCooperativeLevel(control.FindForm().MdiParent, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);

            HorizontalRadians = 0.0f;
            VerticalRadians = 0.0f;
            Position = new Vector3(0f, 0f, 0f);
            ComputePosition();
        }

        public void Move()
        {
            try
            {
                Device.Acquire();
            }
            catch
            {
                return;
            }

            foreach (Key kk in Device.GetPressedKeys())
            {
                switch (kk.ToString())
                {
                    case "W":
                        Position.X += (Target.X - Position.X) * Speed;
                        Position.Y += (Target.Y - Position.Y) * Speed;
                        Position.Z += (Target.Z - Position.Z) * Speed;
                        break;

                    case "S":
                        Position.X -= (Target.X - Position.X) * Speed;
                        Position.Y -= (Target.Y - Position.Y) * Speed;
                        Position.Z -= (Target.Z - Position.Z) * Speed;
                        break;

                    case "A":
                        Strafe(false);
                        break;

                    case "D":
                        Strafe(true);
                        break;

                    case "Z":
                        Position.Z -= Speed;
                        break;

                    case "X":
                        Position.Z += Speed;
                        break;

                    case "Equals":
                    case "Add":
                        Speed += 0.01f;
                        break;

                    case "Minus":
                    case "NumPadMinus":
                        Speed -= 0.01f;
                        if (Speed <= 0.01f)
                            Speed = 0.01f;
                        break;
                }

                ComputePosition();
            }
        }

        public void Update(int x, int y)
        {
            var tempX = OldX - x;
            var tempY = OldY - y;

            HorizontalRadians += DegreesToRadians((float)tempX);
            VerticalRadians += DegreesToRadians((float)tempY);

            ComputePosition();
            OldX = x;
            OldY = y;
        }

        public void LookAt(Vector3 position)
        {
            Target = new Vector3(position.X, position.Y, position.Z);
        }

        public void Strafe(bool right)
        {
            HorizontalRadians = HorizontalRadians > (float)Math.PI * 2 ? HorizontalRadians - (float)Math.PI * 2 : HorizontalRadians;
            HorizontalRadians = HorizontalRadians < 0 ? HorizontalRadians + (float)Math.PI * 2 : HorizontalRadians;

            VerticalRadians = VerticalRadians > (float)Math.PI * 2 ? VerticalRadians - (float)Math.PI * 2 : VerticalRadians;
            VerticalRadians = VerticalRadians < 0 ? VerticalRadians + (float)Math.PI * 2 : VerticalRadians;

            float tempi = Radius * (float)(Math.Cos(HorizontalRadians - 1.57) * Math.Cos(VerticalRadians)) * this.Speed;
            float tempj = Radius * (float)(Math.Sin(HorizontalRadians - 1.57) * Math.Cos(VerticalRadians)) * this.Speed;

            if (right == true)
            {
                Target.X += tempi;
                Target.Y += tempj;

                Position.X += tempi;
                Position.Y += tempj;
            }
            else
            {
                Target.X -= tempi;
                Target.Y -= tempj;

                Position.X -= tempi;
                Position.Y -= tempj;
            }
        }

        public void Dispose()
        {
            Device = null;
        }

        public void ComputePosition()
        {
            HorizontalRadians = HorizontalRadians > (float)Math.PI * 2 ? HorizontalRadians - (float)Math.PI * 2 : HorizontalRadians;
            HorizontalRadians = HorizontalRadians < 0 ? HorizontalRadians + (float)Math.PI * 2 : HorizontalRadians;

            VerticalRadians = VerticalRadians > (float)Math.PI * 2 ? VerticalRadians - (float)Math.PI * 2 : VerticalRadians;
            VerticalRadians = VerticalRadians < 0 ? VerticalRadians + (float)Math.PI * 2 : VerticalRadians;

            Target.X = Radius * (float)(Math.Cos(HorizontalRadians) * Math.Cos(VerticalRadians));
            Target.Y = Radius * (float)(Math.Sin(HorizontalRadians) * Math.Cos(VerticalRadians));
            Target.Z = Radius * (float)Math.Sin(VerticalRadians);

            if (FixedRotation)
            {
                Position.X = Target.X + Position.X;
                Position.Y = Target.Y + Position.Y;
                Position.Z = Target.Z + Position.Z;
            }
            else
            {
                Target.X += Position.X;
                Target.Y += Position.Y;
                Target.Z += Position.Z;
            }
        }

        public float DegreesToRadians(float degree)
        {
            return (float)(degree * (Math.PI / 180));
        }

        public void BuildViewFrustum(ref Direct3D.Device device)
        {
            Matrix viewProjection = device.Transform.View * device.Transform.Projection;

            // Left plane
            Frustum[0].A = viewProjection.M14 + viewProjection.M11;
            Frustum[0].B = viewProjection.M24 + viewProjection.M21;
            Frustum[0].C = viewProjection.M34 + viewProjection.M31;
            Frustum[0].D = viewProjection.M44 + viewProjection.M41;

            // Right plane
            Frustum[1].A = viewProjection.M14 - viewProjection.M11;
            Frustum[1].B = viewProjection.M24 - viewProjection.M21;
            Frustum[1].C = viewProjection.M34 - viewProjection.M31;
            Frustum[1].D = viewProjection.M44 - viewProjection.M41;

            // Top plane
            Frustum[2].A = viewProjection.M14 - viewProjection.M12;
            Frustum[2].B = viewProjection.M24 - viewProjection.M22;
            Frustum[2].C = viewProjection.M34 - viewProjection.M32;
            Frustum[2].D = viewProjection.M44 - viewProjection.M42;

            // Bottom plane
            Frustum[3].A = viewProjection.M14 + viewProjection.M12;
            Frustum[3].B = viewProjection.M24 + viewProjection.M22;
            Frustum[3].C = viewProjection.M34 + viewProjection.M32;
            Frustum[3].D = viewProjection.M44 + viewProjection.M42;

            // Near plane
            Frustum[4].A = viewProjection.M13;
            Frustum[4].B = viewProjection.M23;
            Frustum[4].C = viewProjection.M33;
            Frustum[4].D = viewProjection.M43;

            // Far plane
            Frustum[5].A = viewProjection.M14 - viewProjection.M13;
            Frustum[5].B = viewProjection.M24 - viewProjection.M23;
            Frustum[5].C = viewProjection.M34 - viewProjection.M33;
            Frustum[5].D = viewProjection.M44 - viewProjection.M43;

            // Normalize planes
            for (int i = 0; i < 6; i++)
                Frustum[i] = Plane.Normalize(Frustum[i]);
        }

        public bool SphereInFrustum(Vector3 position, float radius)
        {
            var position4 = new Vector4(position.X, position.Y, position.Z, 1f);

            for (int i = 0; i < 6; i++)
            {
                if (Frustum[i].Dot(position4) + radius < 0)
                    return false;
            }

            return true;
        }
    }
}