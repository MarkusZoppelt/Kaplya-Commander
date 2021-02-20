using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
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
        player.IsInMenu = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        player.IsInMenu = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
