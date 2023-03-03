using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageInput : MonoBehaviour
{
    [SerializeField] TMP_InputField messageInputField;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && messageInputField.text.Length > 0 )
        {
            SendNewMessage();
        }
    }

    public void SendNewMessage()
    {
        NetworkManager.instance.SendMessageRequest(messageInputField.text);
        messageInputField.text = string.Empty;
    }
}
