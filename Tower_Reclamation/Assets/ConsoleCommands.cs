using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCommands : MonoBehaviour
{
    [SerializeField] InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField.text = "";
        inputField.gameObject.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipInputFieldActive()
    {
        inputField.gameObject.active = !inputField.gameObject.active;
    }

    public void ActivateConsoleCommands(string command)
    {

        string text = inputField.text.ToString();

        switch (text)
        {
            case "":
                break;
            case "1":
                print("1!!");
                break;
            case "2":
                print("2!!!");
                break;
            //case "":
            //    break;
            default:
                print("hit default command with : " + text + "  .T");
                break;
        }
    }
}
