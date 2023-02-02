using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupEffect : ScriptableObject {
	[Header("Design")]
	public GameObject Drop;
	public string powerupName;
	public AudioClip powerupSound;
	public abstract void Apply(GameObject target);
}
