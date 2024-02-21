
using UnityEngine;
using System.Collections.Generic;

namespace GamingIsLove.Footsteps
{
	[AddComponentMenu("Footstepper/Footstep Manager")]
	public class FootstepManager : MonoBehaviour
	{
		[Tooltip("Prefabs spawned by footstep effects will be disabled instead of destroyed to reuse them again.\n" +
			"This can improve performance.")]
		public bool usePrefabPool = true;

		[Tooltip("Prevents the footstep manager from being destroyed when changing scenes.\n" +
			"Uses 'GameObject.DontDestroyOnLoad' to prevent the game object's destruction.")]
		public bool keepAlive = false;

		[Tooltip("The manager's texture materials are used to find footstep effects based on the texture that was hit by the raycast.\n" +
			"It's used if a 'Terrain Footstep Source' didn't find an effect or if the raycast didn't hit any footstep source.")]
		public List<FootstepTextureMaterial> textureMaterials = new List<FootstepTextureMaterial>();


		// distance to player
		[Tooltip("Set a player 'Footstepper' component to allow only footsteppers within a defined distance to the player.\n" + 
			"Can be set via code to change the player in-game.")]
		[Space(20)]
		public Footstepper player;

		[Tooltip("The distance (in world units) to the player any non-player footstepper can play footsteps.")]
		public float allowedDistanceToPlayer = 50;


		// in-game
		protected static FootstepManager instance;

		protected Dictionary<GameObject, Queue<GameObject>> prefabPool = new Dictionary<GameObject, Queue<GameObject>>();

		protected virtual void Awake()
		{
			instance = this;
			if(this.keepAlive)
			{
				GameObject.DontDestroyOnLoad(this.gameObject);
			}
		}

		/// <summary>
		/// The current instance of the footstep manager.
		/// </summary>
		public static FootstepManager Instance
		{
			get { return instance; }
		}

		public static bool IsAllowed(Footstepper footstepper)
		{
			return instance == null ||
				instance.player == null ||
				instance.player == footstepper ||
				Vector3.Distance(instance.player.transform.position, footstepper.transform.position) <= instance.allowedDistanceToPlayer;
		}

		/// <summary>
		/// Get a pool of spawned prefabs for a prefab.
		/// </summary>
		/// <param name="prefab">The prefab that will be used.</param>
		/// <returns>The prefab pool.</returns>
		public Queue<GameObject> GetPool(GameObject prefab)
		{
			if(this.usePrefabPool)
			{
				Queue<GameObject> pool;
				if(!this.prefabPool.TryGetValue(prefab, out pool))
				{
					pool = new Queue<GameObject>();
					this.prefabPool.Add(prefab, pool);
				}
				return pool;
			}
			return null;
		}

		/// <summary>
		/// Returns the footstep effect for a provided texture and effect tag.
		/// </summary>
		/// <param name="texture">The texture to check for.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found foodstep effect.</returns>
		public virtual FootstepEffect GetFootstepFor(Texture texture, Vector2 textureCoord, string effectTag)
		{
			if(this.textureMaterials.Count > 0 &&
				texture != null)
			{
				for(int i = 0; i < this.textureMaterials.Count; i++)
				{
					FootstepEffect effect = this.textureMaterials[i].GetEffect(texture, textureCoord, effectTag);
					if(effect != null)
					{
						return effect;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the footstep effect for a provided sprite and effect tag.
		/// </summary>
		/// <param name="sprite">The sprite to check for.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found foodstep effect.</returns>
		public virtual FootstepEffect GetFootstepFor(Sprite sprite, string effectTag)
		{
			if(this.textureMaterials.Count > 0 &&
				sprite != null)
			{
				for(int i = 0; i < this.textureMaterials.Count; i++)
				{
					FootstepEffect effect = this.textureMaterials[i].GetEffect(sprite, effectTag);
					if(effect != null)
					{
						return effect;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the footstep effect for a provided material and effect tag.
		/// </summary>
		/// <param name="material">The material to check for.</param>
		/// <param name="effectTag">The effect tag to check for.</param>
		/// <returns>The found foodstep effect.</returns>
		public virtual FootstepEffect GetFootstepFor(Material material, Vector2 textureCoord, string effectTag)
		{
			if(this.textureMaterials.Count > 0 &&
				material != null)
			{
				for(int i = 0; i < this.textureMaterials.Count; i++)
				{
					FootstepEffect effect = this.textureMaterials[i].GetEffect(material, textureCoord, effectTag);
					if(effect != null)
					{
						return effect;
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
			Gizmos.DrawIcon(this.transform.position, "/GamingIsLove/Footsteps/FootstepManager Icon.png");
		}
	}
}
