using UnityEngine;

public class EffectLib : MonoBehaviour
{
		private static GameObject cubePrimitive;

		private static GameObject CubePrimitive
		{
				get
				{
						if (cubePrimitive == null)
						{
								cubePrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
								cubePrimitive.SetActive(false);
						}
						return cubePrimitive;
				}
		}

		public static void Explode(Transform target, float? destroySeconds = null)
		{
				// remove collider from original
				if (target.GetComponent<Collider>() != null)
						target.GetComponent<Collider>().enabled = false;

				target.gameObject.SetActive(false);

				var tRender = target.GetComponent<Renderer>();
				Explosion(target.position, target.GetComponent<MeshFilter>().mesh, tRender.material, destroySeconds);
		}

		public static void Explosion(Vector3 position, Vector3 size, Material material, float? destroySeconds = null)
		{
				Explosion(position, size, CubePrimitive.GetComponent<MeshFilter>().mesh, material, destroySeconds);
		}
		public static void Explosion(Vector3 position, float? destroySeconds = null)
		{
				Explosion(position, Vector3.one, CubePrimitive.GetComponent<Renderer>().material, destroySeconds);
		}
		public static void Explosion(Vector3 position, Vector3 size, float? destroySeconds = null)
		{
				Explosion(position, size, CubePrimitive.GetComponent<MeshFilter>().sharedMesh, cubePrimitive.GetComponent<Renderer>().sharedMaterial, destroySeconds);
		}

		public static void Explosion(Vector3 position, Mesh mesh, Material material, float? destroySeconds = null)
		{
				Explosion(position, mesh.bounds.size, mesh, material, destroySeconds);
		}
		public static void Explosion(Vector3 position, Vector3 size, Mesh mesh, Material material, float? destroySeconds = null)
		{
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				int[] triangles = mesh.triangles;
				Vector2[] uvs = mesh.uv;
				int index = 0;

				// Used to make independency on the material (creates a copy)
				var materialClone = Instantiate(material);

				float extrudeSize = ((size.x + size.y + size.z) / 3) * 0.3f;

				// get each face
				for (int i = 0; i < triangles.Length; i += 3)
				{
						// TODO: inherit speed, spin...?
						Vector3 averageNormal = (normals[triangles[i]] + normals[triangles[i + 1]] + normals[triangles[i + 2]]).normalized;

						CreateMeshPiece(extrudeSize, position, materialClone, index, averageNormal,
								vertices[triangles[i]].MultiplyBy(size), vertices[triangles[i + 1]].MultiplyBy(size), vertices[triangles[i + 2]].MultiplyBy(size),
								uvs[triangles[i]], uvs[triangles[i + 1]], uvs[triangles[i + 2]],
								destroySeconds);
						index++;
				}
		}

