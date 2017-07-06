using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CannonBall : NetworkBehaviour
{

    public Player shootingPlayer;
    public ParticleSystem splash;
    public ParticleSystem cannonHit;
    public AudioSource hitSound;
    public AudioSource splashSound;

    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }

        var hit = collision.gameObject;
        
        if (hit.name == "Sea")
        {
            RpcSplash();
        }

        Debug.Log(hit.name);

        if (hit.name == "Ship2(Clone)" && !(shootingPlayer.Equals(hit.GetComponentInParent<Player>())))
        {
            var player = hit.GetComponentInParent<Player>();

            //Debug.Log("Ship Hit");

            if (player != null)
            {
                //Debug.Log("Player Damaged");
                player.TakeDamage(damage);
            }

            RpcHit();
        }
    }

    [ClientRpc]
    private void RpcSplash()
    {
        GameObject newSplash = Instantiate(splash, transform.position, Quaternion.Euler(new Vector3(270, 0, 0))).gameObject as GameObject;
        splashSound.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        GameObject.Destroy(gameObject, 2f);

    }


    [ClientRpc]
    private void RpcHit()
    {
        Debug.Log("RpcHit()");
        GameObject newHit = Instantiate(cannonHit, transform.position, Quaternion.identity).gameObject as GameObject;
        hitSound.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        GameObject.Destroy(gameObject, 2f);
    }
}
