using UnityEngine;
using System.Collections;

public class StaticWhiteTarget : Target {
    public int correctFilterId = 0;

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    public override bool OnFiltered(int filterId)
    {
        Debug.Log("On Filtererd");
        bool succeeded = filterId == correctFilterId;
        if(succeeded)
        {
            isHarmful = false;
            renderer.enabled = false;
        }
        return succeeded;
    }
}
