using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonBehavior : MonoBehaviour
{
    private GameObject title;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        title = GameObject.Find("MainTitle");
        parent = GameObject.Find("OkButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MyAction() 
    {
        Debug.Log("Coucou");
        title.SetActive(false);
        parent.SetActive(false);
    }
        
}
