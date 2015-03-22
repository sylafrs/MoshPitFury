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
	public float InitialParadiseBeerSpeed;

	private Dictionary<Player, AbsentInfo> PlayerAbsentTime;
	
	public float MinCooldown;
	public float MaxCooldown;
	public float InitialBeerSpeed;
	public float InitialTargetingBeerSpeed;
	private float CooldownTimer;
	private GameObject Beer;

	public Transform boxA, boxB;

	private Transform Beers;
	private List<GameObject> BeerClones;

	private Vector3 minP, maxP;

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

		Vector3 A = boxA.position, B = boxB.position;

		minP = new Vector3(
			Mathf.Min(A.x, B.x),
			Mathf.Min(A.y, B.y),
			Mathf.Min(A.z, B.z)
		);

		maxP = new Vector3(
			Mathf.Max(A.x, B.x),
			Mathf.Max(A.y, B.y),
			Mathf.Max(A.z, B.z)
		);
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

		BeerClone.rigidbody.angularVelocity = Random.insideUnitSphere.normalized * Random.Range(5f, 20f);		
		BeerClone.transform.parent = Beers;

		return BeerClone;
	}

	private void ThrowBeer()
	{
		GameObject BeerClone = CreateBeer();
		Vector3 position;
		
		float targetRandom = Random.Range(0f, 1f);
		int nPlayers = this.Manager.AlivePlayers.Count;

		if(targetRandom <= this.ChancesToTarget)
		{
			Vector2 r = Random.insideUnitCircle * OffsetTarget;
			position = GetRandomAlivePlayerPosition() + new Vector3(r.x, 0, r.y);
			BeerClone.rigidbody.velocity = -Vector3.up * this.InitialTargetingBeerSpeed;
		}
		else
		{
			position = GetRandomPosition();
			BeerClone.rigidbody.velocity = -Vector3.up * this.InitialBeerSpeed;
		}

		BeerClone.transform.position = position;
	}

	private void ThrowParadiseBeer(Vector3 position)
	{
		GameObject BeerClone = CreateBeer();
		position.y = minP.y;
		BeerClone.rigidbody.velocity = -Vector3.up * this.InitialParadiseBeerSpeed;
		BeerClone.transform.position = position;
	}

	private Vector3 GetRandomAlivePlayerPosition()
	{
		int target = Random.Range(0, this.Manager.AlivePlayers.Count);
		Vector3 position = this.Manager.AlivePlayers[target].transform.position;
		position.y = Random.Range(minP.y, maxP.y);
		return position;
	}

	private Vector3 GetRandomPosition()
	{
		Vector3 random = new Vector3(
			Random.Range(minP.x, maxP.x),
			Random.Range(minP.y, maxP.y),
			Random.Range(minP.z, maxP.z)
		);

		return random;
	}
}
