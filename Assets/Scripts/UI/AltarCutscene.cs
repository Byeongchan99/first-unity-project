using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AltarCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            playableDirector.gameObject.SetActive(true);
            playableDirector.Play();
        }
    }
}
