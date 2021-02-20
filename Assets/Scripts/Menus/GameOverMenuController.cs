using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;

    public void ShowMenu() {
        menuObject.SetActive(true);
    }

    public void TryAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
