using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string whichPlayer;
	public float speed;
	public bool isDashing = false; 


	void Start () {
	
	}
	
	void Update () {

		if (Input.GetAxisRaw (whichPlayer+"_Vertical") != 0) {
			this.transform.Translate(Vector3.right * speed * Time.deltaTime * Input.GetAxisRaw (whichPlayer+"_Vertical") , Space.World);
		}
		if (Input.GetAxisRaw (whichPlayer+"_Horizontal") != 0) {
			this.transform.Translate(Vector3.forward * speed * Time.deltaTime * Input.GetAxisRaw (whichPlayer+"_Horizontal") , Space.World);
		}

		if (Input.GetButton (whichPlayer + "_A")) {
			isDashing = true;
		} 
		else {
			isDashing = false;
		}
	}

	public void OnCollisionEnter(Collision other){
		if (other.transform.tag == "Player" && this.isDashing) {

		}
	}
}
