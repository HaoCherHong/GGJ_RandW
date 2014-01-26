using UnityEngine;
using System.Collections;

public class ObstacleThornTarget : Target
{

    public override bool OnMirrored()
    {
        base.OnMirrored();
        isHarmful = (transform.localScale.x > 0) || (scaleState != ScaleState.ScaledDown);
        return true;
    }
}
