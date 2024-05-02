using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonwalk : Tools
{
    public override void Activate()
    {
        PlayerManager player = GetPlayer();

        player.moonWalkDuration = 5f;
        player.moonWalk = true;
    }
}
