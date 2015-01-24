using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string whichPlayer;
	public float speed;
	public bool isDashing = false; 
	Vector3 dir;


	void Start () {
	
	}
	
	void Update () {

		Debug.DrawRay (this.transform.position, this.transform.forward, Color.green);

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
		if (other.transform.tag == "Player" && this.isDashing && !other.transform.GetComponent<PlayerController>().isDashing) {
			CalculateDirection(other.transform.position);
		}
	}
	void CalculateDirection(Vector3 TargetPos){
		dir = TargetPos - this.transform.position;
		return;                           
	}
}
