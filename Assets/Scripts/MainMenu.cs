using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public void PlayMap1(){
        SceneManager.LoadScene("Map1");
    }

    public void PlayMap2(){
        //SceneManager.LoadSceneAsync("Map2");
    }

    public void Leave(){
        Application.Quit();
    }

}
