using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleSurvival : Rule
{
	[Tooltip("Chances que la bouteille arrive sur un joueur"), Range(0, 1)]
	public float ChancesToTarget;

	[Tooltip("Ecart entre la position du joueur possible et la position finale (aléatoire)")]
	public float OffsetTarget;

	public float ParadiseBeerTime;

	private Dictionary<Player, AbsentInfo> PlayerAbsentTime;

	public float MinCooldown;
	public float MaxCooldown;

#if USING_GRAVITY
	public float InitialBeerSpeed;
	public float InitialTargetingBeerSpeed;
	public float InitialParadiseBeerSpeed;
#else
	public float BeerDuration;
	public float BeerHeight;
	public float TargetingBeerDuration;
	public float ParadiseBeerDuration;
	public float ParadiseBeerHeight;
#endif

	private float CooldownTimer;
	private GameObject Beer;

	public Transform boxA, boxB;

	public Transform publicLeftA, publicLeftB;
	public Transform publicRightA, publicRightB;
	public Transform publicBotA, publicBotB;

	private Transform Beers;
	private List<GameObject> BeerClones;

	private class AbsentInfo
	{
		public float remaining;
		public Vector3 position;

		public AbsentInfo(float remaining)
		{
			this.remaining = remaining;
		}
	}

	public override void Prepare(GameManager manager)
	{
		this.enabled = true;
		Beer = this.transform.FindChild("Beer").gameObject;
		BeerClones = new List<GameObject>();
		Beers = new GameObject("Beers").transform;

		PlayerAbsentTime = new Dictionary<Player, AbsentInfo>();
		foreach(Player p in manager.Players)
		{
			PlayerAbsentTime.Add(p, new AbsentInfo(ParadiseBeerTime));
		}

		
	}

	public override string Description
	{
		get { return "Survive to the beer hell"; }
	}

	public override Player[] GetWinners()
	{
		return this.Manager.AlivePlayers.ToArray();
	}

	public override void OnUpdate()
	{
		if (!IsFinished)
		{
			CooldownTimer -= Time.deltaTime;
			if (CooldownTimer <= 0)
			{
				ThrowBeer();
				CooldownTimer = Random.Range(MinCooldown, MaxCooldown);
			}

			foreach(Player p in this.Manager.AlivePlayers)
			{
				if (this.PlayerAbsentTime[p].position == p.transform.position)
				{
					this.PlayerAbsentTime[p].remaining -= Time.deltaTime;
					if(this.PlayerAbsentTime[p].remaining < 0)
					{
						ThrowParadiseBeer(p.transform.position);
						this.PlayerAbsentTime[p].remaining = ParadiseBeerTime * 2;
					}
				}
				else
				{
					this.PlayerAbsentTime[p].remaining = ParadiseBeerTime;
					this.PlayerAbsentTime[p].position = p.transform.position;
				}
			}
		}
	}

	public override void GameOver()
	{
		this.enabled = false;
		base.GameOver();
		GameObject.Destroy(Beers.gameObject);
	}

	public override void OnBeerDestroyed(GameObject g)
	{
		this.BeerClones.Remove(g);
	}

	private GameObject CreateBeer()
	{
		GameObject BeerClone = GameObject.Instantiate(this.Beer) as GameObject;
		this.BeerClones.Add(BeerClone);

		BeerClone.SendMessage("OnSetCallback", this.Manager);		

		BeerClone.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere.normalized * Random.Range(5f, 20f);		
		BeerClone.transform.parent = Beers;

		return BeerClone;
	}

	private void ThrowBeer()
	{
		GameObject BeerClone = CreateBeer();

		Vector3 position;

		float targetRandom = Random.Range(0f, 1f);
		int nPlayers = this.Manager.AlivePlayers.Count;

		if (targetRandom <= this.ChancesToTarget)
		{
			Vector2 r = Random.insideUnitCircle * OffsetTarget;
			position = GetRandomAlivePlayerPosition() + new Vector3(r.x, 0, r.y);
		}
		else
		{
			position = GetRandomPosition(boxA, boxB);
		}
		
#if USING_GRAVITY		
		if (targetRandom <= this.ChancesToTarget)
			BeerClone.rigidbody.velocity = -Vector3.up * this.InitialTargetingBeerSpeed;
		else
			BeerClone.rigidbody.velocity = -Vector3.up * this.InitialBeerSpeed;
		
		BeerClone.transform.position = position;
#else
		position.y = 0;

		LaunchedItem launched = BeerClone.AddComponent<LaunchedItem>();
		BeerClone.GetComponent<Rigidbody>().useGravity = false;

		BeerClone.transform.position = GetRandomPublicPosition();

		if (targetRandom <= this.ChancesToTarget)
			launched.Duration = TargetingBeerDuration;
		else
			launched.Duration = BeerDuration;

		launched.FinalPosition = position;
		launched.Height = BeerHeight;
#endif

	}

	private void ThrowParadiseBeer(Vector3 position)
	{
		GameObject BeerClone = CreateBeer();
		
#if USING_GRAVITY		
		position.y = minP.y;
		BeerClone.rigidbody.velocity = -Vector3.up * this.InitialParadiseBeerSpeed;
		BeerClone.transform.position = position;
#else
		position.y = 0;

		LaunchedItem launched = BeerClone.AddComponent<LaunchedItem>();
		BeerClone.GetComponent<Rigidbody>().useGravity = false;

		BeerClone.transform.position = GetRandomPublicPosition();

		BeerClone.transform.localScale *= 2;

		launched.Duration = ParadiseBeerDuration;
		launched.FinalPosition = position;
		launched.Height = ParadiseBeerHeight;
#endif
	}

	private Vector3 GetRandomPublicPosition()
	{
		Transform a = null, b = null;

		int side = Random.Range(0, 3);
		switch(side)
		{
			case 0:
				a = publicLeftA;
				b = publicLeftB;
				break;

			case 1:
				a = publicBotA;
				b = publicBotB;
				break;

			case 2:
				a = publicRightA;
				b = publicRightB;
				break;
		}

		return GetRandomPosition(a, b);
	}

	private Vector3 GetRandomAlivePlayerPosition()
	{
		int target = Random.Range(0, this.Manager.AlivePlayers.Count);
		Vector3 position = this.Manager.AlivePlayers[target].transform.position;
		position.y = GetRandomHeight(boxA, boxB);
		return position;
	}
		
	private float GetRandomHeight(Transform boxA, Transform boxB)
	{
		Vector3 A = boxA.position, B = boxB.position;

		float minP = Mathf.Min(A.y, B.y);
		float maxP = Mathf.Max(A.y, B.y);
		float random = Random.Range(minP, maxP);

		return random;
	}

	private Vector3 GetRandomPosition(Transform boxA, Transform boxB)
	{
		Vector3 A = boxA.position, B = boxB.position;

		Vector3 minP = new Vector3(
			Mathf.Min(A.x, B.x),
			Mathf.Min(A.y, B.y),
			Mathf.Min(A.z, B.z)
		);

		Vector3 maxP = new Vector3(
			Mathf.Max(A.x, B.x),
			Mathf.Max(A.y, B.y),
			Mathf.Max(A.z, B.z)
		);

		Vector3 random = new Vector3(
			Random.Range(minP.x, maxP.x),
			Random.Range(minP.y, maxP.y),
			Random.Range(minP.z, maxP.z)
		);

		return random;
	}
}
