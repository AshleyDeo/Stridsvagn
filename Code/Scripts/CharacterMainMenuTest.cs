using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMainMenuTest : MonoBehaviour {
    [SerializeField] TankBase player;

    [Header("UI")]
    [SerializeField] private TMP_Text _tankName;
    [SerializeField] private TMP_Text _tankDesc;
    [SerializeField] private TMP_Text _tankBonus;
    [SerializeField] private TMP_Text _lockText;
    [SerializeField] private TMP_Text _confirmText;
    [SerializeField] private Button _confirmButton;
	[SerializeField] private bool _ignoreLocks = false;

	[Header("Tank Selection")]
	public TankType[] Tanks;
    private TankType _tank;
    private int _numTanks = 0;
    public int TankSelector = 0;

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
        _numTanks = Tanks.Length;
    }
    void Start() {
        UpdateText();
		player.SetTank(Tanks[TankSelector]);
	}
    private bool CheckLocks() {
        if (_ignoreLocks) return true;
        if (_tank.Campaign && !GameManager.Instance.Campaign) {
            return false;
        }
        if (GameManager.Instance.Kills < _tank.Kills) {
            return false;
        }
        if (_tank.ZeroDeaths && GameManager.Instance.Deaths > 0) {
            return false;
        }
        return true;
    }
    //UI
    private void UpdateText() {
        _tank = Tanks[TankSelector];
        _tankName.text = _tank.name;
        _tankDesc.text = _tank.Flavor;
        _tankBonus.text = _tank.Bonus;
        _lockText.text = _tank.HowToUnlock;
        _confirmButton.enabled = CheckLocks();
        _confirmText.text = CheckLocks() ? "Confirm" : "Locked";
    }
    public void OnConfirmClick() {
        if (_confirmText.text != "Confirm") return;
        GameManager.Instance.Tank = Tanks[TankSelector];
    }
    public void OnPrevClick() {
        TankSelector--;
        if (TankSelector < 0) {
            TankSelector = _numTanks - 1;
        }
        UpdateText();
        player.SetTank(Tanks[TankSelector]);
    }
    public void OnNextClick() {
        TankSelector = (TankSelector + 1) % _numTanks;
        UpdateText();
		player.SetTank(Tanks[TankSelector]);
	}
    //DEBUG
    public void DebugReset() {
        GameManager.Instance.Kills = 0;
        GameManager.Instance.Deaths = int.MaxValue;
        GameManager.Instance.Campaign = false;
    }
    public void DebugKills(int kills) {
        GameManager.Instance.Kills += kills;
    }
    public void DebugCampaign() {
        GameManager.Instance.Campaign = !GameManager.Instance.Campaign;
    }
    public void DebugNLL() {
        GameManager.Instance.Deaths = 0;
    }
}