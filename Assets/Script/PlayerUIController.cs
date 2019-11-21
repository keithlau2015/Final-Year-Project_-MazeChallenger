using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUpSight, frontSight;

    [SerializeField]
    private Text Health, Hunger, Coins, Price;

    [SerializeField]
    private Slider bgmVolumeSlider, soundEffectVolumeSlider;

    [SerializeField]
    private GameObject warningPanel, volumeSettingPanel;

    // Update is called once per frame
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
