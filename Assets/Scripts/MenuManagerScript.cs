using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{

    public GameObject mainMenuScreen;
    public GameObject howToPlayScreen;
    public GameObject creditScreen;
    public string gameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void HowToPlay()
    {
        mainMenuScreen.SetActive(false);
        howToPlayScreen.SetActive(true);
    }

    public void Credits()
    {
        mainMenuScreen.SetActive(false);
        creditScreen.SetActive(true);
    }
    public void MainMenu()
    {
        howToPlayScreen.SetActive(false);
        creditScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
