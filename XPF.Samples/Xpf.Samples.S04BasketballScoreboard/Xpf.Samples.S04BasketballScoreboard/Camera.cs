namespace Xpf.Samples.S04BasketballScoreboard
{
    using System;

    using Microsoft.Xna.Framework;

    public class Camera
    {
        private readonly object syncRoot = new object();

        private readonly BoundingFrustum viewFrustum = new BoundingFrustum(Matrix.Identity);

        private float aspectRatio;

        private float currentHeading;

        private float currentPitch;

        private float farClip;

        private float fieldOfView;

        private float maxPitch = 75;

        private float minPitch = -75;

        private float nearClip;

        private Quaternion orientation;

        private Vector3 position;

        private Matrix projectionMatrix;

        private Vector3 viewDirection;

        private Matrix viewMatrix = Matrix.Identity;

        private Matrix viewProjectionMatrix;

        private Matrix worldMatrix;

        private Vector3 worldYAxis = Vector3.Up;

        public Camera(string name)
        {
            this.Name = name;
            this.IsForwardsFixed = true;
        }

        public float AspectRatio
        {
            get
            {
                return this.aspectRatio;
            }
        }

        public Vector3 AxisX
        {
            get
            {
                return this.worldMatrix.Right;
            }
        }

        public Vector3 AxisY
        {
            get
            {
                return this.worldMatrix.Up;
            }
        }

        public Vector3 AxisZ
        {
            get
            {
                return this.worldMatrix.Backward;
            }
        }

        public BoundingBox Bounds { get; set; }

        public float CurrentHeading
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.currentHeading;
                }
            }
        }

        public float CurrentPitch
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.currentPitch;
                }
            }
        }

        public float FarClip
        {
            get
            {
                return this.farClip;
            }
        }

        public float FieldOfView
        {
            get
            {
                return this.fieldOfView;
            }
        }

        public bool IsActive { get; set; }

        /// <summary>
        ///     If false the camera's forwards movement is pitch aligned, 
        ///     otherwise it's fixed at right-angles to the world Up vector.
        /// </summary>
        public bool IsForwardsFixed { get; set; }

        public float MaxPitch
        {
            get
            {
                return this.maxPitch;
            }

            set
            {
                this.maxPitch = value;
            }
        }

        public float MinPitch
        {
            get
            {
                return this.minPitch;
            }

            set
            {
                this.minPitch = value;
            }
        }

        public string Name { get; set; }

        public float NearClip
        {
            get
            {
                return this.nearClip;
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return this.orientation;
            }

            set
            {
                this.orientation = value;
                this.UpdateViewMatrix();
            }
        }

        public Vector3 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.UpdateViewMatrix();
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return this.projectionMatrix;
            }
        }

        public Vector3 ViewDirection
        {
            get
            {
                return this.viewDirection;
            }
        }

        public BoundingFrustum ViewFrustum
        {
            get
            {
                return this.viewFrustum;
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return this.viewMatrix;
            }
        }

        public Matrix ViewProjectionMatrix
        {
            get
            {
                return this.viewProjectionMatrix;
            }
        }

        public int ZoomLevel { get; set; }

        public void LookAt(Vector3 target)
        {
            this.LookAt(this.position, target, this.worldMatrix.Up);
        }

        public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            this.viewMatrix = Matrix.CreateLookAt(eye, target, up);
            this.worldMatrix = Matrix.Invert(this.viewMatrix);

            this.position = eye;
            this.orientation = Quaternion.CreateFromRotationMatrix(this.worldMatrix);

            this.OnViewMatrixChanged();
        }

        public void Move(Vector3 vector)
        {
            this.Move(vector.X, vector.Y, vector.Z);
        }

        public void Move(float deltaX, float deltaY, float deltaZ)
        {
            Vector3 forwards = this.IsForwardsFixed
                                   ? Vector3.Normalize(Vector3.Cross(this.worldYAxis, this.worldMatrix.Right))
                                   : this.worldMatrix.Forward;

            this.position += this.worldMatrix.Right * deltaX;
            this.position += this.worldYAxis * deltaY;
            this.position += forwards * deltaZ;

            if (this.Bounds != default(BoundingBox) && this.Bounds.Contains(this.position) == ContainmentType.Disjoint)
            {
                this.position = Vector3.Clamp(this.position, this.Bounds.Min, this.Bounds.Max);
            }

            this.UpdateViewMatrix();
        }

        public void Perspective(float fieldOfView, float aspectRatio, float nearClip, float farClip)
        {
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.nearClip = nearClip;
            this.farClip = farClip;

            this.UpdateProjectionMatrix();
            this.UpdateViewProjectionMatrix();
        }

        public void Rotate(float yaw, float pitch, float roll)
        {
            lock (this.syncRoot)
            {
                float previousPitch = this.currentPitch;
                this.currentPitch = MathHelper.Clamp(this.currentPitch + pitch, this.MinPitch, this.MaxPitch);
                pitch = this.currentPitch - previousPitch;
            }

            Quaternion rotation;
            Vector3 localXAxis = this.worldMatrix.Right;

            if (yaw != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref this.worldYAxis, MathHelper.ToRadians(yaw), out rotation);
                Quaternion.Concatenate(ref this.orientation, ref rotation, out this.orientation);
            }

            if (pitch != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref localXAxis, MathHelper.ToRadians(pitch), out rotation);
                Quaternion.Concatenate(ref this.orientation, ref rotation, out this.orientation);
            }

            this.UpdateViewMatrix();
        }

        private void OnViewMatrixChanged()
        {
            this.viewDirection = this.worldMatrix.Forward;

            lock (this.syncRoot)
            {
                this.currentPitch = MathHelper.ToDegrees((float)Math.Asin(this.viewDirection.Y));
                this.currentHeading = this.viewDirection.CalculateHeading();
            }

            this.UpdateViewProjectionMatrix();
        }

        private void UpdateProjectionMatrix()
        {
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                this.fieldOfView, this.aspectRatio, this.nearClip, this.farClip);
        }

        private void UpdateViewMatrix()
        {
            Matrix newWorldMatrix = Matrix.CreateFromQuaternion(this.orientation) *
                                    Matrix.CreateTranslation(this.position);

            // This is a bit crap! But it stops unintentional roll caused by rounding errors when continually amending the same Quaternion
            this.viewMatrix = Matrix.CreateLookAt(this.position, this.position + newWorldMatrix.Forward, Vector3.Up);
            this.worldMatrix = Matrix.Invert(this.viewMatrix);
            this.orientation = Quaternion.CreateFromRotationMatrix(this.worldMatrix);

            this.OnViewMatrixChanged();
        }

        private void UpdateViewProjectionMatrix()
        {
            this.viewProjectionMatrix = this.viewMatrix * this.projectionMatrix;
            this.viewFrustum.Matrix = this.viewProjectionMatrix;
        }
    }
}