using UnityEngine;
using System.Collections;

public class BeerParticles : MonoBehaviour {
	
	public ParticleSystem [] destructionFX;
	public float groundPos;

	private void OnBeerDestroyed()
	{
		if (destructionFX != null)
		{
			foreach (ParticleSystem ps in destructionFX)
			{
				ps.transform.parent = null;

				Vector3 pos = ps.transform.position;
				pos.y = groundPos;
				ps.transform.position = pos;

				ps.enableEmission = true;
				ps.time = 0;
				ps.loop = false;
				GameObject.Destroy(ps.gameObject, ps.duration + 1);
			}
		}
	}
}
