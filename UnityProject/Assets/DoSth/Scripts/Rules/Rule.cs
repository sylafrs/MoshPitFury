using UnityEngine;
using System.Collections;

public abstract class Rule 
{
	protected GameManager Manager;

	public virtual void Start(GameManager manager)
	{
		this.Manager = manager;
	}

	public abstract bool IsFinished
	{
		get;
	}
}
