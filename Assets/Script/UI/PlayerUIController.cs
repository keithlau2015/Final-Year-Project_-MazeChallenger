using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Text Health, Hunger, Coins, Price, Sanity, Reinforcement, Result, DeathReason, ReadingMaterials;

    [SerializeField]
    private Slider bgmVolumeSlider, soundEffectVolumeSlider;

    [SerializeField]
    private GameObject warningPanel, volumeSettingPanel, pickUpSight, frontSight, getHurtPanel;

    [SerializeField]
    private Transform getHurtSpawningPoint;

    private int playerHpTemp;

    private void Awake()
    {
        warningPanel.SetActive(false);
        volumeSettingPanel.SetActive(false);
    }

    private void Update()
    {
        Price.enabled = PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine();
        Price.text = PlayerStatus.Instance.getPriceUIText();
        pickUpSight.SetActive(PlayerStatus.Instance.getPlayerCanInteractWithOtherObject());
        if (PlayerStatus.Instance.getPlayerCanInteractWithReadingMaterial() || PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine()) frontSight.SetActive(false);
        else frontSight.SetActive(true);
        Hunger.text = PlayerStatus.Instance.getHunger().ToString();
        Health.text = PlayerStatus.Instance.getHealth().ToString();
        Coins.text = PlayerStatus.Instance.getCoins().ToString();
        Sanity.text = PlayerStatus.Instance.getSanity().ToString();
        Reinforcement.text = PlayerStatus.Instance.getReinforcement().ToString();
        ReadingMaterials.text = PlayerStatus.Instance.getReadingMaterials();
        ReadingMaterials.enabled = PlayerStatus.Instance.getPlayerCanInteractWithReadingMaterial();
        Result.text = "You had reach to " + PlayerStatus.Instance.getPlayerReachLevels() + " Levels !";
        DeathReason.text = PlayerStatus.Instance.getPlayerKilledBy();
        if (PlayerStatus.Instance.getHealth() > playerHpTemp) playerHpTemp = PlayerStatus.Instance.getHealth();
        else if(PlayerStatus.Instance.getHealth() < playerHpTemp)
        {
            playerHpTemp = PlayerStatus.Instance.getHealth();
            GameObject clone = Instantiate(getHurtPanel, getHurtSpawningPoint) as GameObject;
            Destroy(clone, 2);
        }
    }

    public void SoundEffectVolumeSlide()
    {
        GameObject.FindObjectOfType<SoundManager>().AdjustSoundEffectVolume(soundEffectVolumeSlider.value);
    }

    public void BGMVolumeSlide()
    {
        GameObject.FindObjectOfType<SoundManager>().AdjustBGMVolume(bgmVolumeSlider.value);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
        PlayerStatus.Instance.resetData();
        warningPanel.SetActive(false);
        volumeSettingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToPauseMenu()
    {
        warningPanel.SetActive(false);
        volumeSettingPanel.SetActive(false);
    }

    public void PopUpWarningPanel()
    {
        warningPanel.SetActive(true);
    }

    public void PopUpTheVolumeSettingPanel()
    {
        volumeSettingPanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
        PlayerStatus.Instance.resetData();
        warningPanel.SetActive(false);
        volumeSettingPanel.SetActive(false);
        Time.timeScale = 1;
    }
}