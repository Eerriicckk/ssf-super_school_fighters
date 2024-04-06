using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selectBackgroundScript : MonoBehaviour
{
    public GameObject done_button;
    Scene scn;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        scn = SceneManager.GetActiveScene();
        done_button.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectBackground(int bgSelected)
    {
        MainManager.Instance.background = bgSelected;
        done_button.gameObject.SetActive(true);
    }
    public void SelectRandomBackground()
    {
        int bgSelected = Random.Range(0,6);
        MainManager.Instance.background = bgSelected;
        done_button.gameObject.SetActive(true);
    }
    
    public void FuncChangeScene(string scene)
    {
        StartCoroutine(ChangeScene(scene));
    }

    IEnumerator ChangeScene(string scene)
    {
        anim.Play("fadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
