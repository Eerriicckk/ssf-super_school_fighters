using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerButonScript : MonoBehaviour
{
    public GameObject p1Tag, p2Tag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ActivatePtag()
    {
        if (select_chacter.gm.player == 1)
        {
            select_chacter.gm.DeactivateP1tags();
            p1Tag.SetActive(true);
        }
        else if (select_chacter.gm.player == 2)
        {
            select_chacter.gm.DeactivateP2tags();
            p2Tag.SetActive(true);
        }
        
    }
    public void ActivateButtonTag(int player)
    {
        if (player == 1)
        {
            select_chacter.gm.DeactivateP1tags();
            p1Tag.SetActive(true);
        }
        else if (player == 2)
        {
            select_chacter.gm.DeactivateP2tags();
            p2Tag.SetActive(true);
        }
        
    }
}
