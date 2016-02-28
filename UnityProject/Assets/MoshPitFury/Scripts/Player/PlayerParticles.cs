using UnityEngine;
using System.Collections;

public class PlayerParticles : MonoBehaviour {

	private Player player;

	private ParticleSystem walk;
	private ParticleSystem dash;
	private ParticleSystem death;
	private ParticleSystem death_bone;
	private ParticleSystem choc;
	private ParticleSystem choc_smoke;
	private ParticleSystem death_flame;
	private ParticleSystem death_flame_flames;
		
	void Awake()
	{
		player = this.GetComponent<Player>();

		walk		= this.transform.FindChild("Particles/Walk")		.GetComponent<ParticleSystem>();
		dash		= this.transform.FindChild("Particles/Dash")		.GetComponent<ParticleSystem>();
		death		= this.transform.FindChild("Particles/Death")		.GetComponent<ParticleSystem>();
		death_bone	= this.transform.FindChild("Particles/Death/Bone")	.GetComponent<ParticleSystem>();
		choc		= this.transform.FindChild("Particles/Choc")		.GetComponent<ParticleSystem>();
		choc_smoke	= this.transform.FindChild("Particles/Choc/Smoke")	.GetComponent<ParticleSystem>();
		death_flame = this.transform.FindChild("Particles/FlameDeath").GetComponent<ParticleSystem>();
		death_flame_flames = this.transform.FindChild("Particles/FlameDeath/FLAMES").GetComponent<ParticleSystem>();
	}

	void Start()
	{
		walk.enableEmission			= false;
		dash.enableEmission			= false;
		death.enableEmission		= false;
		death_bone.enableEmission	= false;
		choc.enableEmission			= false;
		choc_smoke.enableEmission	= false;
		death_flame.enableEmission = false;
		death_flame_flames.enableEmission = false;
	}

	void OnMove()
	{
		walk.enableEmission = !player.IsDashing;
		dash.enableEmission = player.IsDashing;
	}

	void OnIdle()
	{
		walk.enableEmission = false;
		dash.enableEmission = false;
	}

	void OnDeath(bool flames)
	{
		if (flames)
		{
			GameObject deathFClone = GameObject.Instantiate(death_flame.gameObject) as GameObject;
			deathFClone.transform.position = death_flame.transform.position;
			deathFClone.transform.rotation = death_flame.transform.rotation;

			deathFClone.GetComponent<ParticleSystem>().enableEmission = true;
			deathFClone.GetComponent<ParticleSystem>().time = 0;
			deathFClone.GetComponent<ParticleSystem>().loop = false;

			Transform deathFCloneFlames = deathFClone.transform.FindChild("FLAMES");
			deathFCloneFlames.GetComponent<ParticleSystem>().enableEmission = true;
			deathFCloneFlames.GetComponent<ParticleSystem>().time = 0;
			deathFCloneFlames.GetComponent<ParticleSystem>().loop = false;

			GameObject.Destroy(deathFClone, deathFClone.GetComponent<ParticleSystem>().duration + 1);
		}
		else
		{
			GameObject deathClone = GameObject.Instantiate(death.gameObject) as GameObject;
			deathClone.transform.position = death.transform.position;
			deathClone.transform.rotation = death.transform.rotation;

			deathClone.GetComponent<ParticleSystem>().enableEmission = true;
			deathClone.GetComponent<ParticleSystem>().time = 0;
			deathClone.GetComponent<ParticleSystem>().loop = false;

			Transform deathCloneBone = deathClone.transform.FindChild("Bone");
			deathCloneBone.GetComponent<ParticleSystem>().enableEmission = true;
			deathCloneBone.GetComponent<ParticleSystem>().loop = false;

			GameObject.Destroy(deathClone, deathClone.GetComponent<ParticleSystem>().duration + 1);
		}
	}

	void OnPushed(PushData data)
	{
		StartCoroutine(LaunchChoc(data));
	}

	IEnumerator LaunchChoc(PushData data)
	{
		// Debug.Log("PUSHED " + data.Collision.contacts[0].point);

		GameObject chocClone = GameObject.Instantiate(choc.gameObject) as GameObject;

		chocClone.transform.position = data.Collision.contacts[0].point;	// Choc au point de contact
		//chocClone.transform.position = data.Pushed.transform.position;	// Choc sur le personnage poussé.
		//chocClone.transform.parent	 = data.Pushed.transform;			// Suit le personnage poussé.

		chocClone.transform.rotation = choc.transform.rotation;

		yield return null;

		chocClone.GetComponent<ParticleSystem>().enableEmission = true;
		chocClone.GetComponent<ParticleSystem>().time = 0;
		chocClone.GetComponent<ParticleSystem>().loop = false;

		Transform chocCloneSmoke = chocClone.transform.FindChild("Smoke");
		chocCloneSmoke.GetComponent<ParticleSystem>().enableEmission = true;
		chocCloneSmoke.GetComponent<ParticleSystem>().time = 0;
		chocCloneSmoke.GetComponent<ParticleSystem>().loop = false;

		GameObject.Destroy(chocClone, chocClone.GetComponent<ParticleSystem>().duration + 1);
	}
}
