using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour 
{
    public enum ScaleState
    {
        Normal,
        ScaledUp,
        ScaledDown
    }
    
    public bool isScalable = true;
    public bool isHarmful = true;

    public Vector3 upScale = new Vector3(2f,2f,2f);
    public Vector3 downScale = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 normalScale;

    public ScaleState scaleState = ScaleState.Normal;

    protected void Start()
    {
        normalScale = transform.localScale;
    }

    public virtual bool OnFiltered(int filterId)
    {
        return false;
    }
    public virtual bool OnBlured()
    {
        return false;
    }
    public virtual bool OnFastCaptured()
    {
        return false;
    }
    public virtual bool OnSlowCaptured()
    {
        return false;
    }
    public virtual bool OnMirrored()
    {
        return false;
    }

    public virtual bool OnScaledUp()
    {
        if (isScalable)
        {
            switch(scaleState)
            {
                case ScaleState.Normal:
                    transform.localScale = upScale;
                    scaleState = ScaleState.ScaledUp;
                    break;
                case ScaleState.ScaledUp:
                    return false;
                case ScaleState.ScaledDown:
                    transform.localScale = normalScale;
                    scaleState = ScaleState.Normal;
                    break;
            }
            return true;
        }
        else
            return false;
    }

    public virtual bool OnScaledDown()
    {
        if (isScalable)
        {
            switch (scaleState)
            {
                case ScaleState.Normal:
                    transform.localScale = downScale;
                    scaleState = ScaleState.ScaledDown;
                    break;
                case ScaleState.ScaledUp:
                    transform.localScale = normalScale;
                    scaleState = ScaleState.Normal;
                    break;
                case ScaleState.ScaledDown:
                    return false;
            }
            return true;
        }
        else
            return false;
    }
}