using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Powerup", menuName = "Assets/Powerups/Heal")]
public class Healer : PowerupEffect {
	[Header("Stats")]
	public int healAmount;
	public override void Apply(GameObject target) {
		SoundManager.Instance.PlaySound(powerupSound);
		target.GetComponent<Health>().IncreaseHP(healAmount);
	}
}