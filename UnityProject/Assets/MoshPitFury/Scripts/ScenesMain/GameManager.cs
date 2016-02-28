using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public const float END_ROUND_DURATION = 2f;

	private bool DebugMode = false;
	public static GameManager instance;
	private GameObject Level;
	public int ScoreToReach;

	public Rule[] ExistingRules;
	public bool ForceFirstExistingRule;
	private List<Rule> NotPlayedRules;

	public Transform[] SpawnPoints { get; private set; }
	public Transform[] RuleStartPoints { get; private set; }

	public Rule UsedRule { get; private set; }
	public float RoundTimer { get; private set; }

	public List<Player> AlivePlayers { get; private set; }

	public Player[] Players { get; private set; }
	private Text LabelStartTimer;

    public Sprite gameOverSprite;
    private Image ImageRuleName;
	private Text LabelRoundTimer;

	private MainThemeSoundManager MainTheme;

	private Text GetText(string name)
	{
		GameObject g = GameObject.Find(name);
		if (g == null)
			return null;
		return g.GetComponent<Text>();
	}

	private void Awake()
	{
		Players = GameObject.FindObjectsOfType<Player>();		
		if (Players.Length == 0)
		{
			Application.LoadLevel((int)SCENE.CharacterSelection);
			this.enabled = false;
			return;
		}

		MainTheme = this.GetComponent<MainThemeSoundManager>();
		LabelStartTimer = GetText("TXT_start_cooldown");
		LabelRoundTimer = GetText("TXT_round_timer");
		ImageRuleName = GameObject.Find("IMG_rule_name").GetComponent<Image>();

		Transform spawnPoints = GameObject.Find("SpawnPoints").transform;
		this.SpawnPoints = new Transform[Players.Length];
		this.RuleStartPoints = new Transform[Players.Length];

		for (int i = 0; i < Players.Length; i++)
		{
			this.SpawnPoints[i] = spawnPoints.FindChild("p" + (i + 1).ToString() + "a");
			this.RuleStartPoints[i] = spawnPoints.FindChild("P" + (i + 1).ToString());
		}

		foreach (Player p in Players)
		{
			p.Init();
		}
	}

	private void Start()
	{
		instance = this;
#if !DEBUG_MODE
		if(!DebugMode)
		{
			if(ForceFirstExistingRule)
			{
				StartCoroutine(StartGame(this.ExistingRules[0]));
			}
			else
			{
				StartCoroutine(StartGame());
			}
		}
#endif
	}

	public IEnumerator StartGame()
	{
		if (NotPlayedRules == null || NotPlayedRules.Count == 0)
			NotPlayedRules = new List<Rule>(this.ExistingRules);

		Rule randomRule = NotPlayedRules[Random.Range(0, NotPlayedRules.Count)];
		
		// CHEAT!!!
#if FORCE_SURVIVAL
		foreach (Rule r in ExistingRules)
			if (r is RuleSurvival)
				randomRule = r;
#endif
#if FORCE_KILL_ONE
		foreach (Rule r in ExistingRules)
			if (r is RuleKillOnePlayer)
				randomRule = r;
#endif

		return StartGame(randomRule);
	}

	public Transform GetStartPoint(Rule r, Player p)
	{
		Transform retval = this.UsedRule.GetPlayerStartPoint(p);
		if (retval != null)
			return retval;
		return this.RuleStartPoints[p.Id - 1];
	}

	public IEnumerator StartGame(Rule usedRule)
	{
		if (usedRule == null)
			throw new System.ArgumentNullException("usedRule must not be null");

		if (NotPlayedRules == null || NotPlayedRules.Count == 0)
			NotPlayedRules = new List<Rule>(this.ExistingRules);

		if(NotPlayedRules.Contains(usedRule))
			NotPlayedRules.Remove(usedRule);

		UsedRule = usedRule;
		UsedRule.Prepare(this);

		AlivePlayers = new List<Player>(Players);
		foreach (Player p in Players)
		{
			p.transform.position = this.SpawnPoints[p.Id - 1].position;
			p.transform.forward = this.SpawnPoints[p.Id - 1].forward;

			p.Prepare();
			p.gameObject.SendMessage("OnPlayerPlaced");
			p.JumpsTo(GetStartPoint(this.UsedRule, p), 0.8f, 3);
		}

        //yield return StartCoroutine(CountDown(1.25f, 0.3f, 3));
        yield return StartCoroutine(CountDown(1.5f, 1, 3));

        ImageRuleName.enabled = true;
        ImageRuleName.sprite = UsedRule.ruleSprite;
		UsedRule.OnRuleDisplayed();
		yield return new WaitForSeconds(2);

        ImageRuleName.enabled = false;
		this.LabelRoundTimer.enabled = true;

		RoundTimer = 0;
		yield return UsedRule.StartingGame(this);
		
		SpawnPlayers();
	}

	public void SpawnPlayers()
	{
		AlivePlayers = new List<Player>(Players);
		foreach (Player p in AlivePlayers)
		{
			if (UsedRule != null)
			{
				p.transform.position = this.GetStartPoint(this.UsedRule, p).position;
				p.transform.forward = this.GetStartPoint(this.UsedRule, p).forward;
			}
			else
			{
				p.transform.position = this.GetStartPoint(ExistingRules[0], p).position;
				p.transform.forward = this.GetStartPoint(ExistingRules[0], p).forward;
			}

			p.gameObject.SendMessage("OnPlayerPlaced");
			p.StartGame();
		}
	}

	public void OnPlayerDeath(Player p)
	{
		if (this.UsedRule != null)
			this.UsedRule.OnPlayerDeath(p);

		if (AlivePlayers.Contains(p))
			AlivePlayers.Remove(p);
	}

	public void OnPlayerStayInBeerArea(Player p)
	{
		if (this.UsedRule != null)
			this.UsedRule.OnPlayerStayInBeerArea(p);
	}

	public void OnPlayerMove(Player p)
	{
		if (this.UsedRule != null)
			this.UsedRule.OnPlayerMove(p);
	}

	private void OnBeerDestroyed(GameObject g)
	{
		if (this.UsedRule != null)
			this.UsedRule.OnBeerDestroyed(g);
	}

	void Update()
	{
		if (UsedRule != null && UsedRule.Started)
		{
			RoundTimer += Time.deltaTime;
			this.LabelRoundTimer.text = Mathf.FloorToInt(UsedRule.Duration - RoundTimer).ToString();

			UsedRule.OnUpdate();
			if (UsedRule.IsFinished)
			{
				this.LabelRoundTimer.enabled = false;
				StartCoroutine(OnEndGame());
			}
		}
	}

