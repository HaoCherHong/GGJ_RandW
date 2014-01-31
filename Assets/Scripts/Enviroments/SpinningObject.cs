using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

    public Vector3 translation;
    public Vector3 rotation;

    bool visible = false;

    

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(visible)
        {
            transform.Translate(translation * Time.deltaTime, Space.World);
            transform.Rotate(rotation * Time.deltaTime, Space.Self);
        }
	}

    void OnBecameVisible()
    {
        visible = true;
    }

    void OnBecameInvisible()
    {
        visible = false;
    }
}
