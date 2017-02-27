using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CannonBall : NetworkBehaviour
{

    public Player shootingPlayer;
    public ParticleSystem splash;
    public ParticleSystem cannonHit;
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        
        if (hit.name == "Sea")
        {
            RpcSplash();
        }

        if ((hit.name == "BoatCollider") && !(shootingPlayer.Equals(hit.GetComponentInParent<Player>())))
        {
            var player = hit.GetComponentInParent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            RpcHit();
        }
    }

    [ClientRpc]
    private void RpcSplash()
    {
        GameObject newSplash = Instantiate(splash, transform.position, Quaternion.Euler(new Vector3(270, 0, 0))) as GameObject;
        Destroy(gameObject);
    }


    [ClientRpc]
    private void RpcHit()
    {
        GameObject newHit = Instantiate(cannonHit, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }
}
