using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.Linq;

public class ChatManager : MonoBehaviour
{
    // How many times a second the client checks for new message
    private readonly float messagePollingRate = 10.0f;

    private readonly Color ownColor = new Color32(160, 193, 255, 255);

    [SerializeField] GameObject gridContent; 
    [SerializeField] GameObject tmpPrefab;

    private int lastMessageId = 0;
    private bool readyForNewMessage = true;
    void OnEnable()
    {
        NetworkManager.ObtainChatHistory += UpdateMessages;
    }

    void OnDisable()
    {
        NetworkManager.ObtainChatHistory -= UpdateMessages;
    }
    
    void Start()
    {
        StartCoroutine(UpdateMessageCoroutine());
    }

    void DisplayMessage(Message message)
    {
        var newMessageObj = Instantiate(tmpPrefab, gridContent.transform);

        newMessageObj.GetComponent<TMP_Text>().text = string.Format("{0}:\n{1}", message.username, message.message);

        if(string.Equals(message.username, LoginAndRegister.GetCurrentUsername()))
        {
            newMessageObj.GetComponent<TMP_Text>().color = ownColor;
        }
    }

    IEnumerator UpdateMessageCoroutine()
    {
        while(true)
        {
            if(readyForNewMessage)
            {
                NetworkManager.instance.SendCheckNewMessageRequest(lastMessageId);
                readyForNewMessage = false; // Don't send new requests while current one is pending
            }
            else
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.0f / messagePollingRate);
        }
    }

    public void UpdateMessages(string messages)
    {
        var messageList = JsonConvert.DeserializeObject<List<Message>>(messages);
        
        string s = string.Empty;

        if (messageList.Count > 0) // There is new message
        {
            messageList.ForEach(DisplayMessage);

            lastMessageId = messageList.Max((a) => a.Id); // Update the Id of the last message received
        }

        readyForNewMessage = true; // Ready to send new requests
    }
}
