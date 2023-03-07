using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Gun gun;
    Transform gunObj;
    //Rotation
    Vector3 currentRotation;
    Vector3 targetRotation;

    // Position kickback
    Vector3 targetPosition;
    Vector3 currentPosition;

    ////Hipfire Recoil
    //[SerializeField] float recoilX;
    //[SerializeField] float recoilY;
    //[SerializeField] float recoilZ;

    ////Settings
    //[SerializeField] float snappiness;
    //[SerializeField] float returnSpeed;

    void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        gun = GetComponentInChildren<Gun>();

        if(gun != null)
        {
            gunObj = gun.gameObject.transform;
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun.returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, gun.snappiness * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(currentRotation);
            KickBack();
        }
        
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(gun.recoilX, Random.Range(-gun.recoilY, gun.recoilY), Random.Range(-gun.recoilZ, gun.recoilZ));
        targetPosition += new Vector3(0f, 0f, -gun.kickBack);
        
    }

    void KickBack()
    {
        targetPosition = Vector3.Lerp(targetPosition, gun.initialPosition, gun.returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, gun.snappiness * Time.fixedDeltaTime);
        gunObj.localPosition = currentPosition;
    }
}
