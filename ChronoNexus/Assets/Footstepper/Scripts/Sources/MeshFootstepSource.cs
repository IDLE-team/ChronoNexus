
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	[AddComponentMenu("Footstepper/Mesh Footstep Source")]
	public class MeshFootstepSource : FootstepSource
	{
		[Tooltip("The mesh that will be used.\n" +
			"The mesh must be readable (read/write enabled in import settings).")]
		public Mesh mesh;

		[Tooltip("The mesh renderer that will be used.")]
		public MeshRenderer meshRenderer;

		[Tooltip("The texture materials define the footstep effects (audio clips and prefabs) that are linked to the mesh renderer's materials.\n" +
			"The material used at a position is used to find the correct effect.")]
		public List<FootstepTextureMaterial> textureMaterials = new List<FootstepTextureMaterial>();

		protected virtual void Reset()
		{
			this.mesh = MeshFootstepSource.GetMesh(this.gameObject);
			this.meshRenderer = this.GetComponent<MeshRenderer>();
		}

		/// <summary>
		/// Returns the footstep effect for a provided position and effect tag.
		/// Checks for the main texture of the terrain at the position.
		/// </summary>
		/// <param name="result">The result of a raycast to find the footstep effect (null if no raycast was used).</param>
		/// <param name="position">Not used.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found footstep effect.</returns>
		public override FootstepEffect GetFootstepAt(RaycastResult result, Vector3 position, string effectTag)
		{
			Material material = MeshFootstepSource.GetMaterial(this.mesh, this.meshRenderer, result.triangleIndex);
			if(material != null)
			{
				if(this.textureMaterials.Count > 0)
				{
					for(int i = 0; i < this.textureMaterials.Count; i++)
					{
						FootstepEffect effect = this.textureMaterials[i].GetEffect(material, result.textureCoord, effectTag);
						if(effect != null)
						{
							return effect;
						}
					}
				}

				if(FootstepManager.Instance != null)
				{
					return FootstepManager.Instance.GetFootstepFor(material, result.textureCoord, effectTag);
				}
			}
			return null;
		}


		/*
		============================================================================
		Tool functions
		============================================================================
		*/
		public static Mesh GetMesh(GameObject gameObject)
		{
			Mesh mesh = null;
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if(meshFilter != null)
			{
				mesh = meshFilter.sharedMesh;
			}
			if(mesh == null)
			{
				MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
				if(meshCollider != null)
				{
					mesh = meshCollider.sharedMesh;
				}
			}
			return mesh;
		}

		public static Material GetMaterial(Mesh mesh, Renderer renderer, int triangleIndex)
		{
			if(mesh != null &&
				mesh.isReadable &&
				renderer != null &&
				triangleIndex >= 0)
			{
				int[] hitTriangle = new int[]
				{
					mesh.triangles[triangleIndex * 3],
					mesh.triangles[triangleIndex * 3 + 1],
					mesh.triangles[triangleIndex * 3 + 2]
				};
				for(int i = 0; i < mesh.subMeshCount; i++)
				{
					int[] subMeshTris = mesh.GetTriangles(i);
					for(int j = 0; j < subMeshTris.Length; j += 3)
					{
						if(subMeshTris[j] == hitTriangle[0] &&
							subMeshTris[j + 1] == hitTriangle[1] &&
							subMeshTris[j + 2] == hitTriangle[2])
						{
							return renderer.sharedMaterials[i];
						}
					}
				}
			}
			return null;
		}


		/*
		============================================================================
		Gizmo functions
		============================================================================
		*/
		protected virtual void OnDrawGizmos()
		{
			Gizmos.DrawIcon(this.transform.position, "/GamingIsLove/Footsteps/MeshFootstepSource Icon.png");
		}
	}
}
