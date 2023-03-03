using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginAndRegister : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] TMP_Text displayMessage;

    static string currentUsername;

    public static string GetCurrentUsername()
    {
        return currentUsername;
    } 

    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        currentUsername = username;
        
        usernameField.text = string.Empty;
        passwordField.text = string.Empty;

        NetworkManager.instance.SendLoginRequest(username, password);
    }

    public void Register()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        usernameField.text = string.Empty;
        passwordField.text = string.Empty;

        NetworkManager.instance.SendRegisterRequest(username, password);
    }

    public void DisplayMessage(string message)
    {
        displayMessage.text = message;
    }

    void Awake()
    {
        NetworkManager.LoginSuccess += DisplayMessage;
        NetworkManager.LoginFail += DisplayMessage;

        NetworkManager.RegisterSuccess += DisplayMessage;
        NetworkManager.RegisterFail += DisplayMessage;
    }
}
