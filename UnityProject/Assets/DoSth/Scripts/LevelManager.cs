using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public Text CountDown;
	public int RuleNumber;
	public Text CurrentRule;
	GameObject CurrentLevel;

	public List<GameObject> ListLevel = new List<GameObject>();

	void Start () {
		StartCoroutine (CountDownStart ());
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine(NextLevel());
		}
	
	}

	public IEnumerator CountDownStart(){
		CountDown.text = "Ready?";
		yield return new WaitForSeconds(3f);
		CountDown.text = "3";
		yield return new WaitForSeconds(1f);
		CountDown.text = "2";
		yield return new WaitForSeconds(1f);
		CountDown.text = "1";
		yield return new WaitForSeconds(1f);
		CountDown.text = "GO!";
		yield return new WaitForSeconds(1f);
		CountDown.enabled = false;
		RuleNumber = Random.Range (0, 3);
		RuleDisplayer (RuleNumber);
		yield return new WaitForSeconds (2f);
		CurrentRule.enabled = false;

	}

	public void RuleDisplayer(int rule){
		if (rule == 0) {
			GameObject level;
			level = Instantiate(ListLevel[rule].gameObject, new Vector3(0, -3f, 0), Quaternion.identity) as GameObject; 
			CurrentRule.text = "DEATHMATCH";
			CurrentLevel = level;
		}
		if (rule == 1) {
			GameObject level;
			level = Instantiate(ListLevel[rule].gameObject, new Vector3(0, -3f, 0), Quaternion.identity) as GameObject; 
			CurrentRule.text = "GET TO THE FLAG";
			CurrentLevel = level;

		}
		if (rule == 2) {
			GameObject level;
			level = Instantiate(ListLevel[rule].gameObject, new Vector3(0, -3f, 0), Quaternion.identity) as GameObject; 
			CurrentRule.text = "KILL PLAYER 2";
			CurrentLevel = level;
		}
	}

	public IEnumerator NextLevel(){
		Destroy (CurrentLevel);
		CountDown.enabled = true;
		CountDown.text = "3";
		yield return new WaitForSeconds(1f);
		CountDown.text = "2";
		yield return new WaitForSeconds(1f);
		CountDown.text = "1";
		yield return new WaitForSeconds(1f);
		CountDown.text = "GO!";
		yield return new WaitForSeconds(1f);
		CountDown.enabled = false;
		RuleNumber = Random.Range (0, 2);
		CurrentRule.enabled = true;
		RuleDisplayer (RuleNumber);
		yield return new WaitForSeconds (2f);
		CurrentRule.enabled = false;
	}
}
