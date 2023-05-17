using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] Renderer healthBar = null;
	public int HP { get; private set; }
	[SerializeField] int maxHealth;
    public bool LowHP { get; private set; }
    [SerializeField] private int currentHP;

    void Update() {
        if (currentHP != HP) {
            Mathf.Lerp(HP, currentHP, 0.5f);
            if (HP - currentHP <= 0.01) HP = currentHP;
			if (healthBar != null) {
                float ratio = (float)HP / maxHealth;
                healthBar.material.SetFloat("_Health", ratio);
            }
            LowHP = HP < maxHealth * 0.25;
        }
    }
    public void SetHP(int hp) {
        maxHealth = hp;
        HP = hp;
        currentHP = HP;
	}
    public void IncreaseHP(int heal) {
        HP = currentHP;
        currentHP = HP + heal;
        Mathf.Clamp(currentHP, 0, maxHealth);
    }
    public void DecreaseHP(int damage) {
		HP = currentHP;
		currentHP = HP - damage;
		Mathf.Clamp(currentHP, 0, maxHealth);
	}
	public void ResetHP() => HP = maxHealth;
}