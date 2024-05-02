using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield: Tools
{
    public override void Activate()
    {
        PlayerManager player = GetPlayer();

        player.shielded = true;
        player.trail.enabled = true;
    }
}
