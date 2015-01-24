using UnityEngine;
using System.Collections;

public abstract class Rule : MonoBehaviour
{
	protected GameManager Manager;

	public abstract string Name
	{
		get;
	}

	public Level UsedLevel;

	public bool Started
	{
		get
		{
			return this.Manager != null;
		}
	}

	public virtual void Prepare(GameManager manager) { }

	public virtual void StartGame(GameManager manager)
	{
		this.Manager = manager;
	}

	public virtual void GameOver()
	{
		this.Manager = null;
	}

	public abstract bool IsFinished
	{
		get;
	}
}
