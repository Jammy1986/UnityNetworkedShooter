using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour
{
    public const int MaxHealth = 100;
    [SyncVar(hook = "UpdateHealth")] private int _health = MaxHealth;

    private void Start()
    {
        UpdateHealth(_health);
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        _health -= amount;
        if (_health <= 0)
        {
            NetworkManager.singleton.ServerChangeScene("Lobby");
        }
    }
    
    private void UpdateHealth(int health)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        var healthText = GameObject.Find("HealthHud").GetComponent<Text>();
        healthText.text = "Health: " + health;
    }
}