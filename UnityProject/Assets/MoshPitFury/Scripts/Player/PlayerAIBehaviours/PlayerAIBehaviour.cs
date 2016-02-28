using UnityEngine;
using System.Collections;

public abstract class PlayerAIBehaviour : MonoBehaviour
{
	protected Player player;
	protected PlayerAI brain;

	private void Awake()
	{
		player = this.GetComponent<Player>();
		brain = this.GetComponent<PlayerAI>();
	}

	public abstract Vector3 WantedDirection { get; }

	public virtual void OnUpdate() { }
}

// 
// Don't move : random to move or not
// Suicide : chooses a fire, random to dash or not
// Survival : chooses a place, goes there, random completely
// Deathmatch : target the nearest; or flee
// Snatch the beer : takes the beer then flee
// King of the beer : tries to remove the others, stay on the center
// 
// 
// 
// 