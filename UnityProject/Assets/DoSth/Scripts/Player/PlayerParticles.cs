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

		walk		= this.transform.FindChild("Particles/Walk")		.particleSystem;
		dash		= this.transform.FindChild("Particles/Dash")		.particleSystem;
		death		= this.transform.FindChild("Particles/Death")		.particleSystem;
		death_bone	= this.transform.FindChild("Particles/Death/Bone")	.particleSystem;
		choc		= this.transform.FindChild("Particles/Choc")		.particleSystem;
		choc_smoke	= this.transform.FindChild("Particles/Choc/Smoke")	.particleSystem;
		death_flame = this.transform.FindChild("Particles/FlameDeath").particleSystem;
		death_flame_flames = this.transform.FindChild("Particles/FlameDeath/FLAMES").particleSystem;
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

	void OnDeath()
	{
		GameObject deathClone = GameObject.Instantiate(death.gameObject) as GameObject;
		deathClone.transform.position = death.transform.position;
		deathClone.transform.rotation = death.transform.rotation;

		deathClone.particleSystem.enableEmission = true;
		deathClone.particleSystem.loop = false;

		Transform deathCloneBone = deathClone.transform.FindChild("Bone");
		deathCloneBone.particleSystem.enableEmission = true;
		deathCloneBone.particleSystem.loop = false;

		GameObject.Destroy(deathClone, deathClone.particleSystem.duration + 1);
	}

	void OnPushed(PushData data)
	{
		// Debug.Log("PUSHED " + data.Collision.contacts[0].point);

		GameObject chocClone = GameObject.Instantiate(choc.gameObject) as GameObject;
		chocClone.transform.position = data.Collision.contacts[0].point;
		chocClone.transform.rotation = choc.transform.rotation;

		chocClone.particleSystem.enableEmission = true;
		chocClone.particleSystem.loop = false;

		Transform chocCloneSmoke = chocClone.transform.FindChild("Smoke");
		chocCloneSmoke.particleSystem.enableEmission = true;
		chocCloneSmoke.particleSystem.loop = false;

		GameObject.Destroy(chocClone, chocClone.particleSystem.duration + 1);
	}
}
