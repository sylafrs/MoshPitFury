using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{

	public GameObject Logo;
	public GameObject NewGameBtn;
	public GameObject CreditBtn;
	public GameObject QuitBtn;
	public GameObject MainMenuBtn;
	public GameObject TeamCode;
	public GameObject TeamGA;
	public GameObject TeamSound;
	public EventSystem myEventSystem;


	public void LoadNewGame()
	{
		Application.LoadLevel((int)SCENE.CharacterSelection);
	}

	public void LoadCredit()
	{
		Logo.SetActive(false);
		NewGameBtn.SetActive(false);
		CreditBtn.SetActive(false);
		QuitBtn.SetActive(false);
		MainMenuBtn.SetActive(true);
		myEventSystem.SetSelectedGameObject(MainMenuBtn);
		TeamGA.SetActive(true);
		TeamCode.SetActive(true);
		TeamSound.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void MainMenu()
	{
		NewGameBtn.SetActive(true);
		myEventSystem.SetSelectedGameObject(NewGameBtn);
		CreditBtn.SetActive(true);
		QuitBtn.SetActive(true);
		MainMenuBtn.SetActive(false);
		TeamGA.SetActive(false);
		TeamCode.SetActive(false);
		TeamSound.SetActive(false);
		Logo.SetActive(true);
	}
}
