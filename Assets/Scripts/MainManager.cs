using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance; 
    public int p1_wins;
    public int p2_wins;
    public int gameType;
    public int difficulty;
    public int background;
    public int track = 0;
    public int inputType = 0;// 0 = teclado, 1 = controle
    public float fgt1Handcap, fgt2Handcap;
    public bool fightEnd = false;
    //--------------------------------------//
    public AudioClip[] songs;
    //--------------------------------------//
    string scn;
    int numberToBool;
    public bool isPaused = false;
    AudioSource musicPlayer;
    /*game type
    1 = singleplayer
    2 = multiplayer
    3 = tournament pvp_scene
    */
    
    private void Awake()
    {
        scn = SceneManager.GetActiveScene().name;
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = songs[track];
        musicPlayer.Play();
        numberToBool = PlayerPrefs.GetInt("pausedSong");
        if (numberToBool == 1){
            musicPlayer.Pause();
            isPaused = true;
            PlayerPrefs.SetInt("pausedSong", 1);
        }
        else if (numberToBool == 0)
        {
            musicPlayer.UnPause();
            isPaused = false;
            musicPlayer.loop = true;
            PlayerPrefs.SetInt("pausedSong", 0);
        }
        if (scn == "pvp_scene")
        {
            Debug.Log("teste");
            musicPlayer.Pause();
        }
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //mesa do dj\\
    public void Pause()
    {
        musicPlayer.Pause();
    }
    public void Unpause()
    {
        musicPlayer.Play();
        musicPlayer.loop = true;
    }
    public void ChangeSong(int song)
    {
        if (track == song) return;
        track = song;
        musicPlayer.Stop();
        musicPlayer.clip = songs[song];
        musicPlayer.Play();
        musicPlayer.loop = true;
    }
    public void PauseAndDespauseSong()
    {
        if (isPaused == false){
            musicPlayer.Pause();
            isPaused = true;
            PlayerPrefs.SetInt("pausedSong", 1);
            numberToBool = PlayerPrefs.GetInt("pausedSong");
        }
        else 
        {
            musicPlayer.UnPause();
            isPaused = false;
            musicPlayer.loop = true;
            PlayerPrefs.SetInt("pausedSong", 0);
            numberToBool = PlayerPrefs.GetInt("pausedSong");
        }
    }
    
}
