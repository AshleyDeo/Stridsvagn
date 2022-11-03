using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour {
    [SerializeField] Renderer healthBar = null;

	//public event EventHandle OnHealthChange;
	//public event EventHandle OnDamaged;
	//public event EventHandle OnHealed;
	//public event EventHandle OnDead;

	[SerializeField] int maxHealth;
	[SerializeField] public int HP { get; private set; }
    public bool LowHP { get; private set; }
    public int num;
    void Update() {
        num = HP;
        if (healthBar != null) {
            float ratio = (float)HP / maxHealth;
            healthBar.material.SetFloat("_Health", ratio);
        }
        LowHP = (HP < maxHealth*0.25) ? true : false;
    }
    public void ResetHP() {
        HP = maxHealth;
    }
    public void SetHP(int hp) {
        maxHealth = hp;
        HP = hp;
    }
    public void DecreaseHP(int damage) {
        //Debug.Log("workds");
        HP -= damage;
    }
    public void IncreaseHP(int heal) {
        HP += heal;
        Mathf.Clamp(HP, 0, maxHealth);
    }
}