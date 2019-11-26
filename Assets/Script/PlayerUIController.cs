﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Text Health, Hunger, Coins, Price, Result, DeathReason;

    [SerializeField]
    private Slider bgmVolumeSlider, soundEffectVolumeSlider;

    [SerializeField]
    private GameObject warningPanel, volumeSettingPanel, pickUpSight, frontSight;

    private void Awake()
    {
        warningPanel.SetActive(false);
        volumeSettingPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        Price.enabled = PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine();
        Price.text = PlayerStatus.Instance.getPriceUIText();
        pickUpSight.SetActive(PlayerStatus.Instance.getPlayerCanInteractWithOtherObject());
        frontSight.SetActive(!PlayerStatus.Instance.getPlayerCanInteractWithVendingMachine());
        Hunger.text = PlayerStatus.Instance.getHunger().ToString();
        Health.text = PlayerStatus.Instance.getHealth().ToString();
        Coins.text = PlayerStatus.Instance.getCoins().ToString();
        Result.text = "You had reach to " + PlayerStatus.Instance.getPlayerReachLevels() + " Levels !";
        if (PlayerStatus.Instance.getPlayerReachLevels() > 0) DeathReason.text = PlayerStatus.Instance.getPlayerKilledBy();
        else DeathReason.text = PlayerStatus.Instance.getPlayerKilledBy();
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
}