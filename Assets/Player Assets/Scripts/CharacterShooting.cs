using UnityEngine;
using System.Collections;


public class CharacterShooting : MonoBehaviour
{
    Gun gun;
    Animator animator;
    public int shootButton;
    public KeyCode reloadKey;
    bool isplayed;

    void Awake()
    {
        
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        gun = GetComponentInChildren<Gun>();
        if (gun != null)
        {
            if (Input.GetMouseButton(shootButton))
            {
                gun.Shoot();
            }

            if (Input.GetKeyDown(reloadKey))
            {
                animator.SetBool("Hide", true);
                gun.Reload();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(shootButton))
            {
                StartCoroutine(PlayHitAnimation());
            }

            IEnumerator PlayHitAnimation()
            {
                animator.SetBool("Hit", true);
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("Hit", false);
            }


        }


    }    
}