using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1, 10)]
	public int Id;
	public bool IsDead;
	public bool HasStarted;
	private GameManager Manager;

	public bool CanMove = false;

	void Start () 
	{
		Manager = GameObject.FindObjectOfType<GameManager>();
	}

	private IEnumerator JumpsToCoroutine(Transform to, float duration)
	{
		const float ratioJump = 0.3f;
		const float up = 3;

		Vector3 from = this.transform.position;
		float maxY = this.transform.position.y + up;
		Vector3 v = this.transform.position;
		Vector3 fromFwd = this.transform.forward;

		float t = 0;
		while (t < duration * ratioJump)
		{
			v = Vector3.Lerp(from, to.position, t / duration);
			v.y = Mathf.Lerp(from.y, maxY, t / (duration * ratioJump));
			this.transform.position = v;
			this.transform.forward = Vector3.Slerp(fromFwd, to.forward, t / duration);

			yield return null;
			t += Time.deltaTime;
		}

		while(t < duration)
		{
			v = Vector3.Lerp(from, to.position, t / duration);
			v.y = Mathf.Lerp(maxY, to.position.y, (t - (duration * ratioJump)) / (duration * (1 - ratioJump)));
			this.transform.position = v;
			this.transform.forward = Vector3.Slerp(fromFwd, to.forward, t / duration);

			yield return null;
			t += Time.deltaTime;
		}
	}

	public Coroutine JumpsTo(Transform to, float duration)
	{
		return StartCoroutine(JumpsToCoroutine(to, duration));
	}

	public void Prepare()
	{
		HasStarted = false;
		IsDead = false;
		this.rigidbody.isKinematic = true;
		this.CanMove = false;
	}

	public void StartGame()
	{
		IsDead = false;
		HasStarted = true;
		this.rigidbody.isKinematic = false;
		this.CanMove = true;
	}

	void OnGroundReached()
	{
		Vector3 v = this.transform.position;
			
		if (v.y > -0.1f)
		{
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
			this.rigidbody.useGravity = false;
			v.y = 0;
			this.transform.position = v;
		}
	}

	void OnGroundLost()
	{
		this.rigidbody.useGravity = true;
		this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
		
	void OnDeathTrigger()
	{
		Death();
	}

	void Death()
	{
		Manager.SendMessage("OnPlayerDeath", this);
		this.CanMove = false;
		IsDead = true;
	}

	void OnMouseDown()
	{
		this.Death();
	}
}
