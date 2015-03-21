using UnityEngine;
using System.Collections;

public class MainThemeSoundManager : MonoBehaviour {

	private FMOD_StudioEventEmitter e;
	private FMOD.Studio.ParameterInstance themeMetal;

	private void Start()
	{
		e = this.GetComponent<FMOD_StudioEventEmitter>();
		themeMetal = e.getParameter("Theme_Metal");
		this.StartCoroutine(GoToLoop());
	}

	private IEnumerator GoToLoop()
	{
		yield return new WaitForSeconds(8.5f);
		themeMetal.setValue(3);
	}

	public IEnumerator EndOfMusic()
	{
		themeMetal.setValue(5);
		yield return new WaitForSeconds(7);
	}
}
