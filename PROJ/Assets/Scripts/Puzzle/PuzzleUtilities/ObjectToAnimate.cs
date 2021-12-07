using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToAnimate : ObjectToActivate
{
    [SerializeField] private Animator anim;
    public override void Activate(ActivatorEvent eve)
    {
        if(eve.info.ID == puzzleID)
        {
            anim.SetTrigger("activate");
        }
    }
}
