using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    const float maxHealth = 100f;
    const float turnRate = 10f;
    const float baseSpeed = 10f;

    public ParticleSystem OnFire;
    public AudioSource wakeSound;

    [SyncVar(hook = "OnChangeHealth")]
    private float currentHealth = maxHealth;
    [SyncVar]
    private float sailModifier = 0f;

    private Compass compass;
    private GameManager gameManager;
    private NetworkAnimator animator;
    private SkinnedMeshRenderer sailRenderer;
    private Vector3 spawnPoint;


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

    [SyncVar]
    private bool portCannonsReady = true;
    [SyncVar]
    private bool stbdCannonsReady = true;
    //private float portTimer = 0f;
    //private float stbdTimer = 0f;

    //[SyncVar(hook = "UpdateFlag")]
    //private int playerFlag;


    void Start()
    {

        compass = GameObject.Find("Compass").GetComponent<Compass>();
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
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

        //if (isLocalPlayer)
        //{
        //    playerFlag = PlayerPrefs.GetInt("player_flag");
        //    CmdSetFlag(playerFlag);

        //    foreach (Player player in GameObject.FindObjectsOfType<Player>())
        //    {
        //        if (!player.isLocalPlayer)
        //        player.RpcUpdateFlag();
        //    }
            //CmdUpdateFlag(PlayerPrefs.GetInt("player_flag"));
        //}
    }


    void Update()
    {
        //Make flag blow with wind direction
        float radians = gameManager.windDirection * Mathf.Deg2Rad;
        Vector3 degreeVector = new Vector3(-Mathf.Sin(radians), 0, -Mathf.Cos(radians));
        gameObject.GetComponentInChildren<Cloth>().externalAcceleration = degreeVector * gameManager.windSpeed * 10;

        if (!isLocalPlayer)
        {
            return;
        }

        CalcSpeedAndTurnRate();
        TurnToHeading();

        //Move ship
        //transform.position += (transform.forward * currentSpeed * Time.deltaTime);  //Kinematic Movement
        //GetComponent<Rigidbody>().velocity = (transform.forward * currentSpeed * 25); //Kinematic Movement
        GetComponent<Rigidbody>().AddForce(transform.forward * currentSpeed * 25);  //Non-kinematic movement; untestd
        //Debug.Log("Velocity: " + GetComponent<Rigidbody>().velocity);

        
      

        //Debug.Log("windSpeed: " + gameManager.windSpeed);
        //Debug.Log("windDirection: " + gameManager.windDirection);
        //Debug.Log("radians: " + radians);
        //Debug.Log("Degree vector: " + degreeVector);
        //Debug.Log("Final acceleration: " + degreeVector * gameManager.windSpeed);
        //Debug.Log("Flag index: " + playerFlag);
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

        windAngleModifier = windRelativeHeading / 180;

        currentSpeed = baseSpeed * gameManager.windSpeed * sailModifier * windAngleModifier;

        wakeSound.volume = sailModifier;
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


    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
        {
            return;
        }

        if (collision.gameObject.name == "Ship2(Clone)")
        {
            Vector3 relativeVelocity;

            Debug.Log("Provided Collision relative velocity: " + collision.relativeVelocity);
            relativeVelocity = GetComponent<Rigidbody>().velocity - collision.rigidbody.velocity;
            Debug.Log("Calculated Collision relative velocity: " + relativeVelocity);

            Debug.Log("Provided Collision Magnitude: " + collision.relativeVelocity.magnitude);
            Debug.Log("Calculated collision magnitude:" + relativeVelocity.magnitude);
            TakeDamage(relativeVelocity.magnitude * 10);
        }

        if (collision.collider.name == "Islands")
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
            Invoke("Respawn", 10);
        }
    }


    private void Respawn()
    {
        if (!isServer)
        {
            return;
        }
        Debug.Log("Delayed Respawn");

        //sailModifier = 0f;
        broken = false;
        sunk = false;
        onFire = false;
        currentHealth = maxHealth;

        RpcRespawn();

    }


    [ClientRpc]
    void RpcRespawn()
    {
        Debug.Log("Respawn started");
        OnFire.Stop();

        if (isLocalPlayer)
        {

            animator.SetTrigger("Respawn");
            if (NetworkServer.active)
                GetComponent<Animator>().ResetTrigger("Respawn");
            Debug.Log("Animator reset.");

            transform.position = spawnPoint;
            Debug.Log("LocalPlayer Respawned");

        }
        Debug.Log("Respawn health: " + currentHealth.ToString());
        Debug.Log("Respawned");
    }


    //private void DelayedReset()
    //{
    //    animator.SetTrigger("Respawn");

    //    transform.position = spawnPoint;

    //    sailModifier = 0f;
    //    broken = false;
    //    sunk = false;
    //    onFire = false;
    //    currentHealth = maxHealth;

    //    if (NetworkServer.active)
    //        GetComponent<Animator>().ResetTrigger("Respawn");
    //    Debug.Log("Animator reset.");

    //    transform.position = spawnPoint;
    //}

    //private void ResetAttributes()
    //{
    //    if (!isServer)
    //    {
    //        return;
    //    }

    //    broken = false;
    //    sunk = false;
    //    onFire = false;
    //    currentHealth = maxHealth;
    //}


    [ClientRpc]
    private void RpcFireEffects(bool firePort)
    {
        if (firePort)
        {
            foreach (Cannon cannon in portCannons.cannons)
            {
                cannon.smoke.Play();
                cannon.explosion.Play();
                AudioSource.PlayClipAtPoint(cannon.sound, cannon.transform.position);
            }
        }
        else
        {
            foreach (Cannon cannon in starboardCannons.cannons)
            {
                cannon.smoke.Play();
                cannon.explosion.Play();
                AudioSource.PlayClipAtPoint(cannon.sound, cannon.transform.position);
            }
        }

    }


    private void ResetPortCannons()
    {
        portCannonsReady = true;
    }


    private void ResetStbdCannons()
    {
        stbdCannonsReady = true;
    }


    //[Command]
    //private void CmdSetFlag(int flag)
    //{
    //    playerFlag = flag;
    //    GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().CmdUpdateFlags();
    //}

    //private void UpdateFlag(int playerFlag)
    //{
    //    GetComponentInChildren<Flag>().SetFlag(GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().flags[playerFlag]);
    //}


    //==============================================================================================================================
    //=========================================================== PUBLIC ===========================================================
    //==============================================================================================================================


    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<Camera>().enabled = true;
        GetComponentInChildren<AudioListener>().enabled = true;
        //playerFlag = PlayerPrefs.GetInt("player_flag");
        //CmdSetFlag(PlayerPrefs.GetInt("player_flag"));
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
        if (portCannonsReady == true)
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

            portCannonsReady = false;

            Invoke("ResetPortCannons", 3);
        }
    }


    [Command]
    public void CmdFireStarboard()
    {
        if (stbdCannonsReady == true)
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

            stbdCannonsReady = false;

            Invoke("ResetStbdCannons", 3);
        }
    }


    public void TakeDamage(float damage)
    {
        if (!isServer)
        {
            return;
        }
        Debug.Log("Taking Damage");
        Debug.Log("currentHealth: " + currentHealth);
        if (currentHealth > 0f)
        {
            currentHealth -= damage;
            Debug.Log("currentHealth: " + currentHealth);

            if (currentHealth <= 0f)
            {
                Debug.Log("Invoke Respawn");
                Invoke("Respawn", 10);

                //currentHealth = maxHealth;

                // called on the Server, but invoked on the Clients
                //RpcRespawn();
            }
        }
    }

    //[ClientRpc]
    //public void RpcUpdateFlag()
    //{
    //    GetComponentInChildren<Flag>().SetFlag(gameManager.flags[playerFlag]);
    //}
}
