using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class cut_scene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public GameObject text;

    public void TimeLine()
    {
            playableDirector.Pause();
            text.SetActive(true);
    }


    void Update()
    {
        if (Input.GetKey("f"))
        {
            playableDirector.Play();
            text.SetActive(false);
        }
    }


    public void StartGame()
    {
        Application.LoadLevel("Game");
    }

   
}
