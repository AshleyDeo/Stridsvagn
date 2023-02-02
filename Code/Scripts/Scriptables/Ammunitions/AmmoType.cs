using UnityEngine;

public abstract class AmmoType : ScriptableObject {
    public GameObject Ammo;
    public AudioClip AudioClip;
    public string AmmoName;
    public bool IsDrop;
    public abstract void Use(Transform loc);
}