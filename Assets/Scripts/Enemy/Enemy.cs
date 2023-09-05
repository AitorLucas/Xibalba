using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyShot))]
[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    // Rigidbody rb;
    // public int life = 10;
    // PlayerController playerReference;
    // public float movespeed = 3;
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     playerReference = GameObject.FindObjectOfType<PlayerController>();
    // }

    // void Update()
    // {
    //     transform.LookAt(playerReference.transform.position);

    //     if (life <= 0)
    //         Destroy(this.gameObject);
    // }

    // private void FixedUpdate()
    // {
    //     rb.velocity = transform.forward * movespeed;    
    // }
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.tag == "Bullet")
    //     {
    //         Destroy(collision.gameObject);
    //         life--;
    //     }
    // }
    
    public void Kill() {
        Destroy(gameObject);
    }
}
