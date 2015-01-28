using UnityEngine;
using System.Collections;

public class PlayerAIBehaviour : MonoBehaviour {

    protected Player player;
    protected PlayerAI brain;

    private void Awake()
    {
        player = this.GetComponent<Player>();
        brain = this.GetComponent<PlayerAI>();
    }

    public virtual void OnUpdate() { }
}
