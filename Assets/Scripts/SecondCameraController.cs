using UnityEngine;
using System.Collections;

public class SecondCameraController : MonoBehaviour {
    public enum CameraMode
    {
        None,
        FilterRed,
        FilterGreen,
        FilterBlue,
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

    public SpriteRenderer focusSpriteRenderer;
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
        //targetRenderer.enabled = !Input.GetMouseButton(1);
	}

    void OnGUI()
    {
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(DetectBounds.center, DetectBounds.size);
    }

    void OnModeChanged()
    {
        targetRenderer.enabled = CurrentMode != CameraMode.None;

        if (CurrentMode == CameraMode.Blur)
        {
            GetComponent<Blur>().enabled = true;
            characterController.SetState(GameCharacterController.CharacterState.Normal);
        }
        else
            GetComponent<Blur>().enabled = false;

        bool filterMode = CurrentMode >= CameraMode.FilterRed && CurrentMode <= CameraMode.FilterBlue;
        ColorCorrectionCurves curves = GetComponent<ColorCorrectionCurves>();
        if (filterMode)
        {
            
            curves.redChannel = CurrentMode == CameraMode.FilterRed ? normalCurve : zeroCurve;
            curves.greenChannel = CurrentMode == CameraMode.FilterGreen ? normalCurve : zeroCurve;
            curves.blueChannel = CurrentMode == CameraMode.FilterBlue ? normalCurve : zeroCurve;
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

    public void SetFocus(bool focus)
    {
        Color newColor = focus ? Color.green : Color.white;
        newColor.a = 0.75f;
        focusSpriteRenderer.color = newColor;
    }
}
