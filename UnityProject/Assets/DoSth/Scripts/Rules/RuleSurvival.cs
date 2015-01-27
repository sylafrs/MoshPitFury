using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleSurvival : Rule {

	public float MinCooldown;
	public float MaxCooldown;
	private float CooldownTimer;
	private GameObject Beer;

	public Transform boxA, boxB;

    private Transform Beers;
	private List<GameObject> BeerClones;

	public override void Prepare(GameManager manager)
	{
		this.enabled = true;
		Beer = this.transform.FindChild("Beer").gameObject;
		BeerClones = new List<GameObject>();
        Beers = new GameObject("Beers").transform;
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
		if(!IsFinished)
		{
			CooldownTimer -= Time.deltaTime;
			if(CooldownTimer <= 0)
			{
				ThrowBeer();
				CooldownTimer = Random.Range(MinCooldown, MaxCooldown);
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

	private void ThrowBeer()
	{
		
		GameObject BeerClone = GameObject.Instantiate(this.Beer) as GameObject;
		this.BeerClones.Add(BeerClone);

		BeerClone.SendMessage("OnSetCallback", this.Manager);

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

        BeerClone.transform.parent = Beers;
		BeerClone.transform.position = random;
	}
}
