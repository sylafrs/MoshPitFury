using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1, 10)]
	public int Id;
	private GameManager Manager;

	public bool CanMove = false;

	void Start () 
	{
		Manager = GameObject.FindObjectOfType<GameManager>();
	}

	public void Prepare()
	{
		this.rigidbody.isKinematic = true;
		this.renderer.enabled = false;
		this.CanMove = false;
	}

	public void StartGame()
	{
		this.rigidbody.isKinematic = false;
		this.renderer.material.color = Color.green;
		this.renderer.enabled = true;
		this.CanMove = true;
	}

	void OnDeathTrigger()
	{
		Death();
	}

	void Death()
	{
		Manager.SendMessage("OnPlayerDeath", this);
		this.renderer.material.color = Color.red;
		this.CanMove = false;
	}

	void OnMouseDown()
	{
		this.Death();
	}
}
