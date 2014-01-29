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
    public Bounds DetectBounds
    {
        get
        {
            return new Bounds(new Vector3(transform.position.x, transform.position.y, 0),
                new Vector3(detectArea.x, detectArea.y, 0.0f));
        }
    }

    public float fishEyeStrength = 0.5f;
    public float sizeScale = 2.0f;
    public GameCharacterController characterController;
    public Renderer targetRenderer;
    public Vector2 detectArea;

    CameraMode currentMode;

    AnimationCurve normalCurve;
    AnimationCurve zeroCurve;

    Texture2D blackTexture;
    RenderTexture cameraTexture;

    //float cameraOrthographicSize;
    float cameraFieldOfView;
	// Use this for initialization
	void Start () 
    {
        cameraTexture = new RenderTexture(
            512,
            512 * Screen.height / Screen.width,
            24, RenderTextureFormat.ARGBFloat);
        camera.targetTexture = cameraTexture;

        targetRenderer.material.mainTexture = cameraTexture;
        targetRenderer.material.mainTextureScale = new Vector2((float)Screen.height / Screen.width, 1.0f);
        targetRenderer.material.mainTextureOffset = new Vector2((float)Screen.height / Screen.width / 2, 0.0f);
        
       

        //cameraOrthographicSize = camera.orthographicSize;
        cameraFieldOfView = camera.fieldOfView;
        normalCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        zeroCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        OnModeChanged();

        
	}
	
	// Update is called once per frame
	void Update () 
    {
        targetRenderer.enabled = !Input.GetMouseButton(1);
	}

    void OnGUI()
    {
        return;
        Bounds detectBounds = DetectBounds;
        Vector3 bottomLeft = Camera.main.WorldToScreenPoint(detectBounds.center - detectBounds.size / 2);
        Vector3 topRight = Camera.main.WorldToScreenPoint(detectBounds.center + detectBounds.size / 2);
        Rect drawingRect = new Rect(bottomLeft.x,
            Screen.height - topRight.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
        GUI.DrawTexture(drawingRect, cameraTexture, ScaleMode.ScaleAndCrop);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(DetectBounds.center, DetectBounds.size);
    }

    void OnModeChanged()
    {
        if (CurrentMode == CameraMode.Blur)
        {
            GetComponent<Blur>().enabled = true;
            characterController.SetState(GameCharacterController.CharacterState.Normal);
        }
        else
            GetComponent<Blur>().enabled = false;

        bool filterMode = CurrentMode >= CameraMode.Filter1 && CurrentMode <= CameraMode.Filter3;
        ColorCorrectionCurves curves = GetComponent<ColorCorrectionCurves>();
        if (filterMode)
        {
            
            curves.redChannel = CurrentMode == CameraMode.Filter1 ? normalCurve : zeroCurve;
            curves.greenChannel = CurrentMode == CameraMode.Filter2 ? normalCurve : zeroCurve;
            curves.blueChannel = CurrentMode == CameraMode.Filter3 ? normalCurve : zeroCurve;
            curves.UpdateParameters();

            characterController.SetState(GameCharacterController.CharacterState.Normal);
        }
        curves.enabled = filterMode;

        bool scaleMode = CurrentMode >= CameraMode.ScaleUp && CurrentMode <= CameraMode.ScaleDown;
        Fisheye fishEye = GetComponent<Fisheye>();
        if (scaleMode)
        {
            fishEye.strengthX = CurrentMode == CameraMode.ScaleUp ? fishEyeStrength : -fishEyeStrength;
            fishEye.strengthY = CurrentMode == CameraMode.ScaleUp ? fishEyeStrength : -fishEyeStrength;
            //camera.orthographicSize = CurrentMode == CameraMode.ScaleUp ? cameraOrthographicSize / sizeScale : cameraOrthographicSize * sizeScale;
            camera.fieldOfView = CurrentMode == CameraMode.ScaleUp ? cameraFieldOfView / sizeScale : cameraFieldOfView * sizeScale;

            characterController.SetState(CurrentMode == CameraMode.ScaleUp ? GameCharacterController.CharacterState.ScaleUp : GameCharacterController.CharacterState.ScaleDown);
        }
        else
        {
            //camera.orthographicSize = cameraOrthographicSize;
            camera.fieldOfView = cameraFieldOfView;
        }
        fishEye.enabled = scaleMode;

        
        Vector3 newScale = targetRenderer.transform.localScale;
        if(CurrentMode == CameraMode.Mirror)
        {
            newScale.x = -Mathf.Abs(newScale.x);
            characterController.SetState(GameCharacterController.CharacterState.Normal);
        }
        else
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        targetRenderer.transform.localScale = newScale;
        

        GetComponent<MotionBlur>().enabled = CurrentMode == CameraMode.ShutterSlow;
    }
}
