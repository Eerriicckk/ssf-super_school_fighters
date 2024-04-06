using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateScript : MonoBehaviour
{
    public static UltimateScript ultS;
    CharacterController charCon;
    private Vector3 vetorMovimento = Vector3.zero;
    public float speed = 12;
    public bool canMove;
    int ultDirection;
    void Start()
    {
        charCon = GetComponent<CharacterController>();
        ultS = this;
        if (gameObject.tag == "ult_p1")
        {
            ultDirection = GameManager.gm.ultDirectionP1;
        }
        else if (gameObject.tag == "ult_p2")
        {
            ultDirection = GameManager.gm.ultDirectionP2;
        }
        speed *= ultDirection;
        InvokeRepeating("DestroyThis", 5.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        vetorMovimento = new Vector3(speed,0,0);
        vetorMovimento.x = speed;
        vetorMovimento.x += speed * Time.deltaTime;
        charCon.Move(vetorMovimento * Time.deltaTime);
        Debug.Log(canMove);
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
