using UnityEngine;

namespace Nss.Udt.Zones {
    [RequireComponent(typeof(BoxCollider))]
    public class BoxZone : Zone {
		#region Properties
		/// <summary>
		/// Gets the size of the box collider
		/// </summary>
		public Vector3 Size {
            get {
                return GetComponent<BoxCollider>().size;
            }
        }
		
		/// <summary>
		/// Gets vertices that make up the wireframe
		/// </summary>
		public Vector3[] Vertices {
			get {
				var p1 = new Vector3(
					transform.position.x - (Size.x / 2f * transform.localScale.x),
					transform.position.y - (Size.y / 2f * transform.localScale.y),
					transform.position.z - (Size.z / 2f * transform.localScale.z)
				);
				
				var p2 = new Vector3(
					transform.position.x - (Size.x / 2f * transform.localScale.x),
					transform.position.y + (Size.y / 2f * transform.localScale.y),
					transform.position.z - (Size.z / 2f * transform.localScale.z)
				);
				
	            var p3 = new Vector3(
					transform.position.x + (Size.x / 2f * transform.localScale.x),
					transform.position.y + (Size.y / 2f * transform.localScale.y),
					transform.position.z - (Size.z / 2f * transform.localScale.z)
				);
	            
				var p4 = new Vector3(
					transform.position.x + (Size.x / 2f * transform.localScale.x),
					transform.position.y - (Size.y / 2f * transform.localScale.y),
					transform.position.z - (Size.z / 2f * transform.localScale.z)
				);
	            
				var p5 = new Vector3(
					transform.position.x - (Size.x / 2f * transform.localScale.x),
					transform.position.y - (Size.y / 2f * transform.localScale.y),
					transform.position.z + (Size.z / 2f * transform.localScale.z)
				);
	            
				var p6 = new Vector3(
					transform.position.x - (Size.x / 2f * transform.localScale.x),
					transform.position.y + (Size.y / 2f * transform.localScale.y),
					transform.position.z + (Size.z / 2f * transform.localScale.z)
				);
	            
				var p7 = new Vector3(
					transform.position.x + (Size.x / 2f * transform.localScale.x),
					transform.position.y + (Size.y / 2f * transform.localScale.y),
					transform.position.z + (Size.z / 2f * transform.localScale.z)
				);
	            
				var p8 = new Vector3(
					transform.position.x + (Size.x / 2f * transform.localScale.x),
					transform.position.y - (Size.y / 2f * transform.localScale.y),
					transform.position.z + (Size.z / 2f * transform.localScale.z)
				);
				
				p1 = RotateAroundPoint(p1, transform.position, transform.rotation);
				p2 = RotateAroundPoint(p2, transform.position, transform.rotation);
				p3 = RotateAroundPoint(p3, transform.position, transform.rotation);
				p4 = RotateAroundPoint(p4, transform.position, transform.rotation);
				p5 = RotateAroundPoint(p5, transform.position, transform.rotation);
				p6 = RotateAroundPoint(p6, transform.position, transform.rotation);
				p7 = RotateAroundPoint(p7, transform.position, transform.rotation);
				p8 = RotateAroundPoint(p8, transform.position, transform.rotation);
				
				return new[] { p1, p2, p3, p4, p5, p6, p7, p8 };
			}
		}
		#endregion
		
		/// <summary>
		/// Unity3D OnDrawGizmos event
		/// </summary>
		private void OnDrawGizmos() {
			DrawMeshNow ();
		}

        private Mesh mesh;
        private Material material;

		/// <summary>
		/// Draws the mesh now.
		/// </summary>
		private void DrawMeshNow() {
            if (null == mesh)
            {
                mesh = new Mesh
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

            if (null == material)
            {
                material = new Material(Shader.Find("Custom/TranspUnlit"))
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

			material.color = color;
			mesh.vertices = Vertices;
			
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
			
			for (int i = 0; i < material.passCount; i++) {
				if (material.SetPass(i)) {
					Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
				}
			}
		}
		
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