using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tools
{
    public abstract void Activate();

    protected PlayerManager GetPlayer()
    {
        return PlayerManager.instance;
    }

    protected float GetDirection()
    {
        return PlayerManager.instance.rb.transform.localScale.x;
    }
}
