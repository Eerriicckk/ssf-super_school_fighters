using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    Scene scn;
    Animator anim;
    string scene;
    void Start()
    {
        scn = SceneManager.GetActiveScene();
        anim = GetComponent<Animator>();
    }
    public void SingleplayerGame(string scene)
    {
        MainManager.Instance.gameType = 1;
        StartCoroutine(ChangeCharacter(scene));
    }
    public void OptionScene(string scene)
    {
        StartCoroutine(ChangeCharacter(scene));
    }
    public void QuitGame()
    {
        Application.Quit();

    }
    IEnumerator ChangeCharacter(string scene)
    {
        anim.Play("fadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
