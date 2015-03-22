using UnityEngine;
using System.Collections;

public class BeerParticles : MonoBehaviour {
	
	public ParticleSystem destructionFX;

	private void OnBeerDestroyed()
	{
		if (destructionFX != null)
		{
			destructionFX.enableEmission = true;
			destructionFX.time = 0;
			destructionFX.loop = false;
			destructionFX.transform.parent = null;
			GameObject.Destroy(destructionFX.gameObject, destructionFX.duration + 1);
		}
	}
}
