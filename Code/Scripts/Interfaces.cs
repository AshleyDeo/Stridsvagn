using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible {
    public void Damage(int damage);
    //public void OnDead();
}
public interface IPickup {
    public void AddAmmo(AmmoType ammoT, int amount);
    public void ActivateShield(float time);
}
public interface ISaveable {
	object SaveState();
	void LoadState(object state);
}