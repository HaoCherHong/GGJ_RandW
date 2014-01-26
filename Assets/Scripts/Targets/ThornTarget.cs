﻿using UnityEngine;
using System.Collections;

public class ThornTarget : Target {

    public override bool OnMirrored()
    {
        base.OnMirrored();
        isHarmful = transform.localScale.x > 0;
        return true;
    }
}
