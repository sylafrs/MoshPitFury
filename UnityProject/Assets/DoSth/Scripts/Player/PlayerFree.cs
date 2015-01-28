using UnityEngine;
using System.Collections;

public class PlayerFree : MonoBehaviour
{

	void Awake()
	{
		Player p = this.GetComponent<Player>();
		p.Init();
		p.Prepare();
		p.StartGame();
	}
}
