using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{

    public GameObject mainMenuScreen;
    public GameObject howToPlayScreen;
    public GameObject creditScreen;
    public AudioSource audioSource;
    public AudioClip buttonSound;

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        audioSource.PlayOneShot(buttonSound, 0.25f);
    }

    public void HowToPlay()
    {
        mainMenuScreen.SetActive(false);
        howToPlayScreen.SetActive(true);
        audioSource.PlayOneShot(buttonSound, 0.25f);

    }

    public void Credits()
    {
        mainMenuScreen.SetActive(false);
        creditScreen.SetActive(true);
        audioSource.PlayOneShot(buttonSound, 0.25f);
    }
    public void MainMenu()
    {
        howToPlayScreen.SetActive(false);
        creditScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        audioSource.PlayOneShot(buttonSound, 0.25f);
    }

    public void Quit()
    {
        audioSource.PlayOneShot(buttonSound, 0.25f);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
