
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	[System.Serializable]
	public class UVTexture
	{
		[Tooltip("Select the texture that will use the defined footstep material.")]
		public Texture texture;

		[Tooltip("Define the minimum UV texture coordinate that'll be allowed.\n" +
			"UV texture coordinates start with X=0, Y=0 in the lower left corner and end with X=1, Y=1 in the upper right corner.")]
		public Vector2 minCoord = Vector2.zero;

		[Tooltip("Define the maximum UV texture coordinate that'll be allowed.\n" +
			"UV texture coordinates start with X=0, Y=0 in the lower left corner and end with X=1, Y=1 in the upper right corner.")]
		public Vector2 maxCoord = Vector2.one;

		public UVTexture()
		{

		}

		public virtual bool Contains(Texture texture, Vector2 textureCoord)
		{
			return this.texture == texture &&
				this.minCoord.x <= textureCoord.x &&
				this.minCoord.y <= textureCoord.y &&
				this.maxCoord.x >= textureCoord.x &&
				this.maxCoord.y >= textureCoord.y;
		}
	}
}