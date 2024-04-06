using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    // criamos uma variavel do tipo scene chamada scn
    Scene scn;
    Animator anim;
    public bool paused = false, canPause = true;
    public GameObject pauseGamePanel;
    public static PauseMenuScript gm;
    public int p1Wins, p2Wins;
    // criamos uma variavel do tipo game manager chamada gm
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;//seta a velocidade do jogo pra 1
        anim = GetComponent<Animator>();
        //atribui a cena atual a variavel scn
        scn = SceneManager.GetActiveScene();
        pauseGamePanel.gameObject.SetActive(false);
        gm = this;
        p1Wins = MainManager.Instance.p1_wins;
        p2Wins = MainManager.Instance.p2_wins;
        if (p2Wins == 2 || p1Wins == 2)
        {
            anim.Play("fadeNothing");
        }
    }
    void Update()
    {
        
        canPause = GameManager.gm.canControl;
        if (paused == false && canPause == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }
        }
    }
    //cria uma fun��o que reseta o level
    public void ResetLevel()
    {
        SceneManager.LoadScene(scn.name);
        p1Wins = 0;
        p2Wins = 0;
        MainManager.Instance.p1_wins = p1Wins;
        MainManager.Instance.p2_wins = p2Wins;
    }
    public void PauseGame()//pausa o game
    {
        Time.timeScale = 0.0f;
        paused = true;
        pauseGamePanel.gameObject.SetActive(true);
        GameManager.gm.PauseAndDespauseSong();
    }
    public void QuitGame()//fecha o game
    {
        Application.Quit();
    }
    public void ResumeGame()//despausa o game
    {
        Time.timeScale = 1.0f;
        paused = false;
        pauseGamePanel.gameObject.SetActive(false);
        GameManager.gm.PauseAndDespauseSong();
    }
    public void FuncChangeScene(string scene)
    { 
        Debug.Log(scene);
        Time.timeScale = 1.0f;
        StartCoroutine(ChangeScene(scene));
    }
    IEnumerator ChangeScene(string scene)
    {
        Debug.Log(scene);
        anim.Play("fadeOut");
        yield return new WaitForSeconds(0.5f);
        if (scene == "menu" && MainManager.Instance.isPaused == false)
        {
            MainManager.Instance.Unpause();
        }
        SceneManager.LoadScene(scene);
    }
}
