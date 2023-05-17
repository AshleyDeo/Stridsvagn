using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerMenu : MonoBehaviour {
    private static int enemyCount;
    private static int allyCount;

    public Text gameText;
    public Text enemiesLeftText;

    public int[] campaignThresholds = { 1, 1, 1, 1, 1, 1, 0, 0 };
    public int[] noLivesLostCampaign = { 1, 1, 1, 1, 1, 1, 1, 0 };
    private int deathlessRun;
    private void Awake() {
    }
    private void Start() {
    }
    private void Update() {
    }
    public int countEnemies() => enemyCount;
    public int countAllies() => allyCount;
}