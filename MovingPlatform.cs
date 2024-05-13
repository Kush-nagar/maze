using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;

    Rigidbody2D playerRb;


    public void Awake(){
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }
    public void Start(){
        targetPos = posB.position;
    }

    private void Update(){
        if (Vector2.Distance(transform.position, posA.position) < 0.05f){
            targetPos = posB.position;
        }
        if (Vector2.Distance(transform.position, posB.position) < 0.05f){
            targetPos = posA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            collision.transform.parent = this.transform;
            playerRb.gravityScale = playerRb.gravityScale * 50;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            collision.transform.parent = null;
            playerRb.gravityScale = playerRb.gravityScale / 50;
        }
    }
}
