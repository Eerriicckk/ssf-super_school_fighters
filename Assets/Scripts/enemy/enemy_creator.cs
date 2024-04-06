using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_creator : MonoBehaviour
{
    public float time_between_enemy_spawn = 5;
    public float time_test = 5;
    public float speed = 5;
    public bool can_spawn_enemy = true;
    public Transform spawn_position;
    public GameObject enemy_that_will_spawn;
    public static bool flip = false;
    public static float spd = 0;
    // Start is called before the first frame update
    void Start()
    {
        spd = speed;
    }

    // Update is called once per frame
    void Update()
    { 
        if(can_spawn_enemy == true)
        {
            if (time_between_enemy_spawn > 0)
            {
                time_between_enemy_spawn -= Time.deltaTime;
                Debug.Log("Time is runing");
            }
            else
            {
                Instantiate(enemy_that_will_spawn, spawn_position.position, spawn_position .rotation);
                time_between_enemy_spawn = time_test;
                //can_spawn_enemy = false;
            }
        }
        
    }
}
