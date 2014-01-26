using UnityEngine;
using System.Collections;

public class ToBlackTarget : Target 
{
    public int incorrectFilterId = -1;
    public override bool OnFiltered(int filterId)
    {
        Debug.Log("On Filtererd");
        bool succeeded = filterId != incorrectFilterId;
        if (succeeded)
        {
            isHarmful = false;
            renderer.enabled = false;
        }
        return succeeded;
    }
}
