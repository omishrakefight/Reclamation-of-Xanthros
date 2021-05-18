using System;
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

    public void ActivateConsoleCommands(string commandCode)
    {

        string text = inputField.text.ToString();
        string[] commands;

        //split code into an array
        if (text.Contains(";")){
            commands = text.Split(';');
            // foreach loop, if contains (trim & get last.
        } else
        {
            commands = new string[] { text };
        }

        // for each command, im going to  check for command viability.
        foreach (string command in commands)
        {
            try
            {
                string currentCommand = command.Trim();

                // try to alter the level.
                if (currentCommand.ToLower().Contains("level"))
                {
                    currentCommand = currentCommand.ToLower().Replace("level", "");
                    currentCommand = currentCommand.Trim();

                    int level = int.Parse(currentCommand);
                    Singleton.Instance.SetLevel(level);
                }

                if (currentCommand.ToLower().Contains("zone"))
                {
                    currentCommand = currentCommand.ToLower().Replace("zone", "");
                    currentCommand = currentCommand.Trim();

                    int level = int.Parse(currentCommand);
                    Singleton.Instance.SetLevel(level);
                }


            }
            catch (Exception ex)
            {
                print("error in printing console command, not found: " + command);
            }
        }



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
