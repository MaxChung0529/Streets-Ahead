using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRedState
{
    public IRedState Tick();
    public void Enter();
    public void Exit();

}
