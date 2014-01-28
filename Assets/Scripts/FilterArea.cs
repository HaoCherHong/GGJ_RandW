using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FilterArea : MonoBehaviour {

    static public List<FilterArea> Instances { get{return instances;} }
    static List<FilterArea> instances = new List<FilterArea>();

    public Bounds DetectBounds 
    {
        get 
        {
            if(Application.isPlaying)
                return detectBounds; 
            else
                return new Bounds(new Vector3(transform.position.x, transform.position.y, 0.0f), new Vector3(boundsSize.x, boundsSize.y, 0.0f));
        }
    }

    public Filter passFilters;
    public Vector2 boundsSize;

    Bounds detectBounds;

	// Use this for initialization
	void Start () 
    {
        detectBounds = new Bounds(new Vector3(transform.position.x, transform.position.y, 0.0f), new Vector3(boundsSize.x, boundsSize.y, 0.5f));

        instances.Add(this);
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnDestroy()
    {
        instances.Remove(this);
    }

    protected void OnDrawGizmos()
    {
        Color gizmosColor;
        switch(passFilters)
        {
            case Filter.Red:
                gizmosColor = Color.red;
                break;
            case Filter.Green:
                gizmosColor = Color.green;
                break;
            case Filter.Blue:
                gizmosColor = Color.blue;
                break;
            case Filter.None:
                gizmosColor = Color.magenta;
                break;
            default:
                gizmosColor = Color.black;
                break;
        }
        gizmosColor.a = 0.5f;
        Gizmos.color = gizmosColor;
        Gizmos.DrawCube(DetectBounds.center, DetectBounds.size);

        gizmosColor.a = 1.0f;
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(DetectBounds.center, DetectBounds.size);
    }
}
