using UnityEngine;
using System.Collections;

public abstract class Rule : MonoBehaviour
{
	protected GameManager Manager;
	public Transform[] SpawnPoints;
	public float Duration = 10;

	public abstract string Description
	{
		get;
	}
	
	public bool Started
	{
		get
		{
			return this.Manager != null;
		}
	}

	public virtual Transform GetPlayerSpawnPoint(Player p)
	{
		return this.SpawnPoints[p.Id - 1];
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

	public virtual bool IsFinished
	{
		get { return this.Manager.RoundTimer > this.Duration || this.Manager.AlivePlayers.Count == 0; }
	}

}
