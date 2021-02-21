using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    public void Win()
    {
        Time.timeScale = 0f;
        winScreen.SetActive(true);
        source.PlayOneShot(clip);
    }

    public void ShowMenu() {
        menuObject.SetActive(true);
    }

    public void TryAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
