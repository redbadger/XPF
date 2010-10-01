namespace Xpf.Samples.S04BasketballScoreboard
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Quad
    {
        private readonly short[] indices;

        private readonly Vector3 left;

        private readonly Vector3 lowerLeft;

        private readonly Vector3 lowerRight;

        private readonly Vector3 normal;

        private readonly Vector3 up;

        private readonly Vector3 upperLeft;

        private readonly Vector3 upperRight;

        private readonly VertexPositionNormalTexture[] vertices;

        private Vector3 origin;

        public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            this.vertices = new VertexPositionNormalTexture[4];
            this.indices = new short[6];
            this.origin = origin;
            this.normal = normal;
            this.up = up;

            // Calculate the quad corners
            this.left = Vector3.Cross(normal, this.up);
            Vector3 center = (this.up * height / 2) + origin;
            this.upperLeft = center + (this.left * width / 2);
            this.upperRight = center - (this.left * width / 2);
            this.lowerLeft = this.upperLeft - (this.up * height);
            this.lowerRight = this.upperRight - (this.up * height);

            this.FillVertices();
        }

        public short[] Indices
        {
            get
            {
                return this.indices;
            }
        }

        public VertexPositionNormalTexture[] Vertices
        {
            get
            {
                return this.vertices;
            }
        }

        private void FillVertices()
        {
            // Fill in texture coordinates to display full texture
            // on quad
            var textureUpperLeft = new Vector2(0.0f, 0.0f);
            var textureUpperRight = new Vector2(1.0f, 0.0f);
            var textureLowerLeft = new Vector2(0.0f, 1.0f);
            var textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < this.Vertices.Length; i++)
            {
                this.Vertices[i].Normal = this.normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            this.Vertices[0].Position = this.lowerLeft;
            this.Vertices[0].TextureCoordinate = textureLowerLeft;
            this.Vertices[1].Position = this.upperLeft;
            this.Vertices[1].TextureCoordinate = textureUpperLeft;
            this.Vertices[2].Position = this.lowerRight;
            this.Vertices[2].TextureCoordinate = textureLowerRight;
            this.Vertices[3].Position = this.upperRight;
            this.Vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            this.Indices[0] = 0;
            this.Indices[1] = 1;
            this.Indices[2] = 2;
            this.Indices[3] = 2;
            this.Indices[4] = 1;
            this.Indices[5] = 3;
        }
    }
}