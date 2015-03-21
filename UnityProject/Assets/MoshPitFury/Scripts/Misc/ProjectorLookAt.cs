using UnityEngine;
using System.Collections;

public class ProjectorLookAt : MonoBehaviour
{

	public Transform target;
	private Light spot;
	private Renderer volume;

	public void Awake()
	{
		this.spot = this.transform.FindChild("Spotlight").light;
		this.volume = this.transform.FindChild("Volume").renderer;
	}

	public void OnEnable()
	{
		spot.enabled = true;
		volume.enabled = true;
	}

	public void OnDisable()
	{
		spot.enabled = false;
		volume.enabled = false;
	}

	public void Update()
	{
		if (target != null)
		{
			this.transform.forward = (target.position - this.transform.position).normalized;
		}
	}
}
