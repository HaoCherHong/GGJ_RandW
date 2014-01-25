﻿using UnityEngine;
using System.Collections;

public class GameCharacterController : MonoBehaviour
{
    public enum CharacterState
    {
        Normal,
        ScaleUp,
        ScaleDown
    }
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite scaleUpSprite;
    public Sprite scaleDownSprite;
    public Sprite shuttingSprite;

    CharacterState currentState;
    float lastShotTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time <= lastShotTime + 1.0f)
        {
            spriteRenderer.sprite = shuttingSprite;
        }
        else
        {
            switch(currentState)
            {
                case CharacterState.Normal:
                    spriteRenderer.sprite = normalSprite;
                    break;
                case CharacterState.ScaleUp:
                    spriteRenderer.sprite = scaleUpSprite;
                    break;
                case CharacterState.ScaleDown:
                    spriteRenderer.sprite = scaleDownSprite;
                    break;
            }
        }
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

    public void Shot()
    {
        lastShotTime = Time.time;
    }

    public void SetState(CharacterState state)
    {
        currentState = state;
    }
}
