using UnityEngine;
using System.Collections;

public class ThornTarget : Target {

    public override bool OnMirrored()
    {
        base.OnMirrored();
        isHarmful = false;
        return true;
    }
}
