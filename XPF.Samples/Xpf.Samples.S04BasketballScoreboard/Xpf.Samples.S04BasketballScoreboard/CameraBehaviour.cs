namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;

    public abstract class CameraBehaviour : ICamera
    {
        private readonly ICamera camera;

        protected CameraBehaviour(string name)
        {
            this.camera = new Camera(name);
            this.Name = name;
        }

        public float AspectRatio
        {
            get
            {
                return this.camera.AspectRatio;
            }
        }

        public Vector3 AxisX
        {
            get
            {
                return this.camera.AxisX;
            }
        }

        public Vector3 AxisY
        {
            get
            {
                return this.camera.AxisY;
            }
        }

        public Vector3 AxisZ
        {
            get
            {
                return this.camera.AxisZ;
            }
        }

        public virtual BoundingBox Bounds
        {
            get
            {
                return this.camera.Bounds;
            }

            set
            {
                this.camera.Bounds = value;
            }
        }

        public float CurrentHeading
        {
            get
            {
                return this.camera.CurrentHeading;
            }
        }

        public float CurrentPitch
        {
            get
            {
                return this.camera.CurrentPitch;
            }
        }

        public float FarClip
        {
            get
            {
                return this.camera.FarClip;
            }
        }

        public float FieldOfView
        {
            get
            {
                return this.camera.FieldOfView;
            }
        }

        public bool IsActive { get; set; }

        public bool IsCollisionEnabled
        {
            get
            {
                return this.camera.IsCollisionEnabled;
            }

            set
            {
                this.camera.IsCollisionEnabled = value;
            }
        }

        public bool IsForwardsFixed
        {
            get
            {
                return this.camera.IsForwardsFixed;
            }

            set
            {
                this.camera.IsForwardsFixed = value;
            }
        }

        public bool IsOnGround
        {
            get
            {
                return this.camera.IsOnGround;
            }
        }

        public float MaxPitch
        {
            get
            {
                return this.camera.MaxPitch;
            }

            set
            {
                this.camera.MaxPitch = value;
            }
        }

        public float MinPitch
        {
            get
            {
                return this.camera.MinPitch;
            }

            set
            {
                this.camera.MinPitch = value;
            }
        }

        public string Name
        {
            get
            {
                return this.camera.Name;
            }

            set
            {
                this.camera.Name = value;
            }
        }

        public float NearClip
        {
            get
            {
                return this.camera.NearClip;
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return this.camera.Orientation;
            }

            set
            {
                this.camera.Orientation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.camera.Position;
            }

            set
            {
                this.camera.Position = value;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return this.camera.ProjectionMatrix;
            }
        }

        public Vector3 ViewDirection
        {
            get
            {
                return this.camera.ViewDirection;
            }
        }

        public BoundingFrustum ViewFrustum
        {
            get
            {
                return this.camera.ViewFrustum;
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return this.camera.ViewMatrix;
            }
        }

        public Matrix ViewProjectionMatrix
        {
            get
            {
                return this.camera.ViewProjectionMatrix;
            }
        }

        public int ZoomLevel
        {
            get
            {
                return this.camera.ZoomLevel;
            }

            set
            {
                this.camera.ZoomLevel = value;
            }
        }

        protected ICamera Camera
        {
            get
            {
                return this.camera;
            }
        }

        public void LookAt(Vector3 target)
        {
            this.camera.LookAt(target);
        }

        public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            this.camera.LookAt(eye, target, up);
        }

        public void Move(Vector3 vector)
        {
            this.camera.Move(vector);
        }

        public void Move(float deltaX, float deltaY, float deltaZ)
        {
            this.camera.Move(deltaX, deltaY, deltaZ);
        }

        public void Perspective(float fieldOfView, float aspectRatio, float nearClip, float farClip)
        {
            this.camera.Perspective(fieldOfView, aspectRatio, nearClip, farClip);
        }

        /// <summary>
        ///     Rotates the camera. Positive angles specify counter clockwise
        ///     rotations when looking down the axis of rotation towards the
        ///     origin.
        /// </summary>
        /// <param name = "yaw">
        ///     Y axis rotation in degrees.
        /// </param>
        /// <param name = "pitch">
        ///     X axis rotation in degrees.
        /// </param>
        /// <param name = "roll">
        ///     Z axis rotation in degrees.
        /// </param>
        public void Rotate(float yaw, float pitch, float roll)
        {
            this.camera.Rotate(yaw, pitch, roll);
        }
    }
}