using UnityEngine;
using System.Collections;

public class ImpactingTarget : Target {

    public Vector3 translation = new Vector3(-1, 0, 0);
    bool movingStarted = false;

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


    public override bool OnMirrored()
    {
        base.OnMirrored();
        translation.x *= -1;
        return true;
    }
}
