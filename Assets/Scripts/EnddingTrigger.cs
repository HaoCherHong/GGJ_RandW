using UnityEngine;
using System.Collections;

public class EnddingTrigger : MonoBehaviour {

    public int nextLevel = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Level Endded");
            if (nextLevel >= 0)
                Application.LoadLevel(nextLevel);
            else
                GameController.Instance.OnGameOver(true);
        }
    }
}
