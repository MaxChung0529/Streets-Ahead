using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRedState
{
    public IRedState Tick();
    public void Enter(Red red);
    public void Exit(Red red);

}
