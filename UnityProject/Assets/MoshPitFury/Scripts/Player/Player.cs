using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	//	[Range(1, 4)]
	public int Id;

	public bool IsDead { get; private set; }
	public bool HasStarted { get; private set; }

	private GameManager Manager;

	private GameObject Model;

	public Transform BeerPlaceHolder;

	private PickableBeer Beer;

	public int Score { get; private set; }
	public string Name { get; private set; }

	[HideInInspector]
	public bool IsDashing = false;

	public bool CanMove { get; private set; }

	public Color MainColor;

	private ProjectorLookAt projector;

	public Light Halo { get; private set; }

	public char GetHex(int d)
	{
		string alpha = "0123456789ABCDEF";
		return alpha[d];
	}

	public string MainColorHex
	{
		get
		{
			float red = MainColor.r * 255;
			float green = MainColor.g * 255;
			float blue = MainColor.b * 255;

			char a = GetHex(Mathf.FloorToInt(red / 16));
			char b = GetHex(Mathf.RoundToInt(red % 16));
			char c = GetHex(Mathf.FloorToInt(green / 16));
			char d = GetHex(Mathf.RoundToInt(green % 16));
			char e = GetHex(Mathf.FloorToInt(blue / 16));
			char f = GetHex(Mathf.RoundToInt(blue % 16));

			return "#" + a + b + c + d + e + f;
		}
	}

	public void ResetScore()
	{
		Score = 0;
	}

	public void Init()
	{
		Model = this.transform.FindChild("Player_Model").gameObject;
		Manager = GameObject.FindObjectOfType<GameManager>();
		IsDashing = false;
		IsDead = false;
		CanMove = false;

		this.InitHalo();
		this.InitProjector();
	}

	private void InitHalo()
	{
		Transform t = this.transform.FindChild("Halo");
		if (t)
			this.Halo = t.light;
	}

	private void InitProjector()
	{
		if (this.transform.parent)
		{
			Transform t = this.transform.parent.FindChild("Projector");
			if (t)
				this.projector = t.GetComponent<ProjectorLookAt>();
		}
	}

	public void InitCursor()
	{
		Transform cursor = this.transform.parent.FindChild("NameUI");
		if (cursor)
		{
			TextMesh tm = cursor.GetComponent<TextMesh>();
			if (tm)
			{
				tm.color = this.MainColor;
				tm.text = Name = "P" + this.Id;
			}
		}

		cursor = this.transform.FindChild("Round");
		if(cursor)
		{
			cursor.renderer.material.color = this.MainColor;
			cursor.renderer.enabled = true;
		}
	}

	public void InitCPUCursor()
	{
		Transform cursor = this.transform.parent.FindChild("NameUI");
		if (cursor)
		{
			TextMesh tm = cursor.GetComponent<TextMesh>();
			if (tm)
			{
				tm.color = this.MainColor;
				tm.text = Name = "CPU" + this.Id;
			}
		}

		cursor = this.transform.FindChild("Round");
		if (cursor)
		{
			cursor.renderer.material.color = this.MainColor;
			cursor.renderer.enabled = true;
		}
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

		while (t < duration)
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
		if (Model)
			Model.SetActive(true);
		this.CanMove = false;
	}

	public void StartGame()
	{
		IsDead = false;
		HasStarted = true;
		this.CanMove = true;
		if (Model)
			Model.SetActive(true);
	}

	void OnDeathTrigger()
	{
		Death(true);
	}

	void OnBeerAreaStay()
	{
		if (!IsDead && Manager)
			Manager.OnPlayerStayInBeerArea(this);
	}

	public void Death(bool flames)
	{
		if (!IsDead)
		{
			this.gameObject.SendMessage("OnDeath", flames);
			if (Manager)
				Manager.OnPlayerDeath(this);
			this.CanMove = false;
			IsDead = true;

			if (Model)
				Model.SetActive(false);

			if (Beer)
			{
				Beer.Owner = null;
				Beer.transform.parent = null;
				Beer.transform.position = Vector3.zero;
				Beer.transform.rotation = Quaternion.identity;
				Beer.Projector.target = Beer.transform;
			}
		}
	}

	private void OnMove()
	{
		if (Manager)
			Manager.OnPlayerMove(this);
	}

	private void OnBeerCollision()
	{
		this.Death(false);
	}

	private void OnBeerRangeReached(PickageItemRange beer)
	{
		this.Beer = (PickableBeer)beer;
		this.PlaceBeer();
	}

	private void OnPushed(PushData data)
	{
		if (this.Beer)
		{
			data.Pusher.Beer = this.Beer;
			this.Beer = null;
			data.Pusher.PlaceBeer();
		}
	}

	private void OnBeerDestroyed()
	{
		this.Beer = null;
	}

	private void PlaceBeer()
	{
		if (this.Beer)
		{
			this.Beer.Owner = this;
			this.Beer.transform.parent = this.BeerPlaceHolder;
			this.Beer.transform.localPosition = Vector3.zero;
			this.Beer.transform.localRotation = Quaternion.identity;
			this.Beer.Projector.target = this.transform;
		}
	}

	public Coroutine OnPlayerWin()
	{
		Score++;
		return StartCoroutine(ActiveProjector(3));
	}

	private IEnumerator ActiveProjector(float duration)
	{
		if (projector)
		{
			projector.target = transform;
			projector.enabled = true;
		}
		yield return new WaitForSeconds(duration);
		if (projector)
			projector.enabled = false;
	}
}
