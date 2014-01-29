using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    public Vector2 offsetTransition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.mainTextureOffset = renderer.material.mainTextureOffset + offsetTransition * Time.deltaTime;
	}
}
