using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject morePanel;
    [SerializeField] private GameObject mainButtonGroup;
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Image sfxButtonImage;
    [SerializeField] private AudioManager audioManager;
    [Header("UI Sprites")]
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite sfxOn;
    [SerializeField] private Sprite sfxOff;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartPlaying()
    {
        // TODO:
    }

    public void ShowMore()
    {
        morePanel.SetActive(true);
        mainButtonGroup.SetActive(false);
    }

    public void HideMore()
    {
        morePanel.SetActive(false);
        mainButtonGroup.SetActive(true);
    }

    public void FollowLink(string url)
    {
        Application.OpenURL(url);
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
