using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public ParticleSystem OnFire;

    private static float maxHealth = 100f;

    [SyncVar(hook = "OnChangeHealth")]
    private float health = maxHealth;
    private float sailModifier = 0f;

    private Compass compass;
    private GameManager gameManager;
    private NetworkAnimator animator;
    private SkinnedMeshRenderer sailRenderer;
    private Vector3 spawnPoint;

    private float turnRate = 10f;
    private float baseSpeed = 0.75f;
    [SyncVar]
    private float currentSpeed;
    private float windAngleModifier;
    private float windRelativeHeading;

    private CannonManager portCannons;
    private CannonManager starboardCannons;

    [SyncVar]
    private bool broken = false;
    [SyncVar]
    private bool sunk = false;
    [SyncVar]
    private bool onFire = false;


    void Start ()
    {

        compass = GameObject.Find("Compass").GetComponent<Compass>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponent<NetworkAnimator>();

        foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer.name == "Sails")
            {
                sailRenderer = renderer;
            }
        }

        spawnPoint = transform.position;

        foreach (CannonManager cannonManager in GetComponentsInChildren<CannonManager>())
        {
            if (cannonManager.name == "Port Cannons")
            {
                portCannons = cannonManager;
            }
            else
            {
                starboardCannons = cannonManager;
            }
        }
	}
	

	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CalcSpeedAndTurnRate();
        TurnToHeading();
        transform.position += transform.forward*currentSpeed*Time.deltaTime;
    }


    private void CalcSpeedAndTurnRate()
    {
        if (!broken)
        {
            sailModifier = (100 - sailRenderer.GetBlendShapeWeight(0)) / 100;
        }
        else if (sailModifier > 0)
        {
            sailModifier = sailModifier * .25f * Time.deltaTime; 
        }

        if ((transform.eulerAngles.y - gameManager.windDirection >= 0) && (transform.eulerAngles.y - gameManager.windDirection <= 180))
        {
            windRelativeHeading = transform.eulerAngles.y - gameManager.windDirection;
        }
        else if ((transform.eulerAngles.y - gameManager.windDirection < 0) && (transform.eulerAngles.y - gameManager.windDirection >= -180))
        {
            windRelativeHeading = Mathf.Abs(transform.eulerAngles.y - gameManager.windDirection);
        }
        else if (transform.eulerAngles.y - gameManager.windDirection < -180)
        {
            windRelativeHeading = (transform.eulerAngles.y - gameManager.windDirection) + 360;
        }
        else
        {
            windRelativeHeading = 360 - (transform.eulerAngles.y - gameManager.windDirection);
        }

        windAngleModifier = windRelativeHeading/180;

        currentSpeed = baseSpeed * gameManager.windSpeed * sailModifier * windAngleModifier;
    }

    
    private void TurnToHeading()
    {
        if ((transform.eulerAngles.y < compass.GetHeading() - 0.5f) || (transform.eulerAngles.y > compass.GetHeading() + 0.5f))
        {
            if ((compass.GetHeading() - transform.eulerAngles.y >= 0) && (compass.GetHeading() - transform.eulerAngles.y <= 180))
            {
                transform.eulerAngles += new Vector3(0f, turnRate * Time.deltaTime, 0f);
            }
            else if ((compass.GetHeading() - transform.eulerAngles.y < 0) && (compass.GetHeading() - transform.eulerAngles.y >= -180))
            {
                transform.eulerAngles -= new Vector3(0f, turnRate * Time.deltaTime, 0f);
            }
            else if (compass.GetHeading() - transform.eulerAngles.y < -180)
            {
                transform.eulerAngles += new Vector3(0f, turnRate * Time.deltaTime, 0f);
            }
            else
            {
                transform.eulerAngles -= new Vector3(0f, turnRate * Time.deltaTime, 0f);
            }
        }
    }


    private void OnChangeHealth(float currentHealth)
    {
        if (currentHealth <= 0 & !sunk)
        {
            Debug.Log("Sink");
            animator.SetTrigger("Sink");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("Sink");

            sunk = true;

            Invoke("Respawn", 10);
        }
        else if (currentHealth < 25f && !broken)
        {
            Debug.Log("Break");
            animator.SetTrigger("Break");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("Break");

            if (OnFire.isStopped || OnFire.isPaused)
            {
                OnFire.Play();
            }
            broken = true;
        }
        else if (currentHealth <= 50f && !onFire)
        {
            Debug.Log("OnFire");
            OnFire.Play();
            onFire = true;
        }
    }

    //[ClientRpc]
    private void Respawn()
    {
        Debug.Log("Respawn started");
        OnFire.Stop();
                
        if (isLocalPlayer)
        {
            Debug.Log("LocalPlayer Respawn Started");
            //broken = false;
            //sunk = false;
            //onFire = false;
            //health = maxHealth;
            animator.SetTrigger("Respawn");
            transform.position = spawnPoint;


            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("Respawn");

            Debug.Log("LocalPlayer Respawned");
        }
        Debug.Log("Respawned");
    }

    private void ResetAttributes()
    {
        if (!isServer)
        {
            return;
        }

        broken = false;
        sunk = false;
        onFire = false;
        health = maxHealth;
    }

    [ClientRpc]
    private void RpcFireEffects(bool firePort)
    {
        if (firePort)
        {
            foreach (Cannon cannon in portCannons.cannons)
            {
                cannon.smoke.Play();
                cannon.explosion.Play();
            }
        }
        else
        {
            foreach (Cannon cannon in starboardCannons.cannons)
            {
                cannon.smoke.Play();
                cannon.explosion.Play();
            }
        }

    }


    //==============================================================================================================================
    //=========================================================== PUBLIC ===========================================================
    //==============================================================================================================================


    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<Camera>().enabled = true;
        GetComponentInChildren<AudioListener>().enabled = true;
    }


    public void LowerSail()
    {
        if (sailModifier < .5f)
        {
            Debug.Log("LowerHalfSail");
            animator.SetTrigger("LowerHalfSail");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("LowerHalfSail");
        }
        else if (sailModifier < 1f)
        {
            Debug.Log("LowerFullSail");
            animator.SetTrigger("LowerFullSail");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("LowerFullSail");
        }
    }
    
    
    public void RaiseSail()
    {
        if (sailModifier > .5f)
        {
            Debug.Log("RaiseFullSail");
            animator.SetTrigger("RaiseFullSail");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("RaiseFullSail");
        }
        else if (sailModifier > 0f)
        {
            Debug.Log("RaiseHalfSail");
            animator.SetTrigger("RaiseHalfSail");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("RaiseHalfSail");
        }
    }


    [Command]
    public void CmdFirePort()
    {
        Vector3 direction = (transform.right * -1) + (Vector3.up * .1f);

        foreach (Cannon cannon in portCannons.cannons)
        {
            GameObject shot = Instantiate(cannon.cannonBall, cannon.transform.position, cannon.transform.rotation) as GameObject;
            shot.GetComponent<CannonBall>().shootingPlayer = GetComponent<Player>();
            shot.GetComponent<Rigidbody>().velocity = direction * cannon.shotSpeed;
            NetworkServer.Spawn(shot);
        }
        RpcFireEffects(true);
    }


    [Command]
    public void CmdFireStarboard()
    {
        Vector3 direction = (transform.right + (Vector3.up * .1f));

        foreach (Cannon cannon in starboardCannons.cannons)
        {
            GameObject shot = Instantiate(cannon.cannonBall, cannon.transform.position, cannon.transform.rotation) as GameObject;
            shot.GetComponent<CannonBall>().shootingPlayer = GetComponent<Player>();
            shot.GetComponent<Rigidbody>().velocity = direction * cannon.shotSpeed;
            NetworkServer.Spawn(shot);
        }
        RpcFireEffects(false);
    }


    public void TakeDamage(float damage)
    {
        if (!isServer)
        {
            return;
        }
        if (health > 0)
        {
            health -= damage;            
        }
    }
}
