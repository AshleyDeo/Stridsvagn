using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerMenu : MonoBehaviour {
    private static int enemyCount;
    private static int allyCount;
    //unchenged
    private Scene currentScene;
    private int gameMode;

    public GameObject powerup;
    public Transform targetSpawn;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public bool sameSpawnAreaTanksAndWalls;
    public float xMinWalls;
    public float xMaxWalls;
    public float yMinWalls;
    public float yMaxWalls;
    public int maxObjects;
    [HideInInspector] public int objectsToSpawn;
    public int cratesToSpawn;
    private int decider;
    public bool proceduralTanks;
    public bool proceduralWalls;

    public Text gameText;
    public Text enemiesLeftText;

    public static Vector3 spawnPoint;
    public int xSpawn;
    public int ySpawn;
    public int zSpawn;

    private bool hasGameStarted = false;
    public int[] campaignThresholds = { 1, 1, 1, 1, 1, 1, 0, 0 };
    public int[] noLivesLostCampaign = { 1, 1, 1, 1, 1, 1, 1, 0 };
    private int deathlessRun;

    private void Awake() {
        if (!PlayerPrefs.HasKey("gameMode")) {
            PlayerPrefs.SetInt("gameMode", 3);
        }
        else {
            gameMode = PlayerPrefs.GetInt("gameMode");
        }
        if (!PlayerPrefs.HasKey("campaignThresholds")) {
            PlayerPrefsX.SetIntArray("campaignThresholds", campaignThresholds);
        }
        else {
            campaignThresholds = PlayerPrefsX.GetIntArray("campaignThresholds");
        }
        if (!PlayerPrefs.HasKey("deathlessRun")) {
            PlayerPrefs.SetInt("deathlessRun", 1);
        }
        else {
            deathlessRun = PlayerPrefs.GetInt("deathlessRun");
        }
        if (!PlayerPrefs.HasKey("noLivesLostCampaign")) {
            PlayerPrefsX.SetIntArray("noLivesLostCampaign", noLivesLostCampaign);
        }
        else {
            noLivesLostCampaign = PlayerPrefsX.GetIntArray("noLivesLostCampaign");
        }

        currentScene = SceneManager.GetActiveScene();
        allyCount = 0;
        enemyCount = 0;
        objectsToSpawn = Random.Range(maxObjects / 2, maxObjects + 1);
        if (sameSpawnAreaTanksAndWalls) {
            xMinWalls = xMin;
            xMaxWalls = xMax;
            yMinWalls = yMin;
            yMaxWalls = yMax;
        }
        spawnPoint = new Vector3(xSpawn, ySpawn, zSpawn);
    }
    private void Start() {
        hasGameStarted = true;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown("0")) {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown("1")) {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown("2")) {
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown("3")) {
            SceneManager.LoadScene(5, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown("4")) {
            SceneManager.LoadScene(6, LoadSceneMode.Single);
        }
        if (Crate.numCrates <= cratesToSpawn) {
            Vector3 pos = new(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -0.194f);
            targetSpawn.position = pos;
            var hits = Physics2D.OverlapCircleAll(pos, 2.0f, 31 << 6);
            while (hits.Length > 0) {
                pos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -0.25f);
                hits = Physics2D.OverlapCircleAll(pos, 2.0f, 31 << 6);
            }

            GameObject crate = Instantiate(powerup, pos, transform.rotation) as GameObject;
            crate.transform.Rotate(180f, 0f, 0f);
            //Debug.Log(Crate.numCrates);
        }
    }
    public int countEnemies() => enemyCount;
    public int countAllies() => allyCount;
    public void reloadScene() {
        hasGameStarted = false;
        Crate.numCrates = 0;
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
    }
    public void loadScene(int sceneNumber) {
        hasGameStarted = false;
        Crate.numCrates = 0;
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }
}