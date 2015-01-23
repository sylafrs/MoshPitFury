using UnityEngine;
using System.Collections;

public abstract class Rule : MonoBehaviour
{
	protected GameManager Manager;

	public virtual void StartGame(GameManager manager)
	{
		this.Manager = manager;
	}

	public abstract bool IsFinished
	{
		get;
	}
}
