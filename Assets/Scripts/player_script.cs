using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class player_script : MonoBehaviour
{
    public static player_script ps;
    public bool isDead = false, isJumpChecked = true, isCrouchChecked = true, cantCheck = false, canDoAnithing = true, finAnim = false, walking = false;
    public int flip = 0, canDo = 1; // 0  nao, 1 = sim
    public int player = 1;
    public int fighter = 0;
    public int ultDirectionP1, ultDirectionP2, jumpheight, attack;
    public float speed = 8, trueSpeed;
    public float p_life, p_life_max, fighterDmg;
    public float p_energy, p_energy_max;
    public float jumpforce = 27.0f;
    public float handcapP1, handcapP2;
    public float gravity;
    public float testeZero;
    public Image healthBar, energyBar;
    public GameObject[] enemyPlayer;
    public GameObject selfPosition;
    public Transform ultPosition;
    public GameObject lFootHitBox, rFootHitBox,/* lFootUpHitBox, rFootUpHitBox,*/ lHandHitBox, rHandHitBox;
    public AnimatorOverrideController[] fighters;
    public GameObject[] ultimates;
    public AudioClip[] sounds;

    //------------------------------------------------------\\
    private int crouching, blocking, p1Wins, p2Wins; // 1 = false, 2 = true
    private float horizontal;
    private float vertical;
    private float distanceX;
    private float distanceY;
    private float distanceTotal;
    private string currentState;
    private bool jump = false;
    private bool crouch = false;
    private bool inUltimate = false;
    private bool isBlocking = false;
    private bool normalAttack = true;
    private bool can_walk = true;
    private bool can_jump = true;
    private bool playedOneTime = false;
    private Vector3 vetorMovimento = Vector3.zero;
    private Vector3 vetorjump;
    //------------------------------------------------------\\
    const string Player_idle = "player_idle";
    const string Player_blocking = "player_block";
    const string Player_walk_block = "player_block_walk";
    const string Player_crouch_idle = "player_crouch_idle";
    const string Player_walk = "player_walk";
    const string Player_jump = "player_jump";
    const string Player_hurt = "player_hurt";
    //------------------------------------------------------\\
    const string Player_attack = "player_punch";
    const string Player_kick = "player_kick";
    const string Player_crouch_punch = "crouch_punch";
    const string Player_crouch_kick = "crouch_kick";
    const string Player_jump_punch = "jump_punch";
    const string Player_jump_kick = "jump_kick";
    const string Player_ultimate = "ultimate";
    const string Player_dying = "player_dying";
    //------------------------------------------------------\\
    string Horizontal1, Horizontal2;
    string Kick1, Kick2;
    string Crouch1, Crouch2;
    string Punch1, Punch2;
    string Jump1, Jump2;
    string Ultimate1, Ultimate2;
    string Block1, Block2;
    int enemyFighter;
    bool falseStartChecked = false;
    GameObject playerPosition;
    //------------------------------------------------------\\
    GameObject ult;
    Rigidbody rb;
    SpriteRenderer spr;
    Animator anim;
    Scene scn;
    CharacterController charCon;
    Transform tr;
    AudioSource musicPlayer;
    
    void Start()
    {
        ps = this;
        rb = GetComponent<Rigidbody>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scn = SceneManager.GetActiveScene();
        musicPlayer = GetComponent<AudioSource>();
        charCon = GetComponent<CharacterController>();
        tr = GetComponent<Transform>();
        CheckCharacter();
        CheckInputs();
        DeactivateAllHitBoxes();
        isBlocking = false;
        if (player == 1)
        {
            p_energy = 0;
            ultDirectionP1 = 1;
        }
        else if (player == 2)
        {
            p_energy = 0;
            flip = 1;
            ultDirectionP2 = -1;
            transform.Rotate(new Vector3(0, -180, 0));
        }
    }
    public void FalseStart()
    {
        ps = this;
        rb = GetComponent<Rigidbody>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scn = SceneManager.GetActiveScene();
        musicPlayer = GetComponent<AudioSource>();
        charCon = GetComponent<CharacterController>();
        tr = GetComponent<Transform>();
        CheckCharacter();
        CheckInputs();
        DeactivateAllHitBoxes();
        isBlocking = false;
        if (player == 1)
        {
            p_energy = 0;
            ultDirectionP1 = 1;
        }
        else if (player == 2)
        {
            p_energy = 0;
            ultDirectionP2 = -1;
        }
    }
    void Update()
    {
        if (falseStartChecked == false)
        {
            FalseStart();
            falseStartChecked = true;
        }
        canDoAnithing = GameManager.gm.canControl;
            Vector3 delta = selfPosition.transform.position - playerPosition
            .transform.position;
            distanceX = delta.x;
            distanceY = delta.y;
            distanceTotal = delta.magnitude;
        if (isBlocking == true)
        {
            speed = 0;
        }
        if ((MainManager.Instance.p2_wins != 2) && (MainManager.Instance.p1_wins != 2))
        {
            //------------------------------------------------------\\
            if (player == 1)
            {                
                p_energy = GameManager.gm.p1_energy;
                GameManager.gm.ultDirectionP1 = ultDirectionP1;
                if (canDoAnithing == true)
                {    
                    if (charCon.isGrounded)
                    {
                        if (can_walk == true && attack == 1 && crouch == false)
                        {
                            vetorMovimento = new Vector3(0,0, Input.GetAxisRaw(Horizontal1));
                            vetorMovimento = transform.TransformDirection(vetorMovimento);
                            vetorMovimento *= speed;
                            horizontal = Input.GetAxisRaw(Horizontal1);                  
                            if(distanceX < 0 )
                            {
                                if(horizontal < 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = 8;
                                        anim.SetFloat("Speed", -1.0f);  
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                                if(horizontal > 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                            }
                            if(distanceX > 0 )
                            {
                                if(horizontal > 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = -8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                                if(horizontal < 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = -trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                            }
                        }
                        if (Input.GetAxisRaw(Jump1) > 0)
                        {
                            if (attack == 1 && can_jump == true && jump == false)
                            {
                                Jump();
                            }
                        }
                        //=======================================\\
                        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick1Button4)) && vetorMovimento.y == 0 && attack == 1 && jump == false)
                        {
                            crouching = 1;
                            Crouch(crouching);
                        }
                        //=======================================\\
                        if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.Joystick1Button4)) && jump == false )
                        {
                            crouching = 2;
                            Crouch(crouching);
                        }
                    }
                    else if (charCon.isGrounded == false)
                    {
                        if (can_walk == true && attack == 1 && crouch == false)
                        {
                            vetorjump = new Vector3(0,0, Input.GetAxisRaw(Horizontal1));
                            vetorjump = transform.TransformDirection(vetorjump);
                            vetorjump *= speed;
                            horizontal = Input.GetAxisRaw(Horizontal1);                       
                            if(distanceX < 0 )
                            {
                                if(horizontal < 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = 8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                                if(horizontal > 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                            }
                            if(distanceX > 0 )
                            {
                                if(horizontal > 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = -8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                                if(horizontal < 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = -trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                            }
                        } 
                    }
                    vetorMovimento.y -= gravity * Time.deltaTime;
                    charCon.Move(vetorMovimento * Time.deltaTime);
                    //=======================================\\
                    if (Input.GetAxisRaw(Punch1) > 0)
                    {
                        Punch();
                    }
                    //=======================================\\
                    if (Input.GetAxisRaw(Kick1) > 0)
                    {
                        Kick();
                    }
                    //=======================================\\
                    if (Input.GetAxisRaw(Ultimate1) > 0)
                    {
                        Debug.Log("Ultimate1");
                        if (p_energy == 100)
                        {
                            Debug.Log("Ultimate11");
                            GameManager.gm.p1_energy = 0;
                            Ultimate();
                        }
                    }
                    if (Input.GetAxisRaw(Block1) > 0)
                    {
                        blocking = 1;
                        Block(blocking);
                    }
                    if (Input.GetAxisRaw(Block1) <= 0)
                    {
                        blocking = 2;
                        Block(blocking);
                    }
                    //=======================================\\
                    //=======================================\\
                }
                healthBar.fillAmount = p_life/p_life_max;
                energyBar.fillAmount = p_energy/p_energy_max;
                if (p_life <= 0 && isDead == false)
                {
                    isDead = true;
                    DeactivateAllHitBoxes();
                    MainManager.Instance.p2_wins += 1;
                    CheckWins();
                    if (p2Wins < 2)
                    {
                        GameManager.gm.PlayerResetLevel1();
                    }
                    else if (p2Wins == 2)
                    {
                        GameManager.gm.PlayerResetLevel2();
                    }
                }
                if (p_energy < 100)
                {
                    playedOneTime = false;
                }
                else if (p_energy >= 100 && playedOneTime == false)
                {
                    ChangeAndPlaySound(1);
                    playedOneTime = true;
                }
                if (p_energy > 100)
                {
                    GameManager.gm.p1_energy = 100;
                }
            }
            //------------------------------------------------------\\
            if (player == 2)
            {
                p_energy = GameManager.gm.p2_energy;
                GameManager.gm.ultDirectionP2 = ultDirectionP2;
                if (canDoAnithing == true)
                {
                    if (charCon.isGrounded)
                    {
                        if (can_walk == true && attack == 1 && crouch == false)
                        {
                            vetorMovimento = new Vector3(0,0, Input.GetAxisRaw(Horizontal2));
                            vetorMovimento = transform.TransformDirection(vetorMovimento);
                            vetorMovimento *= speed;
                            horizontal = vetorMovimento.x;                       
                            if(distanceX < 0 )
                            {
                                if(horizontal < 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = 8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                                if(horizontal > 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                            }
                            if(distanceX > 0 )
                            {
                                if(horizontal > 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = -8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                                if(horizontal < 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = -trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorMovimento * Time.deltaTime);
                                }
                            }
                        }  
                        if (Input.GetAxisRaw(Jump2) > 0 && attack == 1 && can_jump == true && jump == false)
                        {
                            Jump();
                        }
                        //=======================================\\
                        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick2Button4)) && vetorMovimento.y == 0 && attack == 1 && jump == false)
                        {
                            crouching = 1;
                            Crouch(crouching);
                        }
                        //=======================================\\
                        if ((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Joystick2Button4)) && jump == false )
                        {
                            crouching = 2;
                            Crouch(crouching);
                        }
                    }
                    else if (charCon.isGrounded == false)
                    {
                        if (can_walk == true && attack == 1 && crouch == false)
                        {
                            vetorjump = new Vector3(0,0, Input.GetAxisRaw(Horizontal2));
                            vetorjump = transform.TransformDirection(vetorjump);
                            vetorjump *= speed;
                            horizontal = vetorjump.x;                       
                            if(distanceX < 0 )
                            {
                                if(horizontal < 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = 8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                                if(horizontal > 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                            }
                            if(distanceX > 0 )
                            {
                                if(horizontal > 0)
                                {    
                                    if (isBlocking == false)
                                    {
                                        speed = -8;
                                        anim.SetFloat("Speed", -1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                                if(horizontal < 0)
                                {
                                    if (isBlocking == false)
                                    {
                                        speed = -trueSpeed;
                                        anim.SetFloat("Speed", 1.0f);
                                    }
                                    charCon.Move(vetorjump * Time.deltaTime);
                                }
                            }
                        } 
                    }
                    vetorMovimento.y -= gravity * Time.deltaTime;
                    charCon.Move(vetorMovimento * Time.deltaTime);
                    //=======================================\\
                    if (Input.GetAxisRaw(Punch2) > 0)
                    {
                        Punch();
                    }
                    //=======================================\\
                    if (Input.GetAxisRaw(Kick2) > 0)
                    {
                        Kick();
                    }
                    //=======================================\\
                    if (Input.GetAxisRaw(Ultimate2) > 0)
                    {
                        Debug.Log("Ultimate2");
                        if (p_energy == 100)
                        {
                            Debug.Log("Ultimate22");
                            GameManager.gm.p2_energy = 0;
                            Ultimate();
                        }
                    }
                    if (Input.GetAxisRaw(Block2) > 0)
                    {
                        blocking = 1;
                        Block(blocking);
                    }
                    if (Input.GetAxisRaw(Block2) <= 0)
                    {
                        blocking = 2;
                        Block(blocking);
                    }
                    //=======================================\\
                    
                    //=======================================\\
                }
                healthBar.fillAmount = p_life/p_life_max;
                energyBar.fillAmount = p_energy/p_energy_max;
                if (p_life <= 0 && isDead == false)
                {
                    isDead = true;
                    DeactivateAllHitBoxes();
                    MainManager.Instance.p1_wins += 1;
                    CheckWins();
                    if (p1Wins < 2)
                    {
                        GameManager.gm.PlayerResetLevel1();
                    }
                    else if (p1Wins == 2)
                    {
                        GameManager.gm.PlayerResetLevel2();
                    }
                }
                if (p_energy < 100)
                {
                    playedOneTime = false;
                }
                else if (p_energy >= 100 && playedOneTime == false)
                {
                    ChangeAndPlaySound(1);
                    playedOneTime = true;
                }
                if (p_energy > 100)
                {
                    GameManager.gm.p2_energy = 100;
                }
            }
        }
        //------------------------------------------------------\\
    }
    void FixedUpdate()
    {   
        if (canDoAnithing == true)
        {    
                if (distanceX > 0 && flip == 0)//olha pra direita
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                    if (player == 1)
                    {
                    ultDirectionP1 = -1;
                    }
                    if (player == 2)
                    {
                    ultDirectionP2 = -1;
                    } 
                    flip = 1;
                }
                if (distanceX < 0 && flip == 1)//olha pra esquerda
                {
                    transform.Rotate(new Vector3(0, -180, 0));
                    if (player == 1)
                    {
                    ultDirectionP1 = 1;
                    }
                    if (player == 2)
                    {
                    ultDirectionP2 = 1;
                    } 
                    flip = 0;
                }
                if (walking == false && horizontal != 0 && charCon.isGrounded == true)
                {
                    ChangeanimationState(Player_walk);
                    walking = true;
                }
                if (horizontal == 0 && attack == 1 && inUltimate == false)
                {
                    anim.SetFloat("Speed", 1.0f);
                    if (jump == false && crouch == false && isBlocking == false && walking == true)
                    {
                        ForceChangeanimationState(Player_idle);
                        walking = false;  
                    }
                    if (jump == false && crouch == false && isBlocking == false && walking == false)
                    {
                        ForceChangeanimationState(Player_idle); 
                    }
                    else if ((jump == true && finAnim == true) || (crouch == true && finAnim == true) && isBlocking == false)
                    {
                        ChangeanimationState(Player_crouch_idle);
                    }
                    else if (isBlocking == true)
                    {
                        ChangeanimationState(Player_blocking);
                    }
                }
        }
    }
    void ChangeanimationState(string newState)//funcao que tem como variavel necessaria uma string
    {
        if (currentState == newState)return;
        anim.Play(newState);
        currentState = newState;
    }
    void ForceChangeanimationState(string newState)//funcao que tem como variavel necessaria uma string
    {
        anim.Play(newState);
        currentState = newState;
    }
    void CheckCharacter()
    {
        if (player == 1)
        {
            fighter = PlayerPrefs.GetInt("player1");
            anim.runtimeAnimatorController = fighters[fighter];
            enemyFighter = PlayerPrefs.GetInt("player2");
            playerPosition = enemyPlayer[enemyFighter];
            for (int u=0; u<ultimates.Length; u++)
            {
                ultimates[u].gameObject.SetActive(false);
            }
        }
        if (player == 2)
        {
            fighter = PlayerPrefs.GetInt("player2");
            anim.runtimeAnimatorController = fighters[fighter];
            enemyFighter = PlayerPrefs.GetInt("player1");
            playerPosition = enemyPlayer[enemyFighter];
            for (int u=0; u<ultimates.Length; u++)
            {
                ultimates[u].gameObject.SetActive(false);
            }
        }
    }
    public void CanAttackAgain(int canAttack)
    {
        attack = canAttack;
    }
    void Punch()
    {
        if (isBlocking == true) return;
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
        if (isBlocking == true) return;
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
        transform.Translate(Vector3.left * speed);
    }
    void Jump()
    {
        if (isBlocking == true) return;
        ChangeanimationState(Player_jump);
        jump = true;
        vertical = jumpheight;
        vetorMovimento.y = vertical;
        normalAttack = false;
    }
    void Crouch(int cType)
    {
        if (isBlocking == true) return;
        if (horizontal != 0) return;
        if (cType == 1)
        {
            rb.isKinematic = true;
            ChangeanimationState(Player_jump);
            can_jump = false;
            can_walk = false;
            normalAttack = false;
            crouch = true;
            speed = 0;
        }
        else if (cType == 2)
        {
            rb.isKinematic = false;
            ChangeanimationState(Player_idle);
            can_jump = true;
            can_walk = true;
            normalAttack = true;
            crouch = false;
            speed = trueSpeed;
        }
    }
    void Block(int bType)
    {
        if(jump == true || crouch == true || attack == 0) return;
        if (bType == 1)
        {    
            ChangeanimationState(Player_blocking);
            isBlocking = true;
        }
        else if (bType == 2)
        {
            ChangeanimationState(Player_idle);
            isBlocking = false;
        }
    }
    void Ultimate()
    {
        canDoAnithing = false;
        walking = false;
        isBlocking = false;
        crouch = false;
        ForceChangeanimationState(Player_ultimate);
    }
    void InstantiateUltimate()
    {
        if (fighter == 0)
        {
            ult = Instantiate(ultimates[fighter], ultPosition.position, ultPosition.rotation);
            ult.tag = "ult_p" + player.ToString();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "chao") 
        {
            ChangeanimationState(Player_idle);
            vertical = 0;
            jump = false;
            attack = 1;
            can_jump = true;
            if(crouch == false)
            {
                normalAttack = true;
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (player == 1)
        {
            fighterDmg = GameManager.gm.fgt2Dmg;
            handcapP2 =  GameManager.gm.fgt2Handcap;
            if (collision.CompareTag("punch_p2"))
            {
                if (isBlocking == false)
                {
                    p_life -= (fighterDmg + handcapP2);
                    GameManager.gm.p2_energy += 10;
                    if (canDoAnithing == true)
                    {
                        ChangeanimationState(Player_hurt);
                    }
                    ChangeAndPlaySound(2);
                }
                else if (isBlocking == true)
                {
                    p_life -= (fighterDmg + handcapP2)/2;
                    GameManager.gm.p2_energy += 5;
                    ChangeAndPlaySound(2);
                }
            }
            if (collision.CompareTag("kick_p2"))
            {
                if (isBlocking == false)
                {
                    p_life -= (fighterDmg + handcapP2);
                    GameManager.gm.p2_energy += 10;
                    if (canDoAnithing == true)
                    {
                        ChangeanimationState(Player_hurt);
                    }
                    ChangeAndPlaySound(2);
                }
                else if (isBlocking == true)
                {
                    p_life -= (fighterDmg + handcapP2)/2;
                    GameManager.gm.p2_energy += 5;
                    ChangeAndPlaySound(2);
                }
            }
            if (collision.CompareTag("ult_p2"))
            {
                p_life -= (fighterDmg + handcapP2);
                if (canDoAnithing == true)
                {
                    ChangeanimationState(Player_hurt);
                    Destroy(collision.gameObject);
                }
                ChangeAndPlaySound(2);
            }
        }
        //------------------------------------------------------\\
        if (player == 2)
        {
            fighterDmg = GameManager.gm.fgt1Dmg;
            handcapP1 =  GameManager.gm.fgt1Handcap;
            if (collision.CompareTag("punch_p1"))
            {
                if (isBlocking == false)
                {
                    p_life -= (fighterDmg + handcapP1);
                    GameManager.gm.p1_energy += 10;
                    if (canDoAnithing == true)
                    {
                        ChangeanimationState(Player_hurt);
                    }
                    ChangeAndPlaySound(2);
                }
                else if (isBlocking == true)
                {
                    p_life -= (fighterDmg + handcapP1)/2;
                    GameManager.gm.p1_energy += 5;
                    ChangeAndPlaySound(2);
                }
            }
            if (collision.CompareTag("kick_p1"))
            {
                if (isBlocking == false)
                {
                    p_life -= (fighterDmg + handcapP1);
                    GameManager.gm.p1_energy += 10;
                    if (canDoAnithing == true)
                    {
                        ChangeanimationState(Player_hurt);
                    }
                    ChangeAndPlaySound(2);
                }
                else if (isBlocking == true)
                {
                    p_life -= (fighterDmg + handcapP1)/2;
                    GameManager.gm.p1_energy += 5;
                    ChangeAndPlaySound(2);
                }
            }
            if (collision.CompareTag("ult_p1"))
            {
                p_life -= (fighterDmg + handcapP1);
                if (canDoAnithing == true)
                {
                    ChangeanimationState(Player_hurt);
                    Destroy(collision.gameObject);
                }
                ChangeAndPlaySound(2);
            }
        }
    }
    void SetDmg(float dmg)
    {
        if (player == 1)
        {
            GameManager.gm.fgt1Dmg = dmg;
        }
        else if (player == 2)
        {
            GameManager.gm.fgt2Dmg = dmg;
        }
    }
    void ActivatePUNCH()
    {
        lHandHitBox.gameObject.SetActive(true);
        rHandHitBox.gameObject.SetActive(true);
    }
    void DeactivatePUNCH()
    {
        lHandHitBox.gameObject.SetActive(false);
        rHandHitBox.gameObject.SetActive(false);
    }
    void ActivateKICK()
    {
        lFootHitBox.gameObject.SetActive(true);
        rFootHitBox.gameObject.SetActive(true);
        /*lFootUpHitBox.gameObject.SetActive(true);
        rFootUpHitBox.gameObject.SetActive(true);*/
    }
    void DeactivateKICK()
    {
        lFootHitBox.gameObject.SetActive(false);
        rFootHitBox.gameObject.SetActive(false);
        /*lFootUpHitBox.gameObject.SetActive(false);
        rFootUpHitBox.gameObject.SetActive(false);*/
    }
    private void DeactivateAllHitBoxes()
    {
        lFootHitBox.gameObject.SetActive(false);
        rFootHitBox.gameObject.SetActive(false);
        /*lFootUpHitBox.gameObject.SetActive(false);
        rFootUpHitBox.gameObject.SetActive(false);*/
        lHandHitBox.gameObject.SetActive(false);
        rHandHitBox.gameObject.SetActive(false);
    }
    void CheckInputs()
    {
        string inputType;
        inputType = "kb";
        Horizontal1 = "Horizontal1" + inputType;
        Horizontal2 = "Horizontal2" + inputType;
        Kick1 = "Kick1" + inputType; 
        Kick2 = "Kick2" + inputType;
        Crouch1 = "Crouch1" + inputType; 
        Crouch2 = "Crouch2" + inputType;
        Punch1 = "Punch1" + inputType; 
        Punch2 = "Punch2" + inputType;
        Jump1 = "Jump1" + inputType;
        Jump2 = "Jump2" + inputType;
        Ultimate1 = "Ultimate1" + inputType;
        Ultimate2 = "Ultimate2" + inputType;
        Block1 = "Block1" + inputType;
        Block2 = "Block2" + inputType;
    }
    void ChangeAndPlaySound(int track)
    {
        musicPlayer.Stop();
        musicPlayer.clip = sounds[track];
        musicPlayer.Play();
    }
    public void Travar()
    {
        canDoAnithing = false;
    }
    public void Destravar()
    {
        canDoAnithing = true;
    }
    void CheckWins()
    {
        p1Wins = MainManager.Instance.p1_wins;
        p2Wins = MainManager.Instance.p2_wins; 
    }
    void PrintTeste()
    {
        Debug.Log("Teste");
    }
    
}
