using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MatchmakingGui : MonoBehaviour
{
    private List<MatchInfoSnapshot> _matchInfoSnapshots = new List<MatchInfoSnapshot>();

    private void Start()
    {
        NetworkManager.singleton.StartMatchMaker();
        StartCoroutine(RefreshInternetMatch());
    }

    private IEnumerator RefreshInternetMatch()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            FindInternetMatch();
        }
    }

    private void OnGUI()
    {
        var rowLabelStyle = new GUIStyle(GUI.skin.label)
        {
            padding = new RectOffset(15, 15, 15, 15)
        };
        var rowButtonStyle = new GUIStyle(GUI.skin.button)
        {
            margin = new RectOffset(15, 15, 15, 15)
        };
        GUI.Box(new Rect(50, Screen.height / 4.0f - 50, Screen.width - 100, Screen.height / 2.0f), "");
        //I think the 16 from the 116 accounts for the width of the vertical scroll bar, but haven't had time to investigate yet.
        GUI.BeginScrollView(new Rect(50, Screen.height / 4.0f - 50, Screen.width - 100, Screen.height / 2.0f), Vector2.down, new Rect(0, 0, Screen.width - 116, Screen.height));
        foreach (var matchInfoSnapshot in _matchInfoSnapshots)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(matchInfoSnapshot.name, rowLabelStyle, GUILayout.Width(200));
            GUILayout.Label(matchInfoSnapshot.currentSize + "/" + matchInfoSnapshot.maxSize, rowLabelStyle, GUILayout.Width(50));
            if (GUILayout.Button("Join Match", rowButtonStyle))
            {
                NetworkManager.singleton.matchMaker.JoinMatch(matchInfoSnapshot.networkId, "", "", "", 0, 0, OnJoinedInternetMatch);
            }
            GUILayout.EndHorizontal();
        }
        GUI.EndScrollView();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateInternetMatch()
    {
        NetworkManager.singleton.matchMaker.CreateMatch(GlobalState.Name, 8, true, "", "", "", 0, 0, OnCreatedInternetMatch);
    }

    private static void OnCreatedInternetMatch(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (!success)
        {
            Debug.Log("Failed to create a match, extendedInfo: '" + extendedInfo + "'.");
            return;
        }
        NetworkServer.Listen(responseData, 8754);

        GlobalState.MatchInfo = responseData;
        GlobalState.IsHost = true;
            
        SceneManager.LoadScene("Lobby");
    }

    private void FindInternetMatch()
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnFoundInternetMatch);
    }

    private void OnFoundInternetMatch(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        if (success)
        {
            if (responseData.Any())
            {
                _matchInfoSnapshots = responseData;
            }
            else
            {
                Debug.Log("No matches in requested room!");
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
        }
    }

    private void OnJoinedInternetMatch(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (success)
        {
//            GlobalState.NetworkId = responseData.networkId;
            GlobalState.MatchInfo = responseData;
            GlobalState.IsHost = false;
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.LogError("Join match failed, extendedInfo: '" + extendedInfo + "'.");
        }
    }
}