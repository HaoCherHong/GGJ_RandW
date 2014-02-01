using UnityEngine;
using System.Collections;

public class ImpactingTarget : Target {

    public Vector3 translation = new Vector3(-1, 0, 0);
    public bool movingStarted = false;

    void Update()
    {
        if(movingStarted)
        {
            transform.Translate(translation * Time.deltaTime);
        }
    }

    void OnBecameVisible()
    {
        movingStarted = true;
    }


    public override bool OnMirrored(bool submitEffects)
    {
        base.OnMirrored(submitEffects);
        if (submitEffects)
        {
            translation.x *= -1;
        }
        return true;
    }

    public override bool OnFastCaptured(bool submitEffects)
    {
        base.OnFastCaptured(submitEffects);

        if(submitEffects)
        {
            translation = Vector3.zero;
        }

        return true;
    }
}
