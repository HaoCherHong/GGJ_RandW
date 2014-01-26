using UnityEngine;
using System.Collections;

public class AnimatedTarget : Target 
{
    public override bool OnMirrored()
    {
        base.OnMirrored();

        animation[animation.clip.name].speed *= -1;

        return true;
    }
}
