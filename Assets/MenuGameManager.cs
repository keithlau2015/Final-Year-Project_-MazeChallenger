﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HealthUI, HungerUI, SanityUI, CoinsUI, ReinforcementUI, PauseUI;

    public string intoAreaIntro;

    // Start is called before the first frame update
    void Start()
    {
        intoAreaIntro = "";                                                                                                                                                                  
    }

    // Update is called once per frame
    void Update()
    {
        switch (intoAreaIntro)
        {
            case "HealthUIIntro":
                HealthUI.SetActive(true);
                break;
            case "HungerUIIntro":
                HungerUI.SetActive(true);
                break;
            case "SanityUIIntro":
                SanityUI.SetActive(true);
                break;
            case "CoinsUIIntro":
                CoinsUI.SetActive(true);
                break;
            case "ReinforcementUIIntro":
                ReinforcementUI.SetActive(true);
                break;
        }
    }
}