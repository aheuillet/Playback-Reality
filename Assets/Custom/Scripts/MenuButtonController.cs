using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadMainScene() 
    {
        SceneManager.LoadScene("Test");
    }

    public void ExitApp() 
    {
        Application.Quit();
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
