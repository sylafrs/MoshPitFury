using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour 
{
	private Player player;

	private FMOD_StudioEventEmitter bump;
	private FMOD.Studio.ParameterInstance bumpParam;

	private FMOD_StudioEventEmitter death;
	private FMOD.Studio.ParameterInstance deathParam;

	private void Awake()
	{
		player = this.GetComponent<Player>();
		// bump = this.transform.FindChild("Sounds/Bump").GetComponent<FMOD_StudioEventEmitter>();
		// bumpParam = bump.getParameter("Bump");
		// death = this.transform.FindChild("Sounds/Death").GetComponent<FMOD_StudioEventEmitter>();
		// deathParam = death.getParameter("Death");
	}

	private void OnDeath(bool fire)
	{
		// Param : Death -> 1
		// deathParam.setValue(1);
		// 
		// StopCoroutine("PlayDeathSound");
		// StartCoroutine("PlayDeathSound");
	}

	private void OnPushed(PushData data)
	{
		// Param : Bump -> 1
		// bumpParam.setValue(1);
		// 
		// StopCoroutine("PlayBumpSound");
		// StartCoroutine("PlayBumpSound");
	}

	private IEnumerator PlayBumpSound()
	{
		bump.Play();
		yield return new WaitForSeconds(2);
		bump.Stop();
	}

	private IEnumerator PlayDeathSound()
	{
		death.Play();
		yield return new WaitForSeconds(0.5f);
		death.Stop();
	}

	
}
