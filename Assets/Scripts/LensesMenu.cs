using UnityEngine;
using System.Collections;

public class LensesMenu : MonoBehaviour 
{
    public GameController gameController;
    public LensesMenuItem[] subItems;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        //Update Position
        Plane plane = new Plane(-Vector3.forward, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        plane.Raycast(ray, out rayDistance);
        Vector3 hit = ray.GetPoint(rayDistance);
        transform.position = hit;

        foreach (LensesMenuItem item in subItems)
            item.OnMenuOpened();
    }

    public void Close()
    {
        foreach (LensesMenuItem item in subItems)
            item.OnMenuClosed();
    }

    public void OnParentSelected(LensesMenuItem selectedItem)
    {
        foreach (LensesMenuItem item in subItems)
            if (item != selectedItem)
                item.visible = false;
    }

    public void OnModeSelected(SecondCameraController.CameraMode mode)
    {
        gameController.SetCameraMode(mode);
    }
}
