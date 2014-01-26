using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    public static GameController Instance { get { return instance; } }
    static GameController instance;
    

    public Camera secondCamera;
    public GameCharacterController characterController;
	public Rigidbody2D characterRigidbody;
	public float speed = 1.0f;
    public RenderTexture cameraTexture;
    public Renderer secondCameraRenderer;
    public float levelEnd = 1000f;
    public int nextLevel = 1;

    //GUI
    public GUISkin emptyGUISkin;
    public Texture2D gameOverTexture;
    public Texture2D restartButtonTexture;
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
    float shutterTime = 0.2f;

    bool isGamePaused = false;
    bool isGameOver = false;
    bool returnToStart = false;
    float gameOverTime = 0.0f;

	// Use this for initialization
	void Start () {
        instance = this;

        worldPlane = new Plane(-Vector3.forward, 0);

        //targets = GameObject.FindGameObjectsWithTag("Target");

        cameraTexture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGBFloat);

        secondCamera.targetTexture = cameraTexture;
        secondCameraController = secondCamera.GetComponent<SecondCameraController>();

        secondCameraRenderer.material.mainTexture = cameraTexture;

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
            TriggerPause();

        if (!isGamePaused)
        {
            characterRigidbody.transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
            if (characterRigidbody.transform.position.x >= levelEnd)
            {
                if (nextLevel >= 0)
                    Application.LoadLevel(nextLevel);
                else
                    OnGameOver(true);
            }
        }
        Update2ndCamera(); 
	}
    void OnDestroy()
    {
        Destroy(cameraTexture);
    }

    

    void OnGUI()
    {
        if (!isGameOver)
        {
            GUILayout.BeginHorizontal();
            System.Array modes = System.Enum.GetValues(typeof(SecondCameraController.CameraMode));
            foreach (SecondCameraController.CameraMode mode in modes)
                if (GUILayout.Button(System.Enum.GetName(typeof(SecondCameraController.CameraMode), mode)))
                    secondCameraController.CurrentMode = mode;
            GUILayout.EndHorizontal();

            //Draw Shutter Flash
            if (Time.time <= lastShutterTime + shutterTime)
            {
                shutterTexture.SetPixel(0, 0, new Color(1, 1, 1, Mathf.Lerp(0.75f, 0f, Mathf.InverseLerp(lastShutterTime, lastShutterTime + shutterTime, Time.time))));
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

    void TriggerPause()
    {
        isGamePaused = !isGamePaused;

        if(isGamePaused)
        {
            foreach (Target target in Targets)
                target.enabled = false;
        }
        else
        {
            foreach (Target target in Targets)
                target.enabled = true;
        }
    }

    void Update2ndCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (worldPlane.Raycast(ray, out rayDistance))
        {
            Vector3 hit = ray.GetPoint(rayDistance);
            secondCamera.transform.position = new Vector3(hit.x, hit.y, secondCamera.transform.position.z);

            //secondCamera.backgroundColor = Color.gray;
            
            if(Input.GetMouseButtonDown(0) && Screen.height - Input.mousePosition.y > 30f)
            {
                Debug.Log("Shot");
                lastShutterTime = Time.time;

                foreach (Target target in Targets)
                {
                    bool detected = secondCameraController.DetectBounds.Intersects(target.DetectBounds);
                    Debug.Log("Testing " + target.gameObject.name + ", " + detected);
                    if (detected)
                    {
                        Shot(target);
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
                 * */

                secondCameraController.CurrentMode = SecondCameraController.CameraMode.None;
                characterController.Shot();
            }
        }
    }

    void Shot(Target target)
    {
        
        switch(secondCameraController.CurrentMode)
        {
            case SecondCameraController.CameraMode.Filter1:
                if(target.OnFiltered(0))
                {

                }
                break;
            case SecondCameraController.CameraMode.Filter2:
                if (target.OnFiltered(1))
                {

                }
                break;
            case SecondCameraController.CameraMode.Filter3:
                if (target.OnFiltered(2))
                {

                }
                break;
            case SecondCameraController.CameraMode.ScaleUp:
                if(target.OnScaledUp())
                {

                }
                break;
            case SecondCameraController.CameraMode.ScaleDown:
                if (target.OnScaledDown())
                {

                }
                break;
            case SecondCameraController.CameraMode.Blur:
                if(target.OnBlured())
                {

                }
                break;
            case SecondCameraController.CameraMode.Mirror:
                if(target.OnMirrored())
                {

                }
                break;
            case SecondCameraController.CameraMode.ShutterFast:
                if (target.OnFastCaptured())
                {

                }
                break;
            case SecondCameraController.CameraMode.ShutterSlow:
                if (target.OnSlowCaptured())
                {

                }
                break;
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

    public static void OnPlayerHarmed()
    {
        Debug.Log("Player Harmed");
        Instance.OnGameOver();
    }
}
