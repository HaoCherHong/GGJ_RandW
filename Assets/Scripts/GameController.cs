using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    public static GameController Instance { get { return instance; } }
    static GameController instance;
    public LensesMenu lensesMenu;
    public Camera secondCamera;
    public GameCharacterController characterController;
	public Rigidbody2D characterRigidbody;
	public float speed = 1.0f;
    public Renderer secondCameraRenderer;
    public float levelEnd = 1000f;
    public int nextLevel = 1;

    //GUI
    public GUISkin emptyGUISkin;
    public Texture2D gameOverTexture;
    public Texture2D restartButtonTexture;

    //SFX
    public AudioClip focusClip;
    public AudioClip shutterClip;

    Rect screenRect;

    Plane worldPlane;

    public List<Target> Targets
    {
        get { return Target.instances; }
    }
    SecondCameraController secondCameraController;
    Texture2D shutterTexture;
    Texture2D blackTexture;
    

    bool isShotAvailable = false;

    float lastShutterTime = 0.0f;
    float shutterTime = 0.1f;

    bool isGamePaused = false;
    bool isGameOver = false;
    bool returnToStart = false;
    float gameOverTime = 0.0f;
    float lastLeftMouseDown = 0.0f;

    bool isLastFrameFocused = false;

	// Use this for initialization
	void Start () {
        instance = this;

        worldPlane = new Plane(-Vector3.forward, 0);

        secondCameraController = secondCamera.GetComponent<SecondCameraController>();

        shutterTexture = new Texture2D(1, 1);
        blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        Update2ndCamera(); 

        if (!isGamePaused)
        {
            characterRigidbody.transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
        }
        if (characterRigidbody.transform.position.x >= levelEnd)
        {
            if (nextLevel >= 0)
                Application.LoadLevel(nextLevel);
            else
                OnGameOver(true);
        }

        bool focus = false;
        foreach (Target target in Targets)
        {
            if (!target.enabled)
                continue;
            if(DetectionCommon.ContainmentTest(secondCameraController.DetectBounds, target.DetectBounds))
            {
                if (TestEffect(target, secondCameraController.CurrentMode))
                {
                    focus = true;
                    break;
                }
            }
        }
        secondCameraController.SetFocus(focus);

        if (!isLastFrameFocused && focus)
            audio.PlayOneShot(focusClip);

        isLastFrameFocused = focus;

        if (Input.GetMouseButtonDown(0))
            lastLeftMouseDown = Time.time;
        if (Input.GetMouseButtonUp(0) && Time.time - lastLeftMouseDown <= 0.25f)
        {
            TryShot();

            audio.PlayOneShot(shutterClip);
        }
        //Manage Lenses Menu
        if (Input.GetMouseButton(0) && Time.time - lastLeftMouseDown > 0.1f)
        {
            //Open Lenses Menu
            if(!lensesMenu.IsOpened)
                lensesMenu.Open();
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(lensesMenu.IsOpened)
                lensesMenu.Close();
        }
	}
    void OnDestroy()
    {

    }

    

    void OnGUI()
    {
        if (!isGameOver)
        {
            //Draw Shutter
            if (Time.time <= lastShutterTime + shutterTime + 0.2f)
            {
                float lerp = Mathf.InverseLerp(lastShutterTime + shutterTime, lastShutterTime + shutterTime + 0.2f, Time.time);
                shutterTexture.SetPixel(0, 0, new Color(0, 0, 0, Mathf.Lerp(1.0f, 0f, lerp)));
                shutterTexture.Apply();
                Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
                GUI.DrawTexture(screenRect, shutterTexture);
            }
        }
        else
        {
            GUI.skin = emptyGUISkin;
            GUI.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, Mathf.InverseLerp(gameOverTime, gameOverTime + 2.0f, Time.time));
            GUI.DrawTexture(screenRect, blackTexture);

            GUI.DrawTexture(new Rect(Screen.width / 2 - gameOverTexture.width / 2, Screen.height / 2 - gameOverTexture.height, gameOverTexture.width, gameOverTexture.height), gameOverTexture);

            if(GUI.Button(new Rect(Screen.width / 2 - restartButtonTexture.width / 2, Screen.height / 2, restartButtonTexture.width, restartButtonTexture.height), restartButtonTexture))
            {
                if(returnToStart)
                    Application.LoadLevel(0);
                else
                    Application.LoadLevel(Application.loadedLevel);
            }

        }
    }

    void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Debug.Log("Toggled Pause");
        /*
        if(isGamePaused)
        {
            foreach (Target target in Targets)
                target.enabled = false;
        }
        else
        {
            foreach (Target target in Targets)
                target.enabled = true;
        }*/
    }

    void Update2ndCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (worldPlane.Raycast(ray, out rayDistance))
        {
            Vector3 hit = ray.GetPoint(rayDistance);
            secondCamera.transform.position = new Vector3(hit.x, hit.y, secondCamera.transform.position.z);
        }
    }

    void TryShot()
    {
        Debug.Log("Shot");
        lastShutterTime = Time.time;

        //Select shutter time
        switch(secondCameraController.CurrentMode)
        {
            case SecondCameraController.CameraMode.ShutterFast:
                shutterTime = 0.0f;
            break;
            case SecondCameraController.CameraMode.ShutterSlow:
                shutterTime = 0.5f;
            break;
            default:
                shutterTime = 0.1f;
            break;
        }

        foreach (Target target in Targets)
        {
            if (!target.enabled)
                continue;
            //bool detected = secondCameraController.DetectBounds.Intersects(target.DetectBounds);
            bool detected = DetectionCommon.ContainmentTest(secondCameraController.DetectBounds, target.DetectBounds);
            if (detected)
            {
                Debug.Log("Testing " + target.gameObject.name + ", " + detected);
                SubmitEffect(target, secondCameraController.CurrentMode);
            }
        }
        /*
        foreach (GameObject targetGO in targets)
        {
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(secondCamera), targetGO.GetComponentInChildren<Renderer>().bounds))
            {
                Target target = targetGO.GetComponent<Target>();
                if (target)
                    Shot(target.GetComponent<Target>());
            }
        }
        */

        secondCameraController.CurrentMode = SecondCameraController.CameraMode.None;
        characterController.Shot();
    }

    void SubmitEffect(Target target, SecondCameraController.CameraMode mode)
    {

        switch (mode)
        {
            case SecondCameraController.CameraMode.FilterRed:
                if(target.OnFiltered(true, Filter.Red))
                {

                }
                break;
            case SecondCameraController.CameraMode.FilterGreen:
                if (target.OnFiltered(true, Filter.Green))
                {

                }
                break;
            case SecondCameraController.CameraMode.FilterBlue:
                if (target.OnFiltered(true, Filter.Blue))
                {

                }
                break;
            case SecondCameraController.CameraMode.ScaleUp:
                if (target.OnScaledUp(true))
                {

                }
                break;
            case SecondCameraController.CameraMode.ScaleDown:
                if (target.OnScaledDown(true))
                {

                }
                break;
            case SecondCameraController.CameraMode.Blur:
                if (target.OnBlured(true))
                {

                }
                break;
            case SecondCameraController.CameraMode.Mirror:
                if (target.OnMirrored(true))
                {

                }
                break;
            case SecondCameraController.CameraMode.ShutterFast:
                if (target.OnFastCaptured(true))
                {

                }
                break;
            case SecondCameraController.CameraMode.ShutterSlow:
                if (target.OnSlowCaptured(true))
                {

                }
                break;
        }
        
    }

    bool TestEffect(Target target, SecondCameraController.CameraMode mode)
    {

        switch (mode)
        {
            case SecondCameraController.CameraMode.FilterRed:
                return target.OnFiltered(false, Filter.Red);
            case SecondCameraController.CameraMode.FilterGreen:
                return target.OnFiltered(false, Filter.Green);
            case SecondCameraController.CameraMode.FilterBlue:
                return target.OnFiltered(false, Filter.Blue);
            case SecondCameraController.CameraMode.ScaleUp:
                return target.OnScaledUp(false);
            case SecondCameraController.CameraMode.ScaleDown:
                return target.OnScaledDown(false);
            case SecondCameraController.CameraMode.Blur:
                return target.OnBlured(false);
            case SecondCameraController.CameraMode.Mirror:
                return target.OnMirrored(false);
            case SecondCameraController.CameraMode.ShutterFast:
                return target.OnFastCaptured(false);
            case SecondCameraController.CameraMode.ShutterSlow:
                return target.OnSlowCaptured(false);
            default:
                return false;
        }

    }

    public void OnGameOver()
    {
        OnGameOver(false);
    }

    public void OnGameOver(bool returnToStart)
    {
        if (!isGameOver)
        {
            this.returnToStart = returnToStart;
            isGameOver = true;
            gameOverTime = Time.time;
        }
    }

    public void SetCameraMode(SecondCameraController.CameraMode mode)
    {
        secondCameraController.CurrentMode = mode;
    }

    public static void OnPlayerHarmed()
    {
        Debug.Log("Player Harmed");
        Instance.OnGameOver();
    }
    
}
