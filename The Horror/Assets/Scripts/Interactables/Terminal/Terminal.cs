using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour {

    [Space (10)]
    public Text ConsoleText;
    public Transform CameraPoint;
    public Transform ActionHolder;
    [Space (10)]

    [Tooltip ("Actions that the Terminal will ejecute")]
    public List<TActionChain> ChainActions = new List<TActionChain>();
    public List<TActionInfo> InfoActions = new List<TActionInfo>();

    InputField inputField;
    string NewConsoleOutput;

    private void Awake()
    {
        inputField = GetComponentInChildren<InputField>();
        inputField.onEndEdit.AddListener(AcceptInput);
        ChainActions.AddRange (ActionHolder.GetComponentsInChildren<TActionChain>());
        InfoActions.AddRange (ActionHolder.GetComponentsInChildren<TActionInfo>());

    }

    void AcceptInput (string input)
    {
        input.ToLower();

        if (input.Length == 0)
            return;

        //add text to console
        ConsoleText.text += "\n" +input + ".";

        string consoleText = ConsoleText.text;

        //reset inputField
        inputField.text = null;
        inputField.ActivateInputField();

        if (input == "help")
        {
            ConsoleText.text += ReturnHelp();
            return;
        }

        if (CheckActionInputs(input))
            ConsoleText.text += "\n" + NewConsoleOutput;
        else
            ConsoleText.text += "\n" + "? syntax error";

    }

    bool CheckActionInputs (string input)
    {
        NewConsoleOutput = null;

        if (input.Contains("."))
        {
            if (input[0] != '.')
            {
                string ActionKey = input.Remove(input.IndexOf('.'), input.Length - input.IndexOf('.'));

                foreach (TActionInfo action in InfoActions)
                {
                    if (action.TriggerKey == ActionKey)
                    {
                        NewConsoleOutput = action.LookForKey(input.Replace(action.TriggerKey + ".", ""));
                    }
                }
            }
        }
        else
        {
            foreach (TActionChain action in ChainActions)
            {
                NewConsoleOutput = action.LookForKey(input);
                Debug.Log(input);
            }
        }

        if (NewConsoleOutput != null)
            return true;

        return false;
    }

    string ReturnHelp ()
    {
        string newOutput = null;

        newOutput += 
            "\n" +
            "\n" + "modules" + 
            "\n" + "-------------" + 
            "\n" + "to enter the action tree " +
            "\n" + "[ACTION.SUBKEY]" +
            "\n";

        foreach (TerminalAction action in ChainActions)
        {
            newOutput += " \n" + action.TriggerKey;
        }

        newOutput += "\n";

        return newOutput;
    }

    #region ENTER / EXIT

    //ENTER
    public void EnterTerminal ()
    {
        if (!OnCoroutine)
        {
            PlayerManager.Instace.ChangeState(PlayerManager.PlayerStates.OnTerminal);

            PlayerManager.Instace.CurrentTerminal = this;

            inputField.enabled = true;
            inputField.ActivateInputField();
            inputField.text = null;

            CameraPlayerPos = PlayerManager._Camera.transform.localPosition;

            MovePlayerCameraCO = StartCoroutine(MoveCameraTerminal());
        }
    }

    //EXIT
    public void ExitTerminal ()
    {
        if (!OnCoroutine)
        {
            PlayerManager.Instace.ChangeState(PlayerManager.PlayerStates.Free);

            PlayerManager.Instace.CurrentTerminal = null;

            inputField.enabled = false ;
            inputField.text = null;

            MovePlayerCameraCO = StartCoroutine(MoveCameraPlayer());

        }

    }

    bool OnCoroutine;
    Vector3 CameraPlayerPos;
    Coroutine MovePlayerCameraCO;
    IEnumerator MoveCameraTerminal ()
    {
        OnCoroutine = true;

        while (PlayerManager._Camera.transform.position != CameraPoint.position)
        {
            PlayerManager._Camera.transform.position = Vector3.MoveTowards(PlayerManager._Camera.transform.position, CameraPoint.position,5 * Time.deltaTime);
            PlayerManager._Camera.transform.LookAt(transform.position);

            yield return null;
        }

        if (PlayerManager._Camera.transform.position == CameraPoint.position)
        {
            OnCoroutine = false;
        }
    }

    IEnumerator MoveCameraPlayer()
    {
        OnCoroutine = true;

        while (PlayerManager._Camera.transform.localPosition != CameraPlayerPos)
        {
            PlayerManager._Camera.transform.localPosition = Vector3.MoveTowards(PlayerManager._Camera.transform.localPosition, CameraPlayerPos, 5 * Time.deltaTime);
            PlayerManager._Camera.transform.LookAt(transform.position);


            yield return null;
        }

        if (PlayerManager._Camera.transform.localPosition == CameraPlayerPos)
        {
            OnCoroutine = false;
            PlayerManager._Camera.transform.localRotation = Quaternion.identity;

        }
    }

    #endregion

}