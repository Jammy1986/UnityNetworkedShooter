using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BoxesVBallsLobbyManager : NetworkLobbyManager
{
    private byte FindSlot()
    {
        for (byte index = 0; (int)index < maxPlayers; ++index)
        {
            if (lobbySlots[index] == null)
                return index;
        }
        return byte.MaxValue;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (conn.playerControllers.Count > 0 && SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            var slot = FindSlot();
            var player = OnLobbyServerCreateLobbyPlayer(conn, playerControllerId) ?? (GameObject)Instantiate(lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            var component = player.GetComponent<NetworkLobbyPlayer>();
            component.slot = slot;
            lobbySlots[slot] = component;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        else if (SceneManager.GetSceneAt(0).name == lobbyScene)
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