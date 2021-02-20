using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Image sfxButtonImage;
    [SerializeField] private AudioManager audioManager;
    [Header("UI Sprites")]
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite sfxOn;
    [SerializeField] private Sprite sfxOff;

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void ToggleAudio(string groupName)
    {
        audioManager.ToggleMute(groupName);
    }

    public void UpdateMusicButton()
    {
        if (audioManager.IsMuted("Music"))
        {
            musicButtonImage.sprite = musicOff;
        }
        else
        {
            musicButtonImage.sprite = musicOn;
        }
    }

    public void UpdateSFXButton()
    {
        if (audioManager.IsMuted("SFX"))
        {
            sfxButtonImage.sprite = sfxOff;
        }
        else
        {
            sfxButtonImage.sprite = sfxOn;
        }
    }
}
