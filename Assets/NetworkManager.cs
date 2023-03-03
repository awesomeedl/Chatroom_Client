using System.Collections;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkManager : MonoBehaviour
{
    // Singleton
    public static NetworkManager instance;
    
    public static readonly string url = "https://chatroom-api.azurewebsites.net";
    public static Action<string> LoginSuccess;
    public static Action<string> LoginFail;
    public static Action<string> RegisterSuccess;
    public static Action<string> RegisterFail;
    public static Action<string> ObtainChatHistory;

    void Awake()
    {
        // Singleton setup
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // for obtaining message history
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    // for login / registering user
    IEnumerator PostRequest(string url, Action<UnityWebRequest> callback, User user)
    {
        using (var request = new UnityWebRequest(
            url, 
            UnityWebRequest.kHttpVerbPOST,
            new DownloadHandlerBuffer(),
            new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(user)))
            ))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            callback(request);
        }
    }

    // for sending message
    IEnumerator PostRequest(string url, Message message)
    {
        using (var request = new UnityWebRequest(
            url, 
            UnityWebRequest.kHttpVerbPOST,
            new DownloadHandlerBuffer(),
            new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(message)))
            ))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void SendLoginRequest(string username, string password)
    {
        User user = new User { username = username, password = password };
        
        StartCoroutine(PostRequest(url + "/login", (UnityWebRequest request) =>
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                LoginSuccess?.Invoke(request.downloadHandler.text);   
            }
            else
            {
                LoginFail?.Invoke(request.downloadHandler.text);
            }
        }, user));
    }

    public void SendRegisterRequest(string username, string password)
    {
        User user = new User { username = username, password = password };
        
        StartCoroutine(PostRequest(url + "/register", (UnityWebRequest request) =>
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                RegisterSuccess?.Invoke(request.downloadHandler.text);   
            }
            else
            {
                RegisterFail?.Invoke(request.downloadHandler.text);
            }
        }, user));
    }

    public void SendMessageRequest(string message)
    {
        Message newMessage = new Message { username = LoginAndRegister.GetCurrentUsername(), message = message };

        StartCoroutine(PostRequest(url + "/send", newMessage));
    }

    public void SendMessageHistoiryRequest()
    {
        StartCoroutine(GetRequest(url + "/history", (UnityWebRequest request) =>
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                ObtainChatHistory?.Invoke(request.downloadHandler.text);   
            }
        }));
    }

    public void SendCheckNewMessageRequest(int Id)
    {
        StartCoroutine(GetRequest(url + "/history/" + Id, (UnityWebRequest request) =>
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                ObtainChatHistory?.Invoke(request.downloadHandler.text);   
            }
        }));
    }
}
