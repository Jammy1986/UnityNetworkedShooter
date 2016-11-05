using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour
{
    public const int MaxHealth = 100;
    [SyncVar] private int _health = MaxHealth;

    private void Start()
    {
        UpdateHealth();
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
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        var healthText = GameObject.Find("HealthHud").GetComponent<Text>();
        healthText.text = "Health: " + _health;
    }
}