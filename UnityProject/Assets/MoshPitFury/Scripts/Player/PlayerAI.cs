using UnityEngine;
using System.Collections;

public class PlayerAI : PlayerBrain
{
	protected PlayerAIBehaviour behaviour;

	public override bool WantToDash
	{
		get { return false; }
	}

	public override Vector3 WantedDirection
	{
		get {
			if (!behaviour)
				return Vector3.zero;
			return behaviour.WantedDirection; 
		}
	}

	void OnPlayerPrepare()
	{
		this.SwitchBehaviour<PlayerAIBehaviourIdle>();
	}
	
	public T SwitchBehaviour<T>() where T:PlayerAIBehaviour, new()
	{
		if (this.behaviour)
			GameObject.Destroy(this.behaviour);
		T behaviour = this.gameObject.AddComponent<T>();
		this.behaviour = behaviour;
		return behaviour;
	}
	
	public override void UpdateState()
	{
		if (this.behaviour)
			this.behaviour.OnUpdate();
	}
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