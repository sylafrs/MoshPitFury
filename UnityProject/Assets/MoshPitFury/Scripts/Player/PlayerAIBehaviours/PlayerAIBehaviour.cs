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

