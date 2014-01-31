using UnityEngine;
using System.Collections;

public class AnimatedTarget : Target 
{
    public override bool OnMirrored(bool submitEffects)
    {
        base.OnMirrored(submitEffects);

        if (submitEffects)
        {
            animation[animation.clip.name].speed *= -1;
        }

        return true;
    }
}
