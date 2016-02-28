using UnityEngine;
using System.Collections;

public abstract class PlayerAIBehaviour : MonoBehaviour
{
	protected Player player;
	protected PlayerAI brain;

	private void Awake()
	{
		player = this.GetComponent<Player>();
		brain = this.GetComponent<PlayerAI>();
	}

	public abstract Vector3 WantedDirection { get; }

	public virtual void OnUpdate() { }
}

// Mode prédateur :
// ==============
// - Poursuivre et dasher autres persos dans le feu
// 
// Mode proie :
// ===========
// - Esquiver les autres joueurs
// - Les dasher pour les repousser si c'est opportun
// 
// Règle suicide :
// -------------------
// - Avancer vers le feu le plus proche
// - Dasher de façon random
// 
// Règle don't move :
// -----------------------
// - Random sur le fait de bouger ou non
// - Une touche du clavier permet de le faire bouger ?
// 
// Règle survival :
// -------------------
// - Se déplace à un endroit random