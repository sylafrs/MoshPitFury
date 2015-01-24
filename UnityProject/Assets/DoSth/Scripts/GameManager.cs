using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;
	private GameObject Level;

	public Rule[] ExistingRules;
	public Transform[] SpawnPoints;

	public Rule UsedRule { get; private set; }


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

		foreach (Player p in AlivePlayers)
		{
			p.transform.position = this.SpawnPoints[p.Id - 1].position;
			p.transform.forward = this.SpawnPoints[p.Id - 1].forward;

			p.Prepare();
			p.JumpsTo(this.UsedRule.SpawnPoints[p.Id - 1], 2);
		}

		yield return StartCoroutine(CountDown(3, 0.3f, 3));
	
		LabelRuleName.enabled = true;
		LabelRuleName.text = UsedRule.Name;
		yield return new WaitForSeconds(2);
		LabelRuleName.enabled = false;
	
		UsedRule.StartGame(this);

		foreach (Player p in AlivePlayers)
		{
			p.transform.position	= UsedRule.SpawnPoints[p.Id - 1].position;
			p.transform.forward		= UsedRule.SpawnPoints[p.Id - 1].forward;
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
			if(UsedRule.IsFinished || Input.GetKeyDown(KeyCode.Space))
			{
				GameOver();
			}
		}
	}

	void GameOver()
	{
		this.UsedRule.GameOver();
		this.UsedRule = null;

		StartCoroutine(StartGame());
	}

	private IEnumerator CountDown(float duration, float readyRatio, int compte)
	{
		LabelStartTimer.enabled = true;
		LabelStartTimer.text = "Ready?";
		yield return new WaitForSeconds(duration * readyRatio);

		// On a duration * 0.7f pour seconds + 1 iteration.
		// Donc une itération : duration * 0.7f * (1 / 1+iteration)

		float durationIteration = duration * (1 - readyRatio) / (1 + compte);

		for (int seconds = compte; seconds > 0; seconds--)
		{
			LabelStartTimer.text = seconds.ToString();
			yield return new WaitForSeconds(durationIteration);
		}
		LabelStartTimer.text = "GO!";
		yield return new WaitForSeconds(durationIteration);
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
