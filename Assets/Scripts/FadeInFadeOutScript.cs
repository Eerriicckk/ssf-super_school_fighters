using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInFadeOutScript : MonoBehaviour
{
    public static Animator anim;
    public static FadeInFadeOutScript fIO;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fIO = this;
    }

    // Update is called once per frame
    public void FadeIn()
    {
        anim.Play("fadeIn");
    }
    public void FadeOut()
    {
        anim.Play("fadeOut");
    }
}
