using UnityEngine;
using System.Collections;

public class GameCharacterController : Target
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

    public GameObject upstate;
    public GameObject downstate;
    public GameObject normalstate;

    CharacterState currentState;
    float lastShotTime = 0.0f;

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Time.time <= lastShotTime + 1.0f)
        {
            spriteRenderer.sprite = shuttingSprite;
        }
        else
        {
            switch (currentState)
            {
                case CharacterState.Normal:
                    spriteRenderer.sprite = scaleUpSprite;
                    normalstate.SetActive(true);
                    upstate.SetActive(false);
                    downstate.SetActive(false);
                    break;
                case CharacterState.ScaleUp:
                    spriteRenderer.sprite = scaleUpSprite;
                    normalstate.SetActive(false);
                    upstate.SetActive(true);
                    downstate.SetActive(false);
                    break;
                case CharacterState.ScaleDown:
                    spriteRenderer.sprite = scaleDownSprite;
                    normalstate.SetActive(false);
                    upstate.SetActive(false);
                    downstate.SetActive(true);
                    break;
            }
        }
    }

    public override bool OnFiltered(bool submitEffects, Filter filter)
    {
        return false;
    }

    public override bool OnMirrored(bool submitEffects)
    {
        return false;
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
