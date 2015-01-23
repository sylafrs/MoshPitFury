using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameManager Manager;

	void Start () {
		Manager = GameObject.FindObjectOfType<GameManager>();
	}

	void OnDeath()
	{
		Manager.SendMessage("OnPlayerDeath", this);
	}

	void OnMouseDown()
	{
		this.OnDeath();
	}
}
