
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	[System.Serializable]
	public class FootstepUVEffect
	{
		[Tooltip("Select the textures and their UV texture coordinates that will use the defined footstep material.")]
		public List<UVTexture> texture = new List<UVTexture>();

		[Tooltip("Select the materials and their UV texture coordinates that will use the defined footstep material.")]
		public List<UVMaterial> materials = new List<UVMaterial>();

		[Space(10)]
		[Tooltip("The footstep material defines the footstep effects (audio clips and prefabs) for these assets.")]
		public FootstepMaterial material;

		public FootstepUVEffect()
		{

		}

		/// <summary>
		/// Checks if a provided texture is included.
		/// </summary>
		/// <param name="texture">The texture to check for.</param>
		/// <param name="textureCoord">The UV texture coordinate to check for.</param>
		/// <returns>true if the texture is included.</returns>
		public virtual bool Contains(Texture texture, Vector2 textureCoord)
		{
			for(int i = 0; i < this.texture.Count; i++)
			{
				if(this.texture[i].Contains(texture, textureCoord))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks if a provided material is included.
		/// </summary>
		/// <param name="material">The material to check for.</param>
		/// <param name="textureCoord">The UV texture coordinate to check for.</param>
		/// <returns>true if the material is included.</returns>
		public virtual bool Contains(Material material, Vector2 textureCoord)
		{
			for(int i = 0; i < this.materials.Count; i++)
			{
				if(this.materials[i].Contains(material, textureCoord))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the footstep effect for a provided effect tag.
		/// </summary>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found footstep effect.</returns>
		public virtual FootstepEffect GetEffect(string effectTag)
		{
			if(this.material != null)
			{
				return this.material.GetEffect(effectTag);
			}
			return null;
		}
	}
}