using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HealthUI, HungerUI, SanityUI, CoinsUI, ReinforcementUI, PauseUI;

    [HideInInspector]
    public string intoAreaIntro;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        intoAreaIntro = "";
        HealthUI.SetActive(false);
        HungerUI.SetActive(false);
        SanityUI.SetActive(false);
        CoinsUI.SetActive(false);
        ReinforcementUI.SetActive(false);
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
