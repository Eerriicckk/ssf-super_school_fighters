/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour
{
    float speed = 0;
    public GameObject enemyfeedback;
    SpriteRenderer spr;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        speed = enemy_creator.spd;
        tag = "enemy";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime);//faz o inimigo andar
        if (speed < 0)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("punch_p1"))
        {
            Instantiate(enemyfeedback, collision.transform.position, collision.transform.rotation);
            GameManager.gm.AddEnemiesKilled();
            Destroy(gameObject);
            //speed = 0;


        }
    }
    public void DeathTime()
    {
        Destroy(gameObject);
    }
}*/
