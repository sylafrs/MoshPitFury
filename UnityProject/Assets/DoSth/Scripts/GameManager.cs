using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public Rule[] ExistingRules;

	public Rule UsedRule { get; private set; }

	private GameObject CurrentLevel;

	public List<Player> AlivePlayers { get; private set; }

	private Player[] Players;
	private Text LabelStartTimer;
	private Text LabelRuleName;

	private void Awake()
	{
		Players = GameObject.FindObjectsOfType<Player>();
		LabelStartTimer = GameObject.Find("TXT_start_cooldown").GetComponent<Text>();
		LabelRuleName = GameObject.Find("TXT_rule_name").GetComponent<Text>();
	}

	private void Start()
	{
		instance = this;
		StartCoroutine(StartGame());
	}

	public IEnumerator StartGame()
	{
		Debug.Log("start game");
				
		UsedRule		= ExistingRules[Random.Range(0, ExistingRules.Length)]; // à gérer plus tard.
		AlivePlayers	= new List<Player>(Players);

		UsedRule.Prepare(this);

		yield return StartCoroutine(CountDown(3));
		CurrentLevel = Instantiate(UsedRule.Level, new Vector3(0, -3f, 0), Quaternion.identity) as GameObject;

		LabelRuleName.enabled = true;
		LabelRuleName.text = UsedRule.Name;
		yield return new WaitForSeconds(2);
		LabelRuleName.enabled = false;
	
		UsedRule.StartGame(this);

		foreach (Player p in AlivePlayers)
		{
			p.StartGame();
		}
	}

	void OnPlayerDeath(Player p)
	{
		Debug.Log(p.name + " is dead");

		if (AlivePlayers.Contains(p))
			AlivePlayers.Remove(p);
	}

	void Update()
	{
		if(UsedRule != null && UsedRule.Started)
		{
			if(UsedRule.IsFinished)
			{
				GameOver();
			}
		}
	}

	void GameOver()
	{
		this.UsedRule.GameOver();
		this.UsedRule = null;
		GameObject.Destroy(CurrentLevel);
		StartCoroutine(StartGame());
	}

	private IEnumerator CountDown(int duration)
	{
		LabelStartTimer.enabled = true;
		LabelStartTimer.text = "Ready?";
		yield return new WaitForSeconds(3);
		for (int seconds = duration; seconds > 0; seconds--)
		{
			LabelStartTimer.text = seconds.ToString();
			yield return new WaitForSeconds(1);
		}
		LabelStartTimer.text = "GO!";
		yield return new WaitForSeconds(1);
		LabelStartTimer.enabled = false;
	}

	public Player GetPlayerById(int id)
	{
		foreach(Player p in Players)
		{
			if (p.Id == id)
				return p;
		}

		return null;
	}

	public bool IsPlayerAlive(int PlayerToKill)
	{
		Player p = GetPlayerById(PlayerToKill);
		if (p == null)
			return false;
		return this.AlivePlayers.Contains(p);
	}
}
