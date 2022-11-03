using UnityEngine;

public abstract class AmmoType : ScriptableObject {
    public GameObject Ammo;
    public AudioClip audioClip;
    public string ammoName;
    public bool isDrop;
    public abstract void Use(Transform loc);
}