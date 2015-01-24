using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1, 10)]
	public int Id;
	public bool IsDead;
	public bool HasStarted;
	private GameManager Manager;

	public bool CanMove = false;
	public Color MainColor;
	public ProjectorLookAt projector;
	
	public char GetHex (int d) 
	{
		string alpha = "0123456789ABCDEF";
		return alpha[d];
	}

	public string MainColorHex {
		get 
		{
			float red	=	MainColor.r * 255;
			float green	=	MainColor.g * 255;
			float blue	=	MainColor.b * 255;
 
			char a = GetHex(Mathf.FloorToInt(red / 16));
			char b = GetHex(Mathf.RoundToInt(red % 16));
			char c = GetHex(Mathf.FloorToInt(green / 16));
			char d = GetHex(Mathf.RoundToInt(green % 16));
			char e = GetHex(Mathf.FloorToInt(blue / 16));
			char f = GetHex(Mathf.RoundToInt(blue % 16));
 
			return "#" + a + b + c + d + e + f;
		}
	}

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
		this.CanMove = false;
	}

	public void StartGame()
	{
		IsDead = false;
		HasStarted = true;
		this.CanMove = true;
	}
		
	void OnDeathTrigger()
	{
		Death();
	}

	void OnBeerAreaStay()
	{
		if(!IsDead)
			Manager.OnPlayerStayInBeerArea(this);
	}

	public void Death()
	{
		Manager.OnPlayerDeath(this);
		this.CanMove = false;
		IsDead = true;
	}
	
	public void OnMove()
	{
		Manager.OnPlayerMove(this);
	}

	public Coroutine OnPlayerWin()
	{
		return StartCoroutine(ActiveProjector(3));
	}

	private IEnumerator ActiveProjector(float duration)
	{
		projector.target = transform;
		projector.enabled = true;
		yield return new WaitForSeconds(duration);
		projector.enabled = false;
	}
}
