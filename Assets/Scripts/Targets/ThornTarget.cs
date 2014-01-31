using UnityEngine;
using System.Collections;

public class ThornTarget : Target {

    public override bool OnMirrored(bool submitEffects)
    {
        base.OnMirrored(submitEffects);
        if (submitEffects)
        {
            isHarmful = transform.localScale.x > 0;
        }
        return true;
    }
}