#if DEBUG_MODE
	void OnGUI()
	{
		if(DebugMode)
		{
			if(UsedRule == null)
			{
				foreach(Rule rule in ExistingRules)
				{
					if (GUILayout.Button(rule.name))
						StartCoroutine(StartGame(rule));
				}

				if (GUILayout.Button("aleatoire"))
					StartCoroutine(StartGame());
			}

			if(GUILayout.Button("respawn"))
			{
				SpawnPlayers();
			}

			foreach (Player p in Players)
			{
				if (p.GetComponent<PlayerInput>())
				{
					if (p.GetComponent<PlayerInput>().StartButton)
					{
						SpawnPlayers();
						break;
					}
				}
			}
		}
		else if(UsedRule == null)
		{
			if (GUILayout.Button("start"))
				StartCoroutine(StartGame());
		}

		if (GUILayout.Button("DebugMode : " + DebugMode))
			DebugMode = !DebugMode;
	}
#endif

	private IEnumerator OnEndGame()
	{
		Player[] winners = UsedRule.GetWinners();

		this.UsedRule.GameOver();
		this.UsedRule = null;

		List<Coroutine> coroutines = new List<Coroutine>();

		// On lance les coroutines
		foreach (Player p in winners)
			coroutines.Add(p.OnPlayerWin());

		// On les attend
		foreach (Coroutine c in coroutines)
			yield return c;

		this.LabelRoundTimer.enabled = false;

		this.UsedRule = null;
		if (!DebugMode)
		{
			if (HasBeenScoreReached())
			{
				// Montrer le panneau des scores

				TableauDesScores.scores = new int[Players.Length];

				foreach (Player p in Players)
				{
					TableauDesScores.scores[p.Id - 1] = p.Score;
				}

				GameObject.Destroy(GameObject.Find("Players"));
                ImageRuleName.sprite = gameOverSprite;
                ImageRuleName.enabled = true;
				yield return MainTheme.StartCoroutine(MainTheme.EndOfMusic());

				Application.LoadLevel((int)SCENE.Score);
			}
			else
			{
				StartCoroutine(StartGame());
			}
		}
	}

	private bool HasBeenScoreReached()
	{
		foreach (Player p in Players)
		{
			if (p.Score >= ScoreToReach)
			{
				return true;
			}
		}
		return false;
	}

	private void ScoreReached()
	{

	}

	private IEnumerator CountDown(float duration, float readyRatio, int compte)
	{
		LabelStartTimer.enabled = true;

        if (duration * readyRatio > 0)
        {
            LabelStartTimer.text = "Ready?";
            yield return new WaitForSeconds(duration * readyRatio);
        }

		// On a duration * 0.7f pour seconds + 1 iteration.
		// Donc une itération : duration * 0.7f * (1 / 1+iteration)

		float durationIteration = duration * (1 - readyRatio) / (1 + compte);

		for (int seconds = compte; seconds > 0; seconds--)
		{
            if (durationIteration > 0) 
            { 
			    LabelStartTimer.text = seconds.ToString();
                yield return new WaitForSeconds(durationIteration);
            }
		}

        if (durationIteration > 0)
        {
            LabelStartTimer.text = "GO!";
            yield return new WaitForSeconds(durationIteration);
        }
     
        LabelStartTimer.enabled = false;
	}
}
