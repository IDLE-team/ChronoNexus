
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	[AddComponentMenu("Footstepper/Terrain Footstep Source")]
	public class TerrainFootstepSource : FootstepSource
	{
		[Tooltip("The terrain that will be used.")]
		public Terrain terrain;

		[Tooltip("The texture materials define the footstep effects (audio clips and prefabs) that are linked to the terrain's textures.\n" +
			"The main texture used at a position is used to find the correct effect.")]
		public List<FootstepTextureMaterial> textureMaterials = new List<FootstepTextureMaterial>();

		protected virtual void Reset()
		{
			this.terrain = this.GetComponent<Terrain>();
		}

		/// <summary>
		/// Returns the footstep effect for a provided position and effect tag.
		/// Checks for the main texture of the terrain at the position.
		/// </summary>
		/// <param name="result">Not used.</param>
		/// <param name="position">The position to check for.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found footstep effect.</returns>
		public override FootstepEffect GetFootstepAt(RaycastResult result, Vector3 position, string effectTag)
		{
			if(this.terrain != null)
			{
				Texture texture = this.GetTextureAt(position);

				if(this.textureMaterials.Count > 0 &&
					texture != null)
				{
					for(int i = 0; i < this.textureMaterials.Count; i++)
					{
						FootstepEffect effect = this.textureMaterials[i].GetEffect(texture, result.textureCoord, effectTag);
						if(effect != null)
						{
							return effect;
						}
					}
				}

				if(FootstepManager.Instance != null)
				{
					return FootstepManager.Instance.GetFootstepFor(texture, result.textureCoord, effectTag);
				}
			}
			return null;
		}

		protected virtual Texture GetTextureAt(Vector3 position)
		{
			int xCoord = (int)(((position.x - this.terrain.transform.position.x) / this.terrain.terrainData.size.x) *
				this.terrain.terrainData.alphamapWidth);
			int zCoord = (int)(((position.z - this.terrain.transform.position.z) / this.terrain.terrainData.size.z) *
				this.terrain.terrainData.alphamapHeight);

			int index = 0;
			float comp = 0;
			float[,,] splatmapData = this.terrain.terrainData.GetAlphamaps(xCoord, zCoord, 1, 1);
			for(int i = 0; i < splatmapData.Length; i++)
			{
				if(comp < splatmapData[0, 0, i])
				{
					index = i;
					comp = splatmapData[0, 0, i];
				}
			}

#if UNITY_2018_3_OR_NEWER
			return this.terrain.terrainData.terrainLayers[index].diffuseTexture;
#else
			return this.terrain.terrainData.splatPrototypes[index].texture;
#endif
		}


		/*
		============================================================================
		Gizmo functions
		============================================================================
		*/
		protected virtual void OnDrawGizmos()
		{
			Gizmos.DrawIcon(this.transform.position, "/GamingIsLove/Footsteps/TerrainFootstepSource Icon.png");
		}
	}
}
