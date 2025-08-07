/*
  NoZ Game Engine

  Copyright(c) 2019 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using UnityEngine;

namespace NoZ
{
    public static partial class Extensions
    {
        public static Vector2 OrthoBoundsMin(this Camera camera)
        {
            return (Vector2)camera.transform.position - camera.OrthoExtents();
        }

        public static Vector2 OrthoBoundsMax(this Camera camera)
        {
            return (Vector2)camera.transform.position + camera.OrthoExtents();
        }

        public static Rect OrthoBounds (this Camera camera)
        {
            var extents = camera.OrthoExtents();
            return new Rect((Vector2)camera.transform.position - extents, extents * 2.0f);
        }

        public static Vector2 OrthoExtents(this Camera camera)
        {
            if (camera.orthographic)
                return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
            else
            {
                Debug.LogError("Camera is not orthographic!", camera);
                return Vector2.zero;
            }
        }
    }
}
