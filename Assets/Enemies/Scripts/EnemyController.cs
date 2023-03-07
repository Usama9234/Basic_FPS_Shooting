using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public Transform searchPlayer;
    Animator animator;
    public Transform enemyPositions;
    int count = 0;
    NavMeshAgent agent;
    public LayerMask layersToHit;
    bool playerSeen=false;
    bool check;
    public ParticleSystem muzzle;
    public float enemyRange;
    bool enemyDead = false;


    private void Start()
    {
        muzzle.Stop();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float angle = Mathf.Sin(Time.time * 10f) * 110;
        Vector3 direction = Quaternion.Euler(0, angle, 0) * searchPlayer.transform.forward;
        Ray ray = new Ray(searchPlayer.position, direction);
        Debug.DrawRay(searchPlayer.position, direction * 100, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "GameController")
            {
                playerSeen = true;
            }

        }

        if (!playerSeen)
        {
            MoveAtDefaultPositions();
        }
        else
        {
            ChasePlayer();
        }

        if (!enemyDead)
        {
            EnemyDead();
        }
        if (enemyDead)
        {
            agent.destination = agent.transform.position;
        }

        if (gameObject.GetComponentInChildren<Slider>().value<1&&!enemyDead)
        {
            ChasePlayer();
        }


        

    }


    void MoveAtDefaultPositions()
    {
        muzzle.Stop();
        animator.SetBool("isRunning", false);
        animator.SetBool("isFiring", false);
        animator.SetBool("isWalking", true);
        agent.speed = 1.5f;
        agent.destination = enemyPositions.GetChild(count).transform.position; 
        if (Vector3.Distance(transform.position, enemyPositions.GetChild(count).transform.position) <= 0.7f)
        {
            count++;
            if (count >= enemyPositions.childCount)
            {
                count = 0;
            }
        }
    }
    void ChasePlayer()
    {
        if (Vector3.Distance(transform.position,player.transform.position)<=9f)
        {
            Attack();
        }
        else if(Vector3.Distance(transform.position, player.transform.position) <= enemyRange)
        {
            agent.speed = 4f;
            agent.destination = player.transform.position;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            animator.SetBool("isFiring", false);
            muzzle.Stop();
        }
        else
        {
            playerSeen = false;
            MoveAtDefaultPositions();
        }
    }

    void Attack()
    {
        Vector3 direct = player.transform.position - transform.position;
        Ray ray = new Ray(searchPlayer.position, direct);
        Debug.DrawRay(searchPlayer.position, direct * 100, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "GameController")
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isFiring", true);
                muzzle.Play();
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
                agent.speed = 0f;
                agent.destination = player.transform.position;
            }

        }

        

    }

    void EnemyDead()
    {
        if (gameObject.GetComponentInChildren<Slider>().value == 0)
        {
            agent.destination = transform.position;
            enemyDead = true;
            muzzle.Stop();
            animator.SetBool("isDead", true);
            agent.speed = 0;
            Destroy(gameObject, 4f);
            

        }
    }
    

   

}
