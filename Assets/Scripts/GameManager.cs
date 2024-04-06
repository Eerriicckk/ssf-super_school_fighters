using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int p1Wins, p2Wins, backgSelected, ultDirectionP1, ultDirectionP2, travar; 
    public float p1_energy, p2_energy, fgt1Dmg, fgt2Dmg, fgt1Handcap, fgt2Handcap;
    public float timer = 100;
    public bool canControl = true, canCount = false, fightEnded = false;
    public Text timer_count;
    public GameObject endGamePanel;
    public Image p1_win1, p2_win1, backg1, backg2;
    public Image[] backgrounds;
    public AudioClip[] sounds;
    public GameObject[] player1Fighters;
    public GameObject[] player2Fighters;
    public static GameManager gm;

    const string Ready = "ready";
    const string KO1 = "ko1";
    const string KO2 = "ko2";
    const string Time_end = "time_end";
    private string currentState;
    int fighter1, fighter2;
    //------------------------------------------------------\\
    string scene;
    public bool isFighting = true;
    bool isPaused = false;
    Scene scn;
    AudioSource musicPlayer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;//bota a velocidade do jogo para normal
        ChangeFighterP1();
        ChangeFighterP2();
        MainManager.Instance.Pause();
        scn = SceneManager.GetActiveScene();
        anim = GetComponent<Animator>();
        musicPlayer = GetComponent<AudioSource>();
        gm = this;
        p1_energy = 0;
        p2_energy = 0;
        fgt1Handcap = MainManager.Instance.fgt1Handcap;
        fgt2Handcap = MainManager.Instance.fgt2Handcap;
        p1Wins = MainManager.Instance.p1_wins;
        p2Wins = MainManager.Instance.p2_wins;
        backgSelected = MainManager.Instance.background;
        Backgrounds();
        WinsCheck();
        if (p1Wins == 2 || p2Wins == 2)
        {

        }
        else
        {
            ChangeanimationState(Ready);
        }
    }
    void Update()
    {
        if (travar == 1)
        {
            canControl = false;
        }
        else if (travar == 0)
        {
            canControl = true;
        }
        if (timer > 0 && isFighting == true)
        {
            timer -= Time.deltaTime;
            timer_count.text = timer.ToString("00");
        }
        else if (timer <= 0)
        {
            ChangeanimationState(Time_end);
        }
        WinsCheck();
        fightEnded = MainManager.Instance.fightEnd;
    }
    public void PlayerResetLevel1()//cria uma fun��o que reseta a fase
    {
        ChangeanimationState(KO1);
    }
    public void PlayerResetLevel2()//cria uma fun��o que reseta a fase
    {
        ChangeanimationState(KO2);
    }
    public void ResetLevel()
    {   
        SceneManager.LoadScene(scn.name);
    }
    void WinsCheck()
    {
        if (p1Wins == 0)
        {
            p1_win1.gameObject.SetActive(false);
        }
        if (p2Wins == 0)
        {
            p2_win1.gameObject.SetActive(false);
        }
        if (p1Wins == 1)
        {
            p1_win1.gameObject.SetActive(true);
        }
        if (p2Wins == 1)
        {
            p2_win1.gameObject.SetActive(true);
        }
        if (p2Wins == 2)
        {
            isFighting = false;
            canControl = false;
            player_script.ps.canDoAnithing = false;
            MainManager.Instance.fightEnd = true;
            endGamePanel.gameObject.SetActive(true);
            timer_count.gameObject.SetActive(false);
        }
        else if (p1Wins == 2)
        {
            isFighting = false;
            canControl = false;
            player_script.ps.canDoAnithing = false;
            MainManager.Instance.fightEnd = true;
            endGamePanel.gameObject.SetActive(true);
            timer_count.gameObject.SetActive(false);
        }
    }
    void Backgrounds()
    {
        for ( int i=0 ; i<backgrounds.Length ; i++)
        {
            if (i == backgSelected)
            {
                backgrounds[i].gameObject.SetActive(true);
            }
            else
            {
                backgrounds[i].gameObject.SetActive(false);
            }
        }
    }
    void ChangeAndPlaySound(int track)
    {
        musicPlayer.Stop();
        musicPlayer.clip = sounds[track];
        if (MainManager.Instance.isPaused == false)
        {
            musicPlayer.Play();
        }

    }
    public void ChangeanimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }
    void LockOrUnlockPlayer(int trava)
    {
        travar = trava;
    }
    void PlayBackgroundSong(int track)
    {
        musicPlayer.Stop();
        musicPlayer.clip = sounds[track];
        if (MainManager.Instance.isPaused == false)
        {
            musicPlayer.Play();
        }
        musicPlayer.loop = true;
    }
    public void PauseAndDespauseSong()
    {
        if (MainManager.Instance.isPaused == false)
        {
             if (isPaused == false){
                musicPlayer.Pause();
                isPaused = true;
            }
            else 
            {
                musicPlayer.UnPause();
                isPaused = false;
                musicPlayer.loop = true;
            }
        }
    }
    public void ChangeFighterP1()
    {
        fighter1 = PlayerPrefs.GetInt("player1");
        for (int f=0; f<player1Fighters.Length; f++)
        {
            if (f == fighter1)
            {
                player1Fighters[f].gameObject.SetActive(true);
            }
            else 
            {
                player1Fighters[f].gameObject.SetActive(false);
            }
        }
    }
    public void ChangeFighterP2()
    {
        fighter2 = PlayerPrefs.GetInt("player2");
        for (int f=0; f<player2Fighters.Length; f++)
        {
            if (f == fighter2)
            {
                player2Fighters[f].gameObject.SetActive(true);
            }
            else 
            {
                player2Fighters[f].gameObject.SetActive(false);
            }
        }
    }
}
