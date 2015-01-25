using UnityEngine;
using System.Collections;

public class PlayerNames : MonoBehaviour {

	float alpha = 0;
	bool draw = false;

	void OnLevelWasLoaded () {

		Debug.Log(this.transform.parent.name);
		GetComponent<TextMesh>().text = "P";
		GetComponent<TextMesh>().text += transform.parent.GetComponent<Player>().Id.ToString();
		if (draw) {
			alpha = 1f;
		}
		Color myColor = transform.parent.GetComponent<Player>().MainColor;
		GetComponent<TextMesh>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
	}

	// Use this for initialization
	void Start () {
		transform.position = transform.parent.position;
		transform.Translate(Vector3.up *3);
		draw = true;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(-Camera.main.transform.position);
	}
}
