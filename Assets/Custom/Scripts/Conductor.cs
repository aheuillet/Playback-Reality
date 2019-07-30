using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    private GameObject SettingsPanel;
    private GameObject MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        SettingsPanel = GameObject.Find("SettingsPanel");
        MainMenu = GameObject.Find("MainMenu"); 
        SettingsPanel.SetActive(false);
    }

    public void ShowSettingsPanel() 
    {
        SettingsPanel.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void ShowMainMenu() 
    {
        MainMenu.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
