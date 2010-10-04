namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;

    public interface ICamera
    {
        /// <summary>
        /// Property to get and set the camera's orientation.
        /// </summary>
        Quaternion Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the camera's position.
        /// </summary>
        Vector3 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get the camera's perspective projection matrix.
        /// </summary>
        Matrix ProjectionMatrix
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's viewing direction.
        /// </summary>
        Vector3 ViewDirection
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's view matrix.
        /// </summary>
        Matrix ViewMatrix
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's concatenated view-projection matrix.
        /// </summary>
        Matrix ViewProjectionMatrix
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's local x axis.
        /// </summary>
        Vector3 AxisX
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's local y axis.
        /// </summary>
        Vector3 AxisY
        {
            get;
        }

        /// <summary>
        /// Property to get the camera's local z axis.
        /// </summary>
        Vector3 AxisZ
        {
            get;
        }

        float FieldOfView
        {
            get;
        }

        float AspectRatio
        {
            get;
        }

        float NearClip
        {
            get;
        }

        float FarClip
        {
            get;
        }

        BoundingBox Bounds
        {
            get;
            set;
        }

        float CurrentHeading
        {
            get;
        }

        float CurrentPitch
        {
            get;
        }

        float MaxPitch
        {
            get;
            set;
        }

        float MinPitch
        {
            get;
            set;
        }

        /// <summary>
        /// Current zoom expressed as a percentage of the distance between MinZoom and MaxZoom.
        /// </summary>
        int ZoomLevel
        {
            get;
            set;
        }

        bool IsCollisionEnabled
        {
            get;
            set;
        }

        bool IsOnGround
        {
            get;
        }

        /// <summary>
        /// If false the camera's forwards movement is pitch aligned, 
        /// otherwise it's fixed at right-angles to the world Up vector.
        /// </summary>
        bool IsForwardsFixed
        {
            get;
            set;
        }

        BoundingFrustum ViewFrustum
        {
            get;
        }

        bool IsActive
        {
            get;
            set;
        }

        string Name { get; set; }

        /// <summary>
        /// Builds a look at style viewing matrix using the camera's current
        /// world position, and its current local y axis.
        /// </summary>
        /// <param name="target">
        /// The target position to look at.
        /// </param>
        void LookAt(Vector3 target);

        /// <summary>
        /// Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="eye">
        /// The camera position.
        /// </param>
        /// <param name="target">
        /// The target position to look at.
        /// </param>
        /// <param name="up">
        /// The up direction.
        /// </param>
        void LookAt(Vector3 eye, Vector3 target, Vector3 up);

        /// <summary>
        /// Moves the camera the specified distance in the specified direction.
        /// </summary>
        /// <param name="vector">
        /// Move in the directon of the vector by the length of the vector.
        /// </param>
        void Move(Vector3 vector);

        /// <summary>
        /// Moves the camera by deltaX world units to the left or right; deltaY
        /// world units upwards or downwards; and deltaZ world units forwards
        /// or backwards.
        /// </summary>
        /// <param name="deltaX">
        /// Distance to move left or right.
        /// </param>
        /// <param name="deltaY">
        /// Distance to move up or down.
        /// </param>
        /// <param name="deltaZ">
        /// Distance to move forwards or backwards.
        /// </param>
        void Move(float deltaX, float deltaY, float deltaZ);

        /// <summary>
        /// Builds a perspective projection matrix based on a horizontal field
        /// of view.
        /// </summary>
        /// <param name="fieldOfView">
        /// Horizontal field of view in Radians.
        /// </param>
        /// <param name="aspectRatio">
        /// The viewport's aspect ratio.
        /// </param>
        /// <param name="nearClip">
        /// The distance to the near clip plane.
        /// </param>
        /// <param name="farClip">
        /// The distance to the far clip plane.
        /// </param>
        void Perspective(float fieldOfView, float aspectRatio, float nearClip, float farClip);

        /// <summary>
        /// Rotates the camera. Positive angles specify counter clockwise
        /// rotations when looking down the axis of rotation towards the
        /// origin.
        /// </summary>
        /// <param name="yaw">
        /// Y axis rotation in degrees.
        /// </param>
        /// <param name="pitch">
        /// X axis rotation in degrees.
        /// </param>
        /// <param name="roll">
        /// Z axis rotation in degrees.
        /// </param>
        void Rotate(float yaw, float pitch, float roll);
    }
}