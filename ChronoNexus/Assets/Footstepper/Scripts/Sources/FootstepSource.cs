
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	public abstract class FootstepSource : MonoBehaviour
	{
		/// <summary>
		/// Returns the footstep effect for a provided position and effect tag.
		/// </summary>
		/// <param name="result">The result of a raycast to find the footstep effect (null if no raycast was used).</param>
		/// <param name="position">The position to check for.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found footstep effect.</returns>
		public abstract FootstepEffect GetFootstepAt(RaycastResult result, Vector3 position, string effectTag);
	}
}
