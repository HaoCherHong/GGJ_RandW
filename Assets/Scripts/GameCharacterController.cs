using UnityEngine;
using System.Collections;

public class GameCharacterController : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Target touchedTarget = other.GetComponent<Target>();
        if(touchedTarget)
        {
            if (touchedTarget.isHarmful)
                GameController.OnPlayerHarmed();
        }
    }
}
