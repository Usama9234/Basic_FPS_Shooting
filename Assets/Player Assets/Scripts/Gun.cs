using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public static bool playerHitEnemy=false;
    public enum ShootState
    {
        Ready,
        Shooting,
        Reloading
    }

    Animator animator;
    Recoil Recoil_Script;
    SoundsManager soundsManager;

    // How far forward the muzzle is from the centre of the gun
    private float muzzleOffset;

    [Header("Magazine")]
    //---- public GameObject round;
    public int ammunition;

    [Range(0.5f, 10)] public float reloadTime;

    [SerializeField] private int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)] public float fireRate;

    // The number of rounds fired each shot
    //public int roundsPerShot;
    public float weaponRange;

    //[Range(0.5f, 100)] public float roundSpeed;

    // The maximum angle that the bullet's direction can vary,
    // in both the horizontal and vertical axes
    //[Range(0, 45)] public float maxRoundVariation;

    private ShootState shootState = ShootState.Ready;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    [Header("Recoil")]
    //Hipfire Recoil
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    //Settings
    public float snappiness;
    public float returnSpeed;

    //Kickback
    public float kickBack;
    internal Vector3 initialPosition;

    

    void Start()
    {
        Recoil_Script = GetComponentInParent<Recoil>();
        soundsManager = GetComponent<SoundsManager>();

        initialPosition = transform.localPosition;
        animator = GetComponentInParent<Animator>();
        try
        {
            muzzleOffset = GetComponent<Renderer>().bounds.extents.z;
        }
        catch
        {
            muzzleOffset = GetComponentInChildren<Renderer>().bounds.extents.z;
        }
        
        remainingAmmunition = ammunition;
        
    }

    void Update()
    {
        switch (shootState)
        {
            case ShootState.Shooting:
                // If the gun is ready to shoot again...
                if (Time.time > nextShootTime)
                {
                    shootState = ShootState.Ready;
                }
                break;
            case ShootState.Reloading:
                // If the gun has finished reloading...
                if (Time.time > nextShootTime)
                {
                    animator.SetBool("Hide", false);
                    remainingAmmunition = ammunition;
                    shootState = ShootState.Ready;
                }
                break;
        }
    }

    /// Attempts to fire the gun
    public void Shoot()
    {
        // Checks that the gun is ready to shoot
        if (shootState == ShootState.Ready)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponRange))
            {
                HandleHit(hit);
            }
            //for (int i = 0; i < roundsPerShot; i++)
            //{
                //    // Instantiates the round at the muzzle position
                //    GameObject spawnedRound = Instantiate(
                //        round,
                //        transform.position + transform.forward * muzzleOffset,
                //        transform.rotation
                //    );

                //    // Add a random variation to the round's direction
                //    spawnedRound.transform.Rotate(new Vector3(
                //        Random.Range(-1f, 1f) * maxRoundVariation,
                //        Random.Range(-1f, 1f) * maxRoundVariation,
                //        0
                //    ));

                //    Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();
                //    rb.velocity = spawnedRound.transform.forward * roundSpeed;
                
            //}


            remainingAmmunition--;
            if (remainingAmmunition > 0)
            {
                Recoil_Script.RecoilFire();
                nextShootTime = Time.time + (1 / fireRate);
                shootState = ShootState.Shooting;
            }
            else
            {
                Reload();
            }
        }

        Ray ray = new Ray(transform.position,transform.forward);
        if (Physics.Raycast(ray,out RaycastHit hit2))
        {
            if (hit2.collider.tag=="enemy")
            {
                playerHitEnemy = true;
                hit2.collider.gameObject.GetComponentInChildren<Slider>().value -= 0.02f;
                print("EnemyHITTTTTTTTTTTT");
            }
        }


    }

    void HandleHit(RaycastHit hit)
    {
        ////////////////
    }

    /// Attempts to reload the gun
    public void Reload()
    {
        // Checks that the gun is ready to be reloaded
        if (shootState == ShootState.Ready)
        {
            soundsManager.PlaySound("reload");
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
            animator.SetBool("Hide", true);
        }
    }
}