using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Tools
{
    public override void Activate()
    {
        PlayerManager player = GetPlayer();
        
        player.speedDuration = 3f;
        player.spedUp = true;
    }
}
