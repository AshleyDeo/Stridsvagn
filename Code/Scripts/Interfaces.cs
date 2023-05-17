public interface IDestructible {
    public void Damage(int damage);
}
public interface IPickup {
    public void AddAmmo(AmmoType ammoT, int amount);
    public void ActivateShield(float time);
}
public interface ISaveable {
	object SaveState();
	void LoadState(object state);
}