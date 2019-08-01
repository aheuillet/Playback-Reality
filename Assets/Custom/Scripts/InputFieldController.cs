using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldController : MonoBehaviour
{
    private TMP_InputField field;

    // Start is called before the first frame update
    void Start()
    {
        field = GetComponentInChildren<TMP_InputField>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() 
    {
        field.Select();
        //field.ActivateInputField();
    }
}
