using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;
	private GameObject Level;

	public Rule[] ExistingRules;
	private List<Rule> NotPlayedRules;

	public Transform[] SpawnPoints;

	public Rule UsedRule { get; private set; }
	public float RoundTimer { get; private set; }

	public List<Player> AlivePlayers { get; private set; }

	public Player[] Players { get; private set; }
	private Text LabelStartTimer;
	private Text LabelRuleName;
	private Text LabelRoundTimer;

	private void Awake()
	{
		Players = GameObject.FindObjectsOfType<Player>();
		LabelStartTimer = GameObject.Find("TXT_start_cooldown").GetComponent<Text>();
		LabelRuleName = GameObject.Find("TXT_rule_name").GetComponent<Text>();
		LabelRoundTimer = GameObject.Find("TXT_round_timer").GetComponent<Text>();
	}

	private void Start()
	{
		instance = this;
		StartCoroutine(StartGame());
	}

	public IEnumerator StartGame()
	{
		Debug.Log("start game");
				
		if(NotPlayedRules == null || NotPlayedRules.Count == 0)
			NotPlayedRules = new List<Rule>(this.ExistingRules);

		UsedRule		= NotPlayedRules[Random.Range(0, NotPlayedRules.Count)];

		NotPlayedRules.Remove(UsedRule);

		AlivePlayers	= new List<Player>(Players);

		UsedRule.Prepare(this);

		foreach (Player p in AlivePlayers)
		{
			p.transform.position = this.SpawnPoints[p.Id - 1].position;
			p.transform.forward = this.SpawnPoints[p.Id - 1].forward;
			p.SendMessage("OnPlayerPlaced");

			p.Prepare();
			p.JumpsTo(this.UsedRule.GetPlayerSpawnPoint(p), 2);
		}

		yield return StartCoroutine(CountDown(3, 0.3f, 3));
	
		LabelRuleName.enabled = true;
		LabelRuleName.text = UsedRule.Description;
		yield return new WaitForSeconds(2);
		LabelRuleName.enabled = false;
		this.LabelRoundTimer.enabled = true;

		RoundTimer = 0;
		UsedRule.StartGame(this);

		foreach (Player p in AlivePlayers)
		{
			p.transform.position = UsedRule.GetPlayerSpawnPoint(p).position;
			p.transform.forward = UsedRule.GetPlayerSpawnPoint(p).forward;
			p.SendMessage("OnPlayerPlaced");
			p.StartGame();
		}
	}

	void OnPlayerDeath(Player p)
	{
		Debug.Log(p.name + " is dead");

		if (this.UsedRule != null)
			this.UsedRule.SendMessage("OnPlayerDeath", p, SendMessageOptions.DontRequireReceiver);

		if (AlivePlayers.Contains(p))
			AlivePlayers.Remove(p);
	}

	void OnPlayerStayInBeerArea(Player p)
	{
		if (this.UsedRule != null)
			this.UsedRule.SendMessage("OnPlayerStayInBeerArea", p, SendMessageOptions.DontRequireReceiver);
	}

	void OnPlayerMove(Player p)
	{
		if (this.UsedRule != null)
			this.UsedRule.SendMessage("OnPlayerMove", p, SendMessageOptions.DontRequireReceiver);
	}

	void Update()
	{
		if(UsedRule != null && UsedRule.Started)
		{
			RoundTimer += Time.deltaTime;
			this.LabelRoundTimer.text = Mathf.FloorToInt(UsedRule.Duration - RoundTimer).ToString();

			if(UsedRule.IsFinished || Input.GetKeyDown(KeyCode.Space))
			{
				GameOver();
			}
		}
	}

	void GameOver()
	{
		this.LabelRoundTimer.enabled = false;
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
}
