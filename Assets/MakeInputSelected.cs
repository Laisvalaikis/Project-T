using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MakeInputSelected : MonoBehaviour
{
    private InputField textField;

    private void Start()
    {
        // Get the reference to the InputField component
        textField = GetComponent<InputField>();

        // Set the input field as selected
        textField.Select();
        textField.ActivateInputField();
        textField.placeholder.GetComponent<Text>().text = "NAME";

    }
}
