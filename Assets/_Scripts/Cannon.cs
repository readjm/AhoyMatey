using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Cannon : NetworkBehaviour
{
    
    public GameObject cannonBall;

    public float shotSpeed = 50f;
    public ParticleSystem smoke;
    public ParticleSystem explosion;


    void Start()
    {
        foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            if (particleSystem.name == "Smoke")
            {
                smoke = particleSystem;
            }
            else if (particleSystem.name == "Explosion")
            {
                explosion = particleSystem;
            }
        }
    }


    //==============================================================================================================================
    //=========================================================== PUBLIC ===========================================================
    //==============================================================================================================================

    
    //public void Fire(Vector3 direction)
    //{
    //    GameObject shot = Instantiate(cannonBall, transform.position, transform.rotation) as GameObject;
    //    shot.GetComponent<CannonBall>().shootingPlayer = GetComponentInParent<Player>();
    //    shot.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
    //    NetworkServer.Spawn(shot);

    //    smoke.Play();
    //    explosion.Play();

    //}

    //[ClientRpc]
    //public void RpcFireEffects()
    //{
    //    smoke.Play();
    //    explosion.Play();
    //}
}
