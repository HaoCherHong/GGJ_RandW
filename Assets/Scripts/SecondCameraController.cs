using UnityEngine;
using System.Collections;

public class SecondCameraController : MonoBehaviour {
    public enum CameraMode
    {
        None,
        Filter1,
        Filter2,
        Filter3,
        Blur,
        ScaleUp,
        ScaleDown,
        ShutterFast,
        ShutterSlow,
        Mirror
    }
    public CameraMode CurrentMode
    {
        get { return currentMode; }
        set
        {
            currentMode = value;
            OnModeChanged();
        }
    }

    public float sizeScale = 2.0f;
    public Renderer targetRenderer;

    CameraMode currentMode;

    AnimationCurve normalCurve;
    AnimationCurve zeroCurve;

    float cameraOrthographicSize;
	// Use this for initialization
	void Start () 
    {
        cameraOrthographicSize = camera.orthographicSize;
        normalCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        zeroCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        OnModeChanged();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        
    }

    void OnModeChanged()
    {
        Debug.Log(currentMode.ToString());
        GetComponent<Blur>().enabled = CurrentMode == CameraMode.Blur;

        bool filterMode = CurrentMode >= CameraMode.Filter1 && CurrentMode <= CameraMode.Filter3;
        ColorCorrectionCurves curves = GetComponent<ColorCorrectionCurves>();
        if (filterMode)
        {
            
            curves.redChannel = CurrentMode == CameraMode.Filter1 ? normalCurve : zeroCurve;
            curves.greenChannel = CurrentMode == CameraMode.Filter2 ? normalCurve : zeroCurve;
            curves.blueChannel = CurrentMode == CameraMode.Filter3 ? normalCurve : zeroCurve;
            curves.UpdateParameters();
        }
        curves.enabled = filterMode;

        bool scaleMode = CurrentMode >= CameraMode.ScaleUp && CurrentMode <= CameraMode.ScaleDown;
        Fisheye fishEye = GetComponent<Fisheye>();
        if(scaleMode)
        {
            fishEye.strengthX = CurrentMode == CameraMode.ScaleUp ? 1f : -1f;
            fishEye.strengthY = CurrentMode == CameraMode.ScaleUp ? 1f : -1f;
            camera.orthographicSize = CurrentMode == CameraMode.ScaleUp ? cameraOrthographicSize / sizeScale : cameraOrthographicSize * sizeScale;
        }
        else
            camera.orthographicSize = cameraOrthographicSize;
        fishEye.enabled = scaleMode;

        Vector3 newScale = targetRenderer.transform.localScale;
        if(CurrentMode == CameraMode.Mirror)
        {
            newScale.x = -Mathf.Abs(newScale.x);
        }
        else
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        targetRenderer.transform.localScale = newScale;
    }
}
