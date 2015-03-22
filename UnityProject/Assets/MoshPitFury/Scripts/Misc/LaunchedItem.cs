using UnityEngine;
using System.Collections;

public class LaunchedItem : MonoBehaviour {

	private Vector3		InitialPosition;
	private Vector3		InitialSpeed;
	private Vector3		Acceleration;
	public	Vector3		FinalPosition;
	private Vector3		CurrentSpeed;

	public Transform	Target;

	public float		Duration;	
	public float		Height;

	// ------------------------------------------------------- 
	//
	// a(t) = a(0)
	// v(t) = v(0) + a(0)t 
	// p(t) = p(0) + v(0)t + (a(0)t²) / 2
	//
	// -------------------------------------------------------
	//
	// Connaissant: p(0) p(tm) v(tm)
	// On veut connaitre a(0)
	//
	// v(tm) = a(0)tm + v(0) <=> v(0) = v(tm) - a(0)tm
	// p(tm) = a(0)tm²/2 + v(0)tm + p(0)
	//		 = a(0)tm²/2 + v(tm)tm - a(0)tm² + p(0)
	//		 = -a(0)tm²/2 + v(tm)tm + p(0)
	// <=> 0 = -a(0)tm²/2 + v(tm)tm + p(0) - p(tm)
	// <=> a(0)tm²/2 = v(tm)tm + p(0) - p(tm)
	// <=> a(0) = 2 * (v(tm)tm + p(0) - p(tm)) / tm²
	//
	// Admettons, en y, vy(tm) = 0, py(0) = 0 et soit h = py(tm) :
	// ay(0) = -2h / tm² 
	// Admettons que nous ne voulons pas d'acceleration lattérales :
	// a(0) = (0, -2h/tm², 0)
	//
	// -------------------------------------------------------
	//
	// Connaissant: a(0) p(0) tf p(tf) 
	// On veut connaitre v(0).
	//
	// p(tf) = p(0) + v(0)tf + (a(0)tf²) / 2
	// p(tf) - p(0) - (a(0)tf²) / 2 = v(0)tf
	// v(0) = (p(tf) - p(0) - (a(0)tf²) / 2) / tf
	//
	// -------------------------------------------------------


	void Start()
	{
		float tm = Duration * 0.5f;

		InitialPosition = this.transform.position;

		if(Target)
			FinalPosition = Target.position;

		if (Duration != 0)
		{
			Acceleration = Vector3.up * (-2 * Height) / (tm * tm);
			InitialSpeed = (FinalPosition - 0.5f * Acceleration * Duration * Duration - InitialPosition) / Duration;
			CurrentSpeed = InitialSpeed;
		}
	}

	void Update ()
	{
		CurrentSpeed += this.Acceleration * Time.deltaTime;
		this.transform.position += CurrentSpeed * Time.deltaTime;
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			InitialPosition = this.transform.position;
			FinalPosition = Target.position;
		}

		Vector3 p1 = InitialPosition;
		Vector3 p2 = FinalPosition;

		p1.y = p2.y = Height;

		Gizmos.DrawLine(p1, p2);
	}
}
