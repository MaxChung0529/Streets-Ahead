using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCelebrateState : IRedState
{

    public void Enter(Red red)
    {
        red.animator.SetTrigger("Celebrate");
    }

    public void Exit(Red red)
    {
        throw new System.NotImplementedException();
    }

    public IRedState Tick()
    {
        return null;
    }
}
