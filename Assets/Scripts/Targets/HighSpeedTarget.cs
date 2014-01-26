using UnityEngine;
using System.Collections;

public class HighSpeedTarget : Target 
{
    public LensFlare[] components;

    public override bool OnFastCaptured()
    {
        animation.Stop();

        foreach (LensFlare component in components)
            component.enabled = false;

        return true;
    }
    public override bool OnMirrored()
    {
        base.OnMirrored();

        animation[animation.clip.name].speed *= -1;

        return true;
    }

}
