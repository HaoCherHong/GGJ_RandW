using UnityEngine;
using System.Collections;


public class DevilHandTarget : Target {

    public SpriteRenderer spriteRenderer;
    public Sprite bluredSprite;

    public override bool OnBlured()
    {
        spriteRenderer.sprite = bluredSprite;

        isHarmful = false;

        return true;
    }


}
