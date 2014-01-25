using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public Camera secondCamera;
	public Rigidbody2D characterRigidbody;
	public float speed = 1.0f;
    public RenderTexture cameraTexture;
    public Renderer secondCameraRenderer;

    Plane worldPlane;

    GameObject[] targets;
    SecondCameraController secondCameraController;
    Texture2D shutterTexture;

    bool isShotAvailable = false;

    float lastShutterTime = 0.0f;
    float shutterTime = 0.2f;

	// Use this for initialization
	void Start () {
        worldPlane = new Plane(-Vector3.forward, 0);

        targets = GameObject.FindGameObjectsWithTag("Target");

        cameraTexture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGBFloat);

        secondCamera.targetTexture = cameraTexture;
        secondCameraController = secondCamera.GetComponent<SecondCameraController>();

        secondCameraRenderer.material.mainTexture = cameraTexture;

        shutterTexture = new Texture2D(1, 1);
        
	}
	
	// Update is called once per frame
	void Update () 
	{
		characterRigidbody.transform.Translate (Vector2.right * speed * Time.deltaTime, Space.World);
        Update2ndCamera(); 
	}
    void OnDestroy()
    {
        Destroy(cameraTexture);
    }

    

    void OnGUI()
    {

        GUILayout.BeginHorizontal();
        System.Array modes = System.Enum.GetValues(typeof(SecondCameraController.CameraMode));
        foreach (SecondCameraController.CameraMode mode in modes)
            if (GUILayout.Button(System.Enum.GetName(typeof(SecondCameraController.CameraMode), mode)))
                secondCameraController.CurrentMode = mode;
        GUILayout.EndHorizontal();

        //Draw Shutter Flash
        if(Time.time <= lastShutterTime + shutterTime)
        {
            shutterTexture.SetPixel(0, 0, new Color(1, 1, 1, Mathf.Lerp(0.75f, 0f, Mathf.InverseLerp(lastShutterTime, lastShutterTime + shutterTime, Time.time))));
            shutterTexture.Apply();
            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(screenRect, shutterTexture);
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
            
                    if(Input.GetMouseButtonDown(0))
                    {
                        foreach (GameObject targetGO in targets)
                            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(secondCamera), targetGO.renderer.bounds))
                            {
                                Target target = targetGO.GetComponent<Target>();
                                if (target)
                                    Shot(target.GetComponent<Target>());
                            }
                    }
        }
    }

    void Shot(Target target)
    {
        lastShutterTime = Time.time;
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
        }
        secondCameraController.CurrentMode = SecondCameraController.CameraMode.None;
    }

    public static void OnPlayerHarmed()
    {
        Debug.Log("Player Harmed");
    }
}
