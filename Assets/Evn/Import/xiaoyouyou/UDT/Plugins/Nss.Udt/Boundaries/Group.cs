using System.Collections.Generic;
using UnityEngine;

namespace Nss.Udt.Boundaries {
    public class Group : MonoBehaviour {

        public List<Segment> segments = new List<Segment>();
        public float height;

        public DepthAnchorTypes depthAnchor;
        public float depth;

        public bool isClosed;
        public Color color;

        public LayerMask layer;

        [SerializeField]
        private bool forceUpdate = false;

        List<Mesh> _segmentMeshes = new List<Mesh>();
        Material _material;
        Mesh _mesh;

        public Vector3 Centroid {
            get {
                if (segments.Count > 0) {
                    var sum = Vector3.zero;
					
					for(int i=0; i < segments.Count; i++) {
						sum += segments[i].Midpoint;
					}

                    return sum / segments.Count;
                }
                else {
                    return Vector3.zero;
                }
            }
        }

        public void Connect() {
            for (int i = 0; i < segments.Count - 1; i++) {
                segments[i].end = segments[i + 1].start;
            }

            if (isClosed && segments.Count > 2) {
                segments[segments.Count - 1].end = segments[0].start;
            }
        }

        public void Translate(Vector3 delta) {
			for(int i=0; i < segments.Count; i++) {
				segments[i].start += delta;
				segments[i].end += delta;
			}
        }
		
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Mesh mesh;

            if (null == _segmentMeshes)
            {
                _segmentMeshes = new List<Mesh>();
            }
            else
            {
                _segmentMeshes.Clear();
            }

            if (null == _material)
            {
                _material = new Material(Shader.Find("Custom/TranspUnlit"))
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

            if (null == _mesh)
            {
                _mesh = new Mesh()
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }


            _material.color = color;
            
			for(int i=0; i < segments.Count; i++) {
				_segmentMeshes.Add(
					segments[i].GetMesh(height, depth, depthAnchor)
				);
			}

            if (segments.Count == 1) {
                _mesh = _segmentMeshes[0];
            }
            else {
                CombineInstance[] combine = new CombineInstance[_segmentMeshes.Count];

                for (int i = 0; i < _segmentMeshes.Count; i++)
                {
                    combine[i].mesh = _segmentMeshes[i];
                }



                _mesh.CombineMeshes(combine, true, false);
                _segmentMeshes.ForEach(sm => GameObject.DestroyImmediate(sm));
            }

            for (int i = 0; i < _material.passCount; i++) {
                if (_material.SetPass(i)) {
                    Graphics.DrawMeshNow(_mesh, Matrix4x4.identity);
                }
            }
        }
#endif

        private void Awake() {
            GenerateColliders();
        }

        private void FixedUpdate() {
            if (Application.isEditor && forceUpdate) {
                GenerateColliders();
            }
        }

        private void GenerateColliders() {

            if (forceUpdate) {
                var children = GetComponentsInChildren<BoxCollider>();

                for (int i = 0; i < children.Length; i++) {
                    GameObject.DestroyImmediate(children[i].gameObject);
                    continue;
                }
            }
			
			for(int i=0; i < segments.Count; i++) {
				GameObject seg = new GameObject(string.Format("{0}-{1}", name, segments[i].name));
                seg.transform.parent = gameObject.transform;
                seg.layer = layer;

                var bc = seg.AddComponent<BoxCollider>();
                bc.size = segments[i].GetSize(height, depth);
                bc.center = segments[i].GetCenter(height, depth, depthAnchor);

                Quaternion rot = segments[i].GetYAxisRotation();
                if (rot != Quaternion.identity) {
                    seg.transform.rotation = rot;
                }

                seg.transform.Rotate(0f, 90f, 0f);
                seg.transform.position = segments[i].Midpoint;
			}
        }
    }
}