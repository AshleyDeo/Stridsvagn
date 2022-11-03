using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterMainMenuTest : MonoBehaviour {
    [SerializeField] PlayerMenu player;
    //UI
    [SerializeField] TMP_Text tankName;
    [SerializeField] TMP_Text tankDesc;
    [SerializeField] TMP_Text tankBonus;
    [SerializeField] private TMP_Text lockText;
    [SerializeField] private TMP_Text confirmText;

    public TankType[] tanks;
    TankType tank;
    private int numTanks = 0;

    //unchanged
    public int tankSelector = 0;
    private int gameMode;
    /*
     --TANKS--
    0 - Strid
    1 - Whispering Death
    2 - Muzak Standard
    3 - Muzak Turbine V1
    4 - Muzak Turbine V2
    5 - Muzak Modern V1
    6 - Muzak Modern V2
    7 - Muzak Advanced
     */
    void Awake() {
        numTanks = tanks.Length;
    }
    void Start() {
        if (!PlayerPrefs.HasKey("gameMode")) {
            PlayerPrefs.SetInt("gameMode", 3);
        }
        else {
            gameMode = PlayerPrefs.GetInt("gameMode");
        }
        UpdateText();
        player.SelectTank();
    }
    private bool CheckLocks() {
        if (tank.campaign && !GameManager.Instance._campaign) {
            return false;
        }
        if (GameManager.Instance._kills < tank.kills) {
            return false;
        }
        if (tank.zeroDeaths && GameManager.Instance._deaths > 0) {
            return false;
        }
        return true;
    }
    //UI
    private void UpdateText() {
        tank = tanks[tankSelector];
        tankName.text = tank.name;
        tankDesc.text = tank.flavor;
        tankBonus.text = tank.bonus;
        lockText.text = tank.howToUnlock;
        if (CheckLocks()) {
            confirmText.text = "Confirm";
        }
        else {
            confirmText.text = "Locked";
        }
    }
    public void OnConfirmClick() {
        if (confirmText.text == "Confirm") {
            GameManager.Instance._tank = tanks[tankSelector];
            SceneManager.LoadScene("Test", LoadSceneMode.Single);
        }
    }
    public void OnPrevClick() {
        tankSelector--;
        if (tankSelector < 0) {
            tankSelector = numTanks - 1;
        }
        UpdateText();
        player.SelectTank();
    }
    public void OnNextClick() {
        tankSelector = (tankSelector + 1) % numTanks;
        UpdateText();
        player.SelectTank();
    }
    public void ExitToMenu() {
        Crate.numCrates = 0;
        SceneManager.LoadScene("MenuSP", LoadSceneMode.Single);
    }
    //Level Select
    public void GoToLevelSelect() {
        Crate.numCrates = 0;
        if (gameMode == 3) SceneManager.LoadScene("LevelSelectSP", LoadSceneMode.Single);
        if (gameMode == 4) { SceneManager.LoadScene("Main", LoadSceneMode.Single); }
        if (gameMode == 5) SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }
    //Debug
    public void DebugReset() {
        GameManager.Instance._kills = 0;
        GameManager.Instance._deaths = int.MaxValue;
        GameManager.Instance._campaign = false;
    }
    public void DebugKills(int kills) {
        GameManager.Instance._kills += kills;
    }
    public void DebugCampaign() {
        GameManager.Instance._campaign = !GameManager.Instance._campaign;
    }
    public void DebugNLL() {
        GameManager.Instance._deaths = 0;
    }
}