using UnityEngine;
using System.Collections;

public class ColoredBackground : MonoBehaviour {
    public int filterId = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        Target target = other.GetComponentInChildren<Target>();
        if(target)
        {
            target.correctFilterId = filterId;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Target target = other.GetComponentInChildren<Target>();
        if (target)
        {
            target.correctFilterId = -1;
        }
    }
}
