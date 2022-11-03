using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour {
    [SerializeField] Renderer healthBar = null;
	public int HP { get; private set; }
	[SerializeField] int maxHealth;
    public bool LowHP { get; private set; }
    [SerializeField] private int currentHP;

	//public event EventHandle OnHealthChange;
	//public event EventHandle OnDamaged;
	//public event EventHandle OnHealed;
	//public event EventHandle OnDead;

    void Update() {
        if (currentHP != HP) {
            Mathf.Lerp(HP, currentHP, 0.5f);
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
        currentHP = HP + heal;
        Mathf.Clamp(currentHP, 0, maxHealth);
    }
    public void DecreaseHP(int damage) => HP -= damage;
    public void ResetHP() => HP = maxHealth;
}