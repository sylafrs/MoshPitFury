using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1, 10)]
	public int Id;
	private GameManager Manager;

	void Start () 
	{
		Manager = GameObject.FindObjectOfType<GameManager>();
	}

	public void StartGame()
	{
		this.renderer.material.color = Color.green;
	}

	void OnDeathTrigger()
	{
		Death();
	}

	void Death()
	{
		Manager.SendMessage("OnPlayerDeath", this);
		this.renderer.material.color = Color.red;
	}

	void OnMouseDown()
	{
		this.Death();
	}
}
