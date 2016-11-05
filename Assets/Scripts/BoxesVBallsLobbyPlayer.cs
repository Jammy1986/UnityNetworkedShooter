using UnityEngine.Networking;

public class BoxesVBallsLobbyPlayer : NetworkLobbyPlayer
{
    [SyncVar] private string _playerName;
    public string GetPlayerName()
    {
        return _playerName;
    }

    [Command]
    public void CmdSetPlayerName(string playerName)
    {
        _playerName = playerName;
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdSetPlayerName(GlobalState.Name);
    }

    public override void OnClientEnterLobby()
    {
        FindObjectOfType<LobbyGui>().AddPlayer(this);
    }

    public override void OnClientExitLobby()
    {
        FindObjectOfType<LobbyGui>().RemovePlayer(this);
    }
}