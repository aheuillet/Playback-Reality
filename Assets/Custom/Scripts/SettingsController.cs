using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dweiss;

public class SettingsController : MonoBehaviour
{
    private TMP_InputField ServerIP;
    private TMP_InputField PortNumber;
    private TMP_InputField DatabaseName;
    private Settings Settings;

    // Start is called before the first frame update
    void Start()
    {
        ServerIP = GameObject.Find("ServerIPInputField").GetComponent<TMP_InputField>();
        PortNumber = GameObject.Find("PortNumberInputField").GetComponent<TMP_InputField>();
        DatabaseName = GameObject.Find("DatabaseNameInputField").GetComponent<TMP_InputField>();
        Settings = GameObject.Find("SettingsSingleton").GetComponent<Settings>();

        ServerIP.text = Settings.ServerIP;
        PortNumber.text = Settings.PortNumber;
        DatabaseName.text = Settings.DatabaseName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteConfig() 
    {
        Settings.ServerIP = ServerIP.text;
        Settings.PortNumber = PortNumber.text;
        Settings.DatabaseName = DatabaseName.text;
    }
}