		private static void CreateMeshPiece(float extrudeSize, Vector3 pos, Material mat, int index,
				Vector3 faceNormal, Vector3 v1, Vector3 v2, Vector3 v3, Vector2 uv1, Vector2 uv2, Vector2 uv3,
				float? destroySeconds)
		{
				GameObject go = new GameObject();
				go.name = "piece_" + index;

				Mesh mesh = go.AddComponent<MeshFilter>().mesh;
				go.AddComponent<MeshRenderer>();
				go.GetComponent<Renderer>().material = mat;
				go.transform.position = pos;

				Vector3[] vertices = new Vector3[3 * 4];
				int[] triangles = new int[3 * 4];
				Vector2[] uvs = new Vector2[3 * 4];

				// get centroid
				Vector3 v4 = (v1 + v2 + v3) / 3;
				// extend to backwards
				v4 = v4 + (-faceNormal) * extrudeSize;

				// not shared vertices
				// orig face
				vertices[0] = (v1);
				vertices[1] = (v2);
				vertices[2] = (v3);

				// right face
				vertices[3] = (v1);
				vertices[4] = (v2);
				vertices[5] = (v4);

				// left face
				vertices[6] = (v1);
				vertices[7] = (v3);
				vertices[8] = (v4);

				// bottom face
				vertices[9] = (v2);
				vertices[10] = (v3);
				vertices[11] = (v4);

				// orig face
				triangles[0] = 0;
				triangles[1] = 1;
				triangles[2] = 2;
				//  right face
				triangles[3] = 5;
				triangles[4] = 4;
				triangles[5] = 3;
				//  left face
				triangles[6] = 6;
				triangles[7] = 7;
				triangles[8] = 8;
				//  bottom face
				triangles[9] = 11;
				triangles[10] = 10;
				triangles[11] = 9;

				// orig face
				uvs[0] = uv1;
				uvs[1] = uv2;
				uvs[2] = uv3; // todo
																		// right face
				uvs[3] = uv1;
				uvs[4] = uv2;
				uvs[5] = uv3; // todo

				// left face
				uvs[6] = uv1;
				uvs[7] = uv3;
				uvs[8] = uv3;   // todo
																				// bottom face (mirror?) or custom color? or fixed from uv?
				uvs[9] = uv1;
				uvs[10] = uv2;
				uvs[11] = uv1; // todo

				mesh.vertices = vertices;
				mesh.uv = uvs;
				mesh.triangles = triangles;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();

				CalculateMeshTangents(mesh);

				go.AddComponent<Rigidbody>();
				MeshCollider mc = go.AddComponent<MeshCollider>();

				if (destroySeconds != null)
				{
						Destroy(go, destroySeconds.Value);
				}

				mc.sharedMesh = mesh;
				mc.convex = true;
		}

		// source: http://answers.unity3d.com/questions/7789/calculating-tangents-vector4.html
		private static void CalculateMeshTangents(Mesh mesh)
		{
				//speed up math by copying the mesh arrays
				int[] triangles = mesh.triangles;
				Vector3[] vertices = mesh.vertices;
				Vector2[] uv = mesh.uv;
				Vector3[] normals = mesh.normals;

				//variable definitions
				int triangleCount = triangles.Length;
				int vertexCount = vertices.Length;

				Vector3[] tan1 = new Vector3[vertexCount];
				Vector3[] tan2 = new Vector3[vertexCount];

				Vector4[] tangents = new Vector4[vertexCount];

				for (long a = 0; a < triangleCount; a += 3)
				{
						long i1 = triangles[a + 0];
						long i2 = triangles[a + 1];
						long i3 = triangles[a + 2];

						Vector3 v1 = vertices[i1];
						Vector3 v2 = vertices[i2];
						Vector3 v3 = vertices[i3];

						Vector2 w1 = uv[i1];
						Vector2 w2 = uv[i2];
						Vector2 w3 = uv[i3];

						float x1 = v2.x - v1.x;
						float x2 = v3.x - v1.x;
						float y1 = v2.y - v1.y;
						float y2 = v3.y - v1.y;
						float z1 = v2.z - v1.z;
						float z2 = v3.z - v1.z;

						float s1 = w2.x - w1.x;
						float s2 = w3.x - w1.x;
						float t1 = w2.y - w1.y;
						float t2 = w3.y - w1.y;

						float r = 1.0f / (s1 * t2 - s2 * t1);

						Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
						Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

						tan1[i1] += sdir;
						tan1[i2] += sdir;
						tan1[i3] += sdir;

						tan2[i1] += tdir;
						tan2[i2] += tdir;
						tan2[i3] += tdir;
				}

				for (int a = 0; a < vertexCount; ++a)
				{
						Vector3 n = normals[a];
						Vector3 t = tan1[a];
						Vector3.OrthoNormalize(ref n, ref t);
						tangents[a].x = t.x;
						tangents[a].y = t.y;
						tangents[a].z = t.z;
						tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
				}
				mesh.tangents = tangents;
		}
}