using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
public class cut_scene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public void TimeLine()
    {
            playableDirector.Pause();
    }


    void Update()
    {
        if (Input.GetKey("f"))
        {
            playableDirector.Play();
        }
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

   
}
