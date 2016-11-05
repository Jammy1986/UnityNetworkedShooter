using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BoxesVBallsLobbyManager : NetworkLobbyManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
        }
        else if (SceneManager.GetSceneAt(0).name == playScene)
        {
            var functionPointer = typeof(NetworkManager).GetMethod("OnServerAddPlayer", new [] { typeof(NetworkConnection), typeof(short) }).MethodHandle.GetFunctionPointer();
            var onServerAddPlayerPointer = (Action<NetworkConnection, short>) Activator.CreateInstance(typeof(Action<NetworkConnection, short>), this, functionPointer);
            onServerAddPlayerPointer(conn, playerControllerId);
        }
    }
}