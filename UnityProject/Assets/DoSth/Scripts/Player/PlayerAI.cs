using UnityEngine;
using System.Collections;

public class PlayerAI : PlayerBrain {


    public override bool WantToDash
    {
        get { return false; }
    }

    public override Vector3 WantedDirection
    {
        get { return Vector3.zero; }
    }
}
