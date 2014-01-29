using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Target : MonoBehaviour 
{
    public enum ScaleState
    {
        Normal,
        ScaledUp,
        ScaledDown
    }
    public Bounds DetectBounds
    {
        get
        {
            if (setDetectBoundsManually)
                return new Bounds(new Vector3(transform.position.x, transform.position.y, 0f), new Vector3(manualDetectBoundsSize.x, manualDetectBoundsSize.y, 0));

            Bounds result = new Bounds(transform.position, Vector3.zero);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach(Renderer renderer in renderers)
                result.Encapsulate(renderer.bounds);
            result.center = new Vector3(result.center.x, result.center.y, 0.0f);
            return result;
        }
    }

    
    public bool isScalable = true;
    public bool isHarmful = true;
    public bool isMirrorable = true;
    public bool setDetectBoundsManually = false;

    public Vector3 upScale = new Vector3(1.5f,1.5f,1.5f);
    public Vector3 downScale = new Vector3(0.75f, 0.75f, 0.75f);
    public Vector2 manualDetectBoundsSize = Vector2.one;

    Vector3 normalScale;

    public ScaleState scaleState = ScaleState.Normal;
    public static List<Target> instances = new List<Target>();

    protected void Start()
    {
        normalScale = transform.localScale;

        instances.Add(this);
    }

    protected void OnDestroy()
    {
        instances.Remove(this);
    }
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(DetectBounds.center, DetectBounds.size);
    }

    public virtual bool OnFiltered(Filter filter)
    {
        Debug.Log("On Filtererd");

        bool result = false;
        var filterAreas = FilterArea.Instances;
        foreach(FilterArea filterArea in filterAreas)
        {
            //if(filterArea.DetectBounds.Intersects(DetectBounds))
            if(DetectionCommon.ContainmentTest(filterArea.DetectBounds, DetectBounds))
            {
                if((filterArea.passFilters & filter) != Filter.None)
                {
                    result = true;
                }
            }
        }

        if(result)
        {
            isHarmful = false;
            renderer.enabled = false;
        }

        return result;
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
        
        if (isMirrorable)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            Debug.Log("On Mirrored");
            return true;
        }
        else
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
        Debug.Log("Scaled Down");
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