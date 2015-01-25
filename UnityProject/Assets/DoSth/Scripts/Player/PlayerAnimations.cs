using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {

	private Player player;
	private string prevAnimation;

	public Animator myAnimator;

	void Start () {

		player = this.GetComponent<Player>();

	}
	
	void Update () {
	
	}

	void OnMove()
	{
//		walk.enableEmission = !player.IsDashing;
		if(player.IsDashing){
			UseAnimation("Dash");
		}
		else{
			UseAnimation("Run");
		}
	}
	
	void OnIdle()
	{
		UseAnimation("Idle");
	}
	
	void OnDeath()
	{

	}
	
	void OnPushed()
	{
		UseAnimation("Bump");

	}

	void UseAnimation(string str)
	{
		if(prevAnimation != str)
		{
			myAnimator.CrossFade(str, 0.1f);
			prevAnimation = str;
		}
	}
}
