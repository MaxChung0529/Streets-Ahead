using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPatrolState : IRedState
{
    private Red _red;

    public void Enter(Red red)
    {
        _red = red;
        _red.SlowDown();
    }

    public IRedState Tick()
    {
        _red.MoveInDir();   
        if (!GameObject.Find("Player").GetComponent<PlayerManager>().alive)
        {
            return new RedCelebrateState();
        }
        if (_red.dead)
        {
            return new RedDeathState();
        }
        if (_red.SeePlayer())
        {
            return new RedChasingState();
        }

        return null;
    }

    public void Exit(Red red)
    {
        throw new System.NotImplementedException();
    }
}
