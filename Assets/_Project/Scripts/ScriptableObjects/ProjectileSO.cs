using System;
using UnityEngine;

[CreateAssetMenu()]
public class ProjectileSO : ScriptableObject {

    public Projectile projectile;
    public bool isSpell;
    public float animationDuration;  
    public float lifeTime;
    public float damage;
     public float shotDelay;
    public Sprite sprite;
    public string objectName;

}