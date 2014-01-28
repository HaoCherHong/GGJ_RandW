using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

    public GameCharacterController character;
    Vector3 positionDelta;

	// Use this for initialization
	void Start () {
        positionDelta = transform.position - character.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = character.transform.position + positionDelta;
	}
}
