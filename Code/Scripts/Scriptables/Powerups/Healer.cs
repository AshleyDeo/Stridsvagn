using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Powerup", menuName = "Assets/Powerups/Heal")]
public class Healer : PowerupEffect {
	[Header("Stats")]
	public int HealAmount;
	public override void Apply(GameObject target) {
		SoundManager.Instance.PlaySound(PowerupSound);
		target.GetComponent<Health>().IncreaseHP(HealAmount);
	}
}