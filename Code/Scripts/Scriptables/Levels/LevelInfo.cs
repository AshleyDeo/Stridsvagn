using UnityEngine;
using UnityEngine.SceneManagement;

namespace strids {
	[CreateAssetMenu(fileName = "New Level", menuName = "Assets/Level")]
	public class LevelInfo : ScriptableObject {
		[Header("Scene")]
		public string LevelName;
		public LevelInfo NextScene;
        public bool Campaign;

		[Header("Setup")]
        public AudioClip Music;
		public int Lives;
        public GameObject[] Buildings;
        public TankType[] EnemyTypes;
        public TankType[] AllyTypes;

		[Header("Procedural")]
		public int NumCrates;
        public int NumEnemies;
        public int NumAllies;
		public Vector2 MinLocation;
		public Vector2 MaxLocation;

    }
}