using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class select_chacter : MonoBehaviour
{
    public int player = 1;
    public int gameType;
    public Text selectCharacterText;
    public GameObject done_button;
    //------------------------------------------------------\\
    int p1, p2;
    string scene;
    //------------------------------------------------------\\
    Scene scn;
    Animator anim;
    public static select_chacter gm;
    void Start()
    {
        anim = GetComponent<Animator>();
        DeactivateP1tags();
        DeactivateP2tags();
        scn = SceneManager.GetActiveScene();//bota a cena atual como o valor da var scn
        player = 1;
        done_button.gameObject.SetActive(false);
        selectCharacterText.text = "Player " + player.ToString("1")  + ", select your fighter!";
        gm = this;
        anim.Play("fadeIn");
    }
    // Update is called once per frame
    void Update()
    {
        p1 = PlayerPrefs.GetInt("player1");
        p2 = PlayerPrefs.GetInt("player2");
        
    }
    
    public void DeactivateP1tags()
    {
        GameObject[] p1tag = GameObject.FindGameObjectsWithTag("p1Tag");
        foreach (GameObject tag in p1tag)
        {
            tag.SetActive(false);
        }
    }

    public void DeactivateP2tags()
    {
        GameObject[] p2tag = GameObject.FindGameObjectsWithTag("p2Tag");
        foreach (GameObject tag in p2tag)
        {
            tag.SetActive(false);
        }
    }

    public void SelectCharacter(int selected_character)
    {
        PlayerPrefs.SetInt("player" + player, selected_character);
        done_button.gameObject.SetActive(true);
    }
    
    public void ChangePlayer()
    {
        player += 1;
        if (player >= 3)
        {

            MainManager.Instance.p1_wins = 0;
            MainManager.Instance.p2_wins = 0;
            StartCoroutine(ChangeScene("selectBackground"));
        }
        selectCharacterText.text = "Player " + player.ToString("2")  + ", select your fighter!";
        done_button.gameObject.SetActive(false);
    }
    
    public void FuncChangeScene(string scene)
    {
        Debug.Log(scene);
        StartCoroutine(ChangeScene(scene));
    }
    
    public void P1Handcap(float handcap)
    {
        MainManager.Instance.fgt1Handcap = handcap;
        DeactivateP1tags();
    }

    public void P2Handcap(float handcap)
    {
        MainManager.Instance.fgt2Handcap = handcap;
        DeactivateP2tags();
    }

    IEnumerator ChangeScene(string scene)
    {
        anim.Play("fadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
