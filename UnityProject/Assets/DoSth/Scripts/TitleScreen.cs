using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour {

	public GameObject NewGameBtn;
	public GameObject CreditBtn;
	public GameObject QuitBtn;
	public GameObject MainMenuBtn;
	public GameObject TeamCode;
	public GameObject TeamGA;
	public GameObject TeamSound;
	public EventSystem myEventSystem;

	void Start () {
	
	}
	
	void Update () {

	}

	public void LoadNewGame(){
		Application.LoadLevel ("CharacterSelection");
	}

	public void LoadCredit(){
		NewGameBtn.SetActive (false);
		CreditBtn.SetActive (false);
		QuitBtn.SetActive (false);
		MainMenuBtn.SetActive (true);
		myEventSystem.SetSelectedGameObject(MainMenuBtn);
		TeamGA.SetActive (true);
		TeamCode.SetActive (true);
		TeamSound.SetActive (true);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void MainMenu(){
		NewGameBtn.SetActive (true);
		myEventSystem.SetSelectedGameObject(NewGameBtn);
		CreditBtn.SetActive (true);
		QuitBtn.SetActive (true);
		MainMenuBtn.SetActive (false);
		TeamGA.SetActive (false);
		TeamCode.SetActive (false);
		TeamSound.SetActive (false);
	}
}
