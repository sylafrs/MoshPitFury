using UnityEngine;
using System.Collections;

public abstract class PlayerBrain : MonoBehaviour
{
    protected Player player;

    protected virtual void Awake()
    {
        player = this.GetComponent<Player>();
    }


    public abstract bool WantToDash { get; }

    public abstract Vector3 WantedDirection { get; }

    public virtual void UpdateState() { }
}
