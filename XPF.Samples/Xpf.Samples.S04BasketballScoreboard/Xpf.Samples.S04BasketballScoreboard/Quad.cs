#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

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
