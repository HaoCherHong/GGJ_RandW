using UnityEngine;
using System.Collections;

public class HighSpeedTarget : Target 
{
    public LensFlare[] components;

    public override bool OnFastCaptured(bool submitEffects)
    {
        if (submitEffects)
        {
            animation.Stop();

            foreach (LensFlare component in components)
                component.enabled = false;
        }
        return true;
    }
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
