using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenuGui : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.singleton.matchMaker != null)
        {
            NetworkManager.singleton.StopMatchMaker();
        }
    }

    public void PlayAsABox()
    {
        GlobalState.IsBall = false;
        GoToLobby();
    }

    public void PlayAsABall()
    {
        GlobalState.IsBall = true;
        GoToLobby();
    }

    private static void GoToLobby()
    {
        SceneManager.LoadScene("Matchmaking");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}