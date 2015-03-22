using UnityEngine;
using System.Collections;

public class Crowd : MonoBehaviour {
	
	public Animator myAnimator;

    public bool isHeadBang;
    public string specificAnimation;

	
	void Start () {
        if (specificAnimation == "")
        {
            if (isHeadBang)
            {
                myAnimator.Play("HeadBang1");
            }
            else
            {
                myAnimator.Play("HeadBang2");
            }
        }
        else myAnimator.Play(specificAnimation);
	}
}
