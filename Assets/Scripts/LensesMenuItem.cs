using UnityEngine;
using System.Collections;

public class LensesMenuItem : MonoBehaviour {
    public LensesMenu menu;

    public SecondCameraController.CameraMode mode;
    public bool isParent;
    public LensesMenuItem[] subItems;

    public bool visible = false;
    Vector3 localScaleCurrentVelocity;
    float targetScale = 1.0f;

    bool isSelected = false;
	// Use this for initialization
	void Start () 
    {
	    if(isParent)
        {
            foreach (LensesMenuItem item in subItems)
                item.visible = false;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one * targetScale * (visible ? 1.0f : 0.0f), ref localScaleCurrentVelocity, 0.05f);
	}

    

    void OnMouseEnter()
    {
        if (isParent)
        {
            menu.OnParentSelected(this);

            foreach (LensesMenuItem item in subItems)
                item.visible = true;

            
        }

        if (mode != SecondCameraController.CameraMode.None)
            menu.OnModeSelected(mode);

        targetScale = 1.2f;
    }

    void OnMouseExit()
    {

        targetScale = 1.0f;
    }

    public void OnMenuOpened()
    {
        visible = true;
    }

    public void OnMenuClosed()
    {
        visible = false;
        if (isParent)
        {
            foreach (LensesMenuItem item in subItems)
                item.visible = false;
        }
    }
}
