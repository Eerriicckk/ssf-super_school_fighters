using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_feedback_destroyer : MonoBehaviour
{
    float speed = 0;
    void Start()
    {
        speed = 15;
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
