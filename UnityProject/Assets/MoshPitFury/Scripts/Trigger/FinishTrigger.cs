using UnityEngine;
using System.Collections;

public class FinishTrigger : MonoBehaviour {

	public Player Winner = null;

	public void OnTriggerEnter(Collider other){
		if(!Winner)
			Winner = other.GetComponent<Player> ();
	}
}
