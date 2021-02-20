using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject morePanel;
    [SerializeField] private GameObject mainButtonGroup;

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
}
