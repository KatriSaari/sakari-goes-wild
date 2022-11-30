using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float Speed;
    Vector2 targetPos;

    void Start()
    {
        targetPos = posB.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position)  < .1f) targetPos = posB.position;

        if (Vector2.Distance(transform.position, posB.position)  < .1f) targetPos = posA.position;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            // collision.transform.position.x = transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
