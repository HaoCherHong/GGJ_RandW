using UnityEngine;
using System.Collections;


public class DevilHandTarget : Target {

    public GameObject unBlured;
    public GameObject blured;

    void Start()
    {
        base.Start();
        unBlured.SetActive(true);
        blured.SetActive(false);
    }

    public override bool OnBlured()
    {
        unBlured.SetActive(false);
        blured.SetActive(true);

        isHarmful = false;

        return true;
    }


}
