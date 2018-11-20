using System;
using UnityEngine;

namespace Nss.Udt.Boundaries {
    [Serializable]
    public class Segment {
        public string name;

        public Vector3 start;
        public Vector3 end;

        public Vector3 Midpoint {
            get {
                return Vector3.Lerp(start, end, 0.5f);
            }
        }

        public Vector3 GetSize(float height, float depth) {
            return new Vector3(
                Vector3.Distance(start, end),
                height,
                depth
            );
        }

        public Vector3 GetCenter(float height, float depth, DepthAnchorTypes depthAnchor) {
            var center = new Vector3();

            switch (depthAnchor) {
                case DepthAnchorTypes.Middle:
                    center = new Vector3(0.0f, height / 2.0f, 0.0f);
                    break;

                case DepthAnchorTypes.Left:
                    center = new Vector3(0.0f, height / 2.0f, depth / 2.0f);
                    break;

                case DepthAnchorTypes.Right:
                    center = new Vector3(0.0f, height / 2.0f, depth / 2.0f * -1);
                    break;
            }

            return center;
        }

        /// <summary>
        /// Gets the Y axis rotation.
        /// </summary>
        /// <returns>The Y axis rotation</returns>
        public Quaternion GetYAxisRotation() {
            var dir = (start - end).normalized;

            return (dir == Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(dir);
        }

#if UNITY_EDITOR
        public Mesh GetMesh(float height, float depth, DepthAnchorTypes depthAnchor) {
            Vector3 s1 = new Vector3();
            Vector3 s2 = new Vector3();
            Vector3 e1 = new Vector3();
            Vector3 e2 = new Vector3();

            var p1 = new Vector3();
            var p2 = new Vector3();
            var p3 = new Vector3();
            var p4 = new Vector3();
            var p5 = new Vector3();
            var p6 = new Vector3();
            var p7 = new Vector3();
            var p8 = new Vector3();

            if (depth == 0.0f) {
                p1 = start;
                p2 = start + Vector3.up * height;
                p3 = end + Vector3.up * height;
                p4 = end;

                p5 = start;
                p6 = start + Vector3.up * height;
                p7 = end + Vector3.up * height;
                p8 = end;
            }
            else {
                switch (depthAnchor) {
                    case DepthAnchorTypes.Middle:
                        s1 = start;
                        s2 = start;

                        s1.x -= depth / 2.0f;
                        s2.x += depth / 2.0f;

                        e1 = end;
                        e2 = end;

                        e1.x -= depth / 2.0f;
                        e2.x += depth / 2.0f;

                        break;

                    case DepthAnchorTypes.Left:

                        s1 = start;
                        s2 = start;

                        s1.x += depth;

                        e1 = end;
                        e2 = end;

                        e1.x += depth;

                        break;

                    case DepthAnchorTypes.Right:

                        s1 = start;
                        s2 = start;

                        s1.x -= depth;

                        e1 = end;
                        e2 = end;

                        e1.x -= depth;

                        break;
                }

                // rotate wireframe box
                s1 = RotateAroundPoint(s1, start, GetYAxisRotation());
                s2 = RotateAroundPoint(s2, start, GetYAxisRotation());
                e1 = RotateAroundPoint(e1, end, GetYAxisRotation());
                e2 = RotateAroundPoint(e2, end, GetYAxisRotation());

                p1 = s1;
                p2 = s1 + Vector3.up * height;
                p3 = e1 + Vector3.up * height;
                p4 = e1;

                p5 = s2;
                p6 = s2 + Vector3.up * height;
                p7 = e2 + Vector3.up * height;
                p8 = e2;
            }

            Mesh mesh = new Mesh {
                hideFlags = HideFlags.HideAndDontSave
            };

            mesh.vertices = new[] { p1, p2, p3, p4, p5, p6, p7, p8 };

            mesh.triangles = new[]{
                0,2,1,1,2,0,
                0,2,3,3,2,0,
                4,6,5,5,6,4,
                4,6,7,7,6,4,
                0,5,1,1,5,0,
                0,5,4,4,5,0,
                1,6,5,5,6,1,
                1,6,2,2,6,1,
                0,7,4,4,7,0,
                0,7,3,3,7,0,
                2,7,6,6,7,2,
                2,7,3,3,7,2
            };

            Vector3[] vertices = mesh.vertices;
            Vector2[] uvs = new Vector2[vertices.Length];
            int j = 0;
            while (j < uvs.Length) {
                uvs[j] = new Vector2(vertices[j].x, vertices[j].z);
                j++;
            }

            mesh.uv = uvs;
            mesh.RecalculateNormals();

            return mesh;
        }
#endif

        /// <summary>
        /// Rotates the around point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="pivot">The pivot.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns></returns>
        private static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion rotation) {
            return rotation * (point - pivot) + pivot;
        }
    }
}
