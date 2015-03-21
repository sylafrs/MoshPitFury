using UnityEngine;
using System.Collections;

public class Crowd : MonoBehaviour {
	
	public Animator myAnimator;

	public bool isHeadBang;

	
	void Start () {
		
		if(isHeadBang){
			myAnimator.Play ("HeadBang1");	
		}
		else{
			myAnimator.Play ("HeadBang2");	
		}
	}
}
