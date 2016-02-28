using UnityEngine;
using System.Collections;

public abstract class Rule : MonoBehaviour
{
	protected GameManager Manager;
    public float Duration = 10;
    public Sprite ruleSprite;

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

	public virtual Transform GetPlayerStartPoint(Player p)
	{
		return null;
	}

	public virtual void Prepare(GameManager manager) { }

	protected virtual void StartGame(GameManager manager)
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
		
	public virtual Player [] GetWinners()
	{
		return new Player[0];
	}

	public virtual void OnUpdate() { }

	public virtual void OnPlayerDeath(Player p) { }

	public virtual void OnPlayerStayInBeerArea(Player p) { }
	
	public virtual void OnPlayerMove(Player p) { }

	public virtual void OnBeerDestroyed(GameObject g) { }

	public virtual void OnRuleDisplayed() { }

	public virtual YieldInstruction StartingGame(GameManager manager)
	{
		this.StartGame(manager);
		return null;
	}

	public abstract void SetAI(Player p, PlayerAI ai);
}
