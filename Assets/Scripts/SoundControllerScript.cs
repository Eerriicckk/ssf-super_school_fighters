using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ChangeTrack(int song)
    {
        MainManager.Instance.ChangeSong(song);
    }
    public void PauseSong()
    {
        MainManager.Instance.PauseAndDespauseSong();
    }
}
