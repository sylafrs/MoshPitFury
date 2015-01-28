using UnityEngine;
using System.Collections;

using FMODEvent = FMOD_StudioEventEmitter;
using FMODParameter = FMOD.Studio.ParameterInstance;
using System;

public class PlayerSounds : MonoBehaviour
{
	private Player player;

	private FMODEvent bump;
	private FMODParameter bumpParam;

	private FMODEvent death;
	private FMODParameter deathParam;

	private FMODParameter GetParameter(FMODEvent evt, string pName)
	{
		FMODParameter p = null;

		if (evt != null && !string.IsNullOrEmpty(pName))
		{
			try
			{
				p = evt.getParameter(pName);
			}
			catch (Exception e)
			{
				Debug.LogError(e, evt);
			}
		}

		return p;
	}

	private FMODEvent GetEvent(string name)
	{
		Transform soundsTransform = this.transform.FindChild("Sounds");
		if (soundsTransform == null)
		{
			Debug.LogError("Missing Sounds parent.", this);
			return null;
		}

		Transform eventTransform = soundsTransform.FindChild(name);
		if (eventTransform == null)
		{
			Debug.LogError("Missing " + name + " object.", soundsTransform);
			return null;
		}

		FMODEvent evnt = eventTransform.GetComponent<FMODEvent>();
		if (evnt == null)
			Debug.LogError("Missing FMODEvent component.", eventTransform);
		return evnt;
	}

	private void Start()
	{
		player = this.GetComponent<Player>();
		bump = GetEvent("Bump");
		bumpParam = GetParameter(bump, "Bump");
		death = GetEvent("Death");
		deathParam = GetParameter(death, "Death");
	}

	private void OnDeath(bool fire)
	{
		// Param : Death -> 1
		deathParam.setValue(1);

		StopCoroutine("PlayDeathSound");
		StartCoroutine("PlayDeathSound");
	}

	private void OnPushed(PushData data)
	{
		// Param : Bump -> 1
		bumpParam.setValue(1);

		StopCoroutine("PlayBumpSound");
		StartCoroutine("PlayBumpSound");
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
