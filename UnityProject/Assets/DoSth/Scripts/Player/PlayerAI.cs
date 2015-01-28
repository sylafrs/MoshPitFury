using UnityEngine;
using System.Collections;

public class PlayerAI : PlayerBrain 
{

    protected NavMeshAgent nma;

    public override bool WantToDash
    {
        get { return false; }
    }

    public override Vector3 WantedDirection
    {
        get { return Vector3.zero; }
    }

    protected override void Awake()
    {
        base.Awake();
        nma = this.GetComponent<NavMeshAgent>();
        if (nma == null)
            nma = this.gameObject.AddComponent<NavMeshAgent>();
        nma.enabled = true;
    }
}
