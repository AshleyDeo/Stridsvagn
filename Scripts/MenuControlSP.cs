using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlSP : MonoBehaviour {
    private int gameMode; // 3 == free play, 4 == campaign, 5 == tutorial
    private int livesRemaining;
    private void Awake() {
        if (!PlayerPrefs.HasKey("gameMode")) {
            PlayerPrefs.SetInt("gameMode", 3);
        }
        else {
            gameMode = PlayerPrefs.GetInt("gameMode");
        }

        if (!PlayerPrefs.HasKey("livesLeft")) {
            PlayerPrefs.SetInt("livesLeft", 4);
        }
        else {
            livesRemaining = PlayerPrefs.GetInt("livesLeft");
        }
    }
    public void StartGame() {
        PlayerPrefs.SetInt("gameMode", 3);
        PlayerPrefs.SetInt("livesLeft", 4);
        SceneManager.LoadScene("CharacterSelectSP", LoadSceneMode.Single);
    }
    public void StartCampaign() {
        PlayerPrefs.SetInt("gameMode", 4);
        PlayerPrefs.SetInt("livesLeft", 4);
        SceneManager.LoadScene("CharacterSelectSP", LoadSceneMode.Single);
    }
    public void StartTutorial() {
        PlayerPrefs.SetInt("gameMode", 5);
        SceneManager.LoadScene("CharacterSelectSP", LoadSceneMode.Single);
    }
    public void ExitGame() => Application.Quit();
}