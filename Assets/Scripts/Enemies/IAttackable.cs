using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void Move();
    void Sucked();
    void KnockBack();
    bool SeePlayer();
    void SetDirection(float _direction);

}
