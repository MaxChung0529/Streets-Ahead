using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDeathState : IRedState
{
    public void Enter(Red red)
    {
        red.rb.constraints = RigidbodyConstraints2D.None;
        red.rb.gravityScale *= 10;
        red.boxCollider.enabled = false;
        red.rb.AddForce(Vector3.right * 12, ForceMode2D.Impulse);
        red.animator.SetBool("Die", true);
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
