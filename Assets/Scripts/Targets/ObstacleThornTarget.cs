using UnityEngine;
using System.Collections;

public class ObstacleThornTarget : Target
{

    public override bool OnMirrored(bool submitEffects)
    {
        base.OnMirrored(submitEffects);
        if (submitEffects)
        {
            isHarmful = (transform.localScale.x > 0) || (scaleState != ScaleState.ScaledDown);
        }
        return true;
    }
}
