using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class botScript : MonoBehaviour
{
    public bool jump = false;
    public bool crouch = false;
    public bool inUltimate = false;
    public bool normalAttack = true;
    public bool can_walk = true;
    public bool can_jump = true;
    public bool canDoAnithing = true;
    public int flip = 0;
    public int whatBotWillDo, botNumOfActions;
    public int attacking = 1; // 0 = true, 1 = false
    public float speed = 8;
    public int fighter = 1;
    public float p_life, p_life_max;
    public float p_energy, p_energy_max;
    public float jumpforce = 10;
    public AnimatorOverrideController fighter1, fighter2; 
    public Image healthBar, energyBar;
    //------------------------------------------------------\\
    public GameObject playerPosition;
    public GameObject selfPosition;
    public float distanceX;
    public float distanceY;
    public float distanceTotal;
    //------------------------------------------------------\\
    float horizontal;
    public int attack;
    //------------------------------------------------------\\
    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;
    Scene scn;
    //------------------------------------------------------\\
    private string currentState;
    //------------------------------------------------------\\
    const string Player_idle = "player_idle";
    const string Player_walk = "player_walk";
    const string Player_attack = "player_punch";
    const string Player_kick = "player_kick";
    const string Player_crouch_punch = "crouch_punch";
    const string Player_crouch_kick = "crouch_kick";
    const string Player_jump_punch = "jump_punch";
    const string Player_jump_kick = "jump_kick";
    const string Player_ultimate = "ultimate";
    const string Player_jump = "player_jump";


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scn = SceneManager.GetActiveScene();
        CheckCharacter();
        if (fighter == 1)
        {
            anim.runtimeAnimatorController = fighter1;
        }
        else if (fighter == 2)
        {
            anim.runtimeAnimatorController = fighter2;
        }
        p_energy = 0;
        flip = 1;
        transform.Rotate(new Vector3(0, -180, 0));
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 delta = selfPosition.transform.position - playerPosition
        .transform.position;
        distanceX = delta.x;
        distanceY = delta.y;
        distanceTotal = delta.magnitude;
        //------------------------------------------------------\\
        p_energy = GameManager.gm.p2_energy;
        //------------------------------------------------------\\
        if (canDoAnithing == true)
        {
            if (p_life <= 100)
            {
                if (p_life <= 75)
                {
                    if(p_life <= 45)
                    {
                        botNumOfActions = 7;
                        WhatTheBotWillDo(botNumOfActions);
                    }
                    else
                    {
                        botNumOfActions = 4;
                        WhatTheBotWillDo(botNumOfActions);
                    }
                }
                else
                {
                    whatBotWillDo = 1;// 1 = attack
                }
            }
            //=======================================\\
            if (Mathf.Abs(distanceX) > 9)
            {
                if (can_walk == true && attacking == 1 && crouch == false)
                {
                    if (distanceX < 0)
                    {
                        speed = -0.25f;
                        Walk();
                    }
                    if (distanceX > 0)
                    {
                        speed = -0.25f;
                        Walk();
                    }
                }
            }
            //=======================================\\
            else
            {
                if (whatBotWillDo == 1 || whatBotWillDo == 2 || whatBotWillDo == 3)
                {
                    if (p_energy == 100)
                    {
                        Ultimate();
                    }
                    //=======================================\\
                    if (Mathf.Abs(distanceX) < 6 && Mathf.Abs(distanceY) < 12 && attacking == 1) 
                    {
                        speed = 0;
                        attack = Random.Range(0,3);
                        if (attack < 2)
                        {
                            Punch();
                        }
                        else if (attack == 2)
                        {
                            Kick();
                        }
                    }
                    if (Mathf.Abs(distanceX) < 8 && Mathf.Abs(distanceX) > 6 && 
                    Mathf.Abs(distanceY) < 12 && attacking == 1)
                    {
                        speed = 0;
                        Kick();
                    }
                }
                //=======================================\\
                if (whatBotWillDo == 4 || whatBotWillDo == 5)
                {    
                    if (Input.GetKeyDown(KeyCode.DownArrow) && attacking == 1 && jump == false)
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                        ChangeanimationState(Player_jump);
                        can_jump = false;
                        can_walk = false;
                        normalAttack = false;
                        crouch = true;
                        speed = 0;
                    }
                    //=======================================\\
                    if (Input.GetKeyUp(KeyCode.DownArrow) && jump == false )
                    {
                        rb.constraints = RigidbodyConstraints2D.None;
                        ChangeanimationState(Player_idle);
                        can_jump = true;
                        can_walk = true;
                        normalAttack = true;
                        crouch = false;
                        speed = 8;
                    }
                }
                //=======================================\\
                if (whatBotWillDo == 6)
                {    
                    if (Input.GetKeyDown(KeyCode.UpArrow) && attacking == 1 && can_jump == true)
                    {
                        Jump();
                    }
                }
                
            }
        }
        //------------------------------------------------------\\
        healthBar.fillAmount = p_life/p_life_max;
        energyBar.fillAmount = p_energy/p_energy_max;
        if (p_life == 0)
        {
            MainManager.Instance.p1_wins += 1;
            SceneManager.LoadScene(scn.name);
        }
        if (p_energy > 100)
        {
            GameManager.gm.p1_energy = 100;
        }
    }
    void FixedUpdate()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (canDoAnithing == true)
        {    
            if (distanceX < 0)
            {
                attacking = 1;
                if (flip == 1)
                {
                    transform.Rotate(new Vector3(0, -180, 0));
                    flip = 0;
                }
                if (jump == false && speed != 0)
                {
                    ChangeanimationState(Player_walk);
                }
            } 
            else if (distanceX > 0)
            {
                attacking = 1;
                if (flip == 0)
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                    flip = 1;
                }
                if (jump == false && speed != 0)
                {
                    ChangeanimationState(Player_walk);
                }
            }
            else if (speed == 0 && jump == false && attacking == 1 && crouch == false && inUltimate == false)
            {
                ChangeanimationState(Player_idle);
            }
        }
        
    }
    void ChangeanimationState(string newState)//funcao que tem como variavel necessaria uma string
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }
    void CheckCharacter()
    {
        fighter = PlayerPrefs.GetInt("player2");
    }
    void Punch()
    {
        CanAttackAgain(0);
        if (jump == true)
        {
            ChangeanimationState(Player_jump_punch);
        }
        if (crouch == true)
        {
            ChangeanimationState(Player_crouch_punch);
        }
        if (normalAttack == true)
        {
            ChangeanimationState(Player_attack);
        }
    }
    void Kick()
    {
        CanAttackAgain(0);
        if (jump == true)
        {
            ChangeanimationState(Player_jump_kick);
        }
        if (crouch == true)
        {
            ChangeanimationState(Player_crouch_kick);
        }
        if (normalAttack == true)
        {
            ChangeanimationState(Player_kick);
        }
    }
    void Walk()
    {
        transform.Translate(Vector2.left * speed);
        Debug.Log(Vector2.left * speed);
    }
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        ChangeanimationState(Player_jump);
        normalAttack = false;
        jump = true;
    }
    void Crouch()
    {

    }
    void Ultimate()
    {
        GameManager.gm.p2_energy = 0;
        canDoAnithing = false;
        ChangeanimationState(Player_ultimate);
    }
    void WhatTheBotWillDo(int numberOfActions)
    {
        whatBotWillDo = Random.Range(1,numberOfActions);
    }
    public void CanAttackAgain(int canAttack)
    {
        attacking = canAttack;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao") 
        {
            jump = false;
            if(crouch == false)
            {
                normalAttack = true;
            }
            attacking = 1;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("punch_p1"))
        {
            p_life -= 5;
            GameManager.gm.p1_energy += 10;
        }
        if (collision.CompareTag("kick_p1"))
        {
            p_life -= 10;
            GameManager.gm.p1_energy += 10;
        }
        
        //------------------------------------------------------\\
    }

}
