using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretReposition : MonoBehaviour
{
    public float positionX;
    public float positionY;
    public bool turretCanRotate;

    public TankTurretController turret;

    //void OnEnable()
    //{
    //    turret = GameObject.Find("TankTurret").GetComponent<TankTurretController>();
    //    turret.positionX = positionX;
    //    turret.positionY = positionY;
    //    turret.canRotate = turretCanRotate;
    //}

    void Update()
    {

    }
}
