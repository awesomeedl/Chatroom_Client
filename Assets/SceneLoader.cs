using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Quit()
    {
        Application.Quit();
    }

    void Awake()
    {
        NetworkManager.LoginSuccess += LoadChatroom;
    }

    void LoadChatroom(string s)
    {
        SceneLoader.LoadScene("Chatroom");
    }
}