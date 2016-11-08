using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyGui : MonoBehaviour
{
    private readonly List<BoxesVBallsLobbyPlayer> _lobbyPlayers = new List<BoxesVBallsLobbyPlayer>();

    public void AddPlayer(BoxesVBallsLobbyPlayer customNetworkLobbyPlayer)
    {
        _lobbyPlayers.Add(customNetworkLobbyPlayer);
    }

    public void RemovePlayer(BoxesVBallsLobbyPlayer lobbyPlayer)
    {
        _lobbyPlayers.Remove(lobbyPlayer);
    }

    private void Awake()
    {
        if(NetworkManager.singleton == null || NetworkManager.singleton.isNetworkActive)
        {
            return;
        }

        if (GlobalState.IsHost)
        {
            NetworkManager.singleton.StartHost(GlobalState.MatchInfo);
        }
        else
        {
            NetworkManager.singleton.StartClient(GlobalState.MatchInfo);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(50, 50, Screen.width - 100, 200), "");
        var rowLabelStyle = new GUIStyle(GUI.skin.label)
        {
            padding = new RectOffset(15, 15, 15, 15)
        };
        var rowButtonStyle = new GUIStyle(GUI.skin.button)
        {
            margin = new RectOffset(15, 15, 15, 15)
        };
        GUI.BeginScrollView(new Rect(50, 50, Screen.width - 100, 200), Vector2.down, new Rect(0, 0, 180, 300));
        foreach (var lobbyPlayer in _lobbyPlayers)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(lobbyPlayer.GetPlayerName(), rowLabelStyle);
            GUILayout.Label(GlobalState.IsBall ? "Ball" : "Box", rowLabelStyle);
            if (lobbyPlayer.isLocalPlayer)
            {
                if (GUILayout.Button(lobbyPlayer.readyToBegin ? "Not Ready" : "Ready", rowButtonStyle))
                {
                    if (lobbyPlayer.readyToBegin)
                    {
                        lobbyPlayer.SendNotReadyToBeginMessage();
                    }
                    else
                    {
                        lobbyPlayer.SendReadyToBeginMessage();
                    }
                }
            }
            else
            {
                GUILayout.Label(lobbyPlayer.readyToBegin ? "Ready" : "Not ready");
            }
            GUILayout.EndHorizontal();
        }
        GUI.EndScrollView();
    }

    public void ReturnToMainMenu()
    {
        NetworkManager.singleton.matchMaker.DestroyMatch(GlobalState.MatchInfo.networkId, 0, OnDestroyedMatch);
        if (GlobalState.IsHost)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }

        NetworkServer.Reset();
        Destroy(NetworkManager.singleton.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    private static void OnDestroyedMatch(bool success, string extendedInfo)
    {
    }
}