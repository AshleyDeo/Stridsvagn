using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMainMenuTest : MonoBehaviour {
    [SerializeField] TankBase player;
    //UI
    [SerializeField] TMP_Text tankName;
    [SerializeField] TMP_Text tankDesc;
    [SerializeField] TMP_Text tankBonus;
    [SerializeField] private TMP_Text lockText;
    [SerializeField] private TMP_Text confirmText;
    [SerializeField] private Button confirmButton;
	[SerializeField] private bool ignoreLocks = false;

    public TankType[] tanks;
    TankType tank;
    private int numTanks = 0;
    public int tankSelector = 0;

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
        UpdateText();
		player.SetTank(tanks[tankSelector]);
	}
    private bool CheckLocks() {
        if (ignoreLocks) return true;
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
        confirmButton.enabled = CheckLocks();
        confirmText.text = CheckLocks() ? "Confirm" : "Locked";
    }
    public void OnConfirmClick() {
        if (confirmText.text != "Confirm") return;
        GameManager.Instance._tank = tanks[tankSelector];
    }
    public void OnPrevClick() {
        tankSelector--;
        if (tankSelector < 0) {
            tankSelector = numTanks - 1;
        }
        UpdateText();
        player.SetTank(tanks[tankSelector]);
    }
    public void OnNextClick() {
        tankSelector = (tankSelector + 1) % numTanks;
        UpdateText();
		player.SetTank(tanks[tankSelector]);
	}
    //DEBUG
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