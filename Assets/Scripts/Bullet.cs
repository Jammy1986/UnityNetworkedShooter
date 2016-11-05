using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //Have to go to the parent, since this object is either the box or the ball.
        var hitPlayer = other.transform.parent.gameObject.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Combat>().TakeDamage(10);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Hit object was not a player.");
        }
    }
}