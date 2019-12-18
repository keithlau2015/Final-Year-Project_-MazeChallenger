using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSection : MonoBehaviour
{
    [SerializeField]
    private GameObject Door;

    [SerializeField]
    private string TutorialSectionTopic;

    [SerializeField]
    private GameObject TutorialUI;

    private MenuGameManager gameManager;

    private void Awake()
    {
        Time.timeScale = 1;
        TutorialUI.SetActive(false);
        gameManager = FindObjectOfType<MenuGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialUI.SetActive(true);
            if (TutorialSectionTopic == "HealthUIIntro")
            {
                PlayerStatus.Instance.setHealth(-2, "");
                gameManager.intoAreaIntro = TutorialSectionTopic;
            }
            if (TutorialSectionTopic == "HungerUIIntro")
            {
                PlayerStatus.Instance.setHunger(-50, "");
                gameManager.intoAreaIntro = TutorialSectionTopic;
            }
            if (TutorialSectionTopic == "SanityUIIntro")
            {
                PlayerStatus.Instance.setSanity(-20);
                gameManager.intoAreaIntro = TutorialSectionTopic;
            }
            if(TutorialSectionTopic == "ReinforcementUIIntro")
            {
                gameManager.intoAreaIntro = TutorialSectionTopic;
            }
            if(TutorialSectionTopic == "CoinsUIIntro")
            {
                gameManager.intoAreaIntro = TutorialSectionTopic;
            }
            Time.timeScale = 0;
        }
    }

    public void ClosingTheTutorialPanel()
    {
        TutorialUI.SetActive(false);
        Time.timeScale = 1;
        Destroy(Door);
    }
}
