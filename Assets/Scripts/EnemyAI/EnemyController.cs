using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private PlayerStats playerStats;
    private EnemyStats enemyStats;

    private Vector2 targetPosition;
    public NavMeshAgent agent { get; private set; }

    public enum AIState
    {
        idle,
        wondering,
        foundPlayer,
        attack,
        stanned,
        dead
    }

    public AIState state { get; private set; } = AIState.idle;
    private float stateCountdown
    {
        get
        {
            return _stateCountdown;
        }
        set
        {
            _stateCountdown = value;

            if (_stateCountdown < 0)
            {
                _stateCountdown = 0;
            }
        }
    }
    private float _stateCountdown;
    private float attackCountdown
    {
        get
        {
            return _attackCountdown;
        }
        set
        {
            _attackCountdown = value;

            if (_attackCountdown < 0)
            {
                _attackCountdown = 0;
            }
        }
    }
    private float _attackCountdown;
    ContactFilter2D contactFilter2D = new ContactFilter2D();
    [SerializeField] private LayerMask playerLayerMask;
    List<RaycastHit2D> searchingResults = new List<RaycastHit2D>();

    void Awake()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = enemyStats.enemySpeed;

        contactFilter2D.layerMask = playerLayerMask;

        stateCountdown = Random.Range(2f, 6f);
    }

    private void Update()
    {
        controlState();
    }

    private void controlState()
    {
        if (state == AIState.idle)
        {
            waitingForPlayer();

            stateCountdown -= Time.deltaTime;

            if (stateCountdown <= 0f)
            {
                targetPosition = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
                state = AIState.wondering;
                agent.SetDestination((Vector2)transform.position + targetPosition);
            }
        }
        if (state == AIState.wondering)
        {
            waitingForPlayer();

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        agent.ResetPath();

                        stateCountdown = Random.Range(2f, 6f);
                        state = AIState.idle;
                    }
                }
            }

        }
        if (state == AIState.foundPlayer)
        {
            targetPosition = playerStats.transform.position;
            agent.SetDestination(targetPosition);

            if (Vector2.Distance(playerStats.transform.position, transform.position) <= enemyStats.attackDistance)
            {
                attackCountdown = enemyStats.attackDelay;
                agent.ResetPath();

                state = AIState.attack;
            }
        }
        if (state == AIState.attack)
        {
            attackCountdown -= Time.deltaTime;

            if (Vector2.Distance(playerStats.transform.position, transform.position) <= enemyStats.attackDistance)
            {
                AttackPlayer(enemyStats.enemyDamage);
            }
            else if (Vector2.Distance(playerStats.transform.position, transform.position) > enemyStats.attackDistance)
            {
                state = AIState.foundPlayer;
            }
        }
        if (state == AIState.stanned)
        {
            agent.ResetPath();
        }
        if (state == AIState.dead)
        {
            agent.ResetPath();
            enemyStats.deathCountDown -= Time.deltaTime;

            float scaleFactor = Mathf.Lerp(0f, 1f, enemyStats.deathCountDown);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            if (enemyStats.deathCountDown <= 0)
            {
                spawnDropObjects();
                Destroy(gameObject);
            }
        }
    }
    private void waitingForPlayer()
    {
        Physics2D.CircleCast(transform.position, 4.14f, transform.position, contactFilter2D, searchingResults);
        foreach (RaycastHit2D hit in searchingResults)
        {
            if (hit.collider.CompareTag("Player"))
            {
                state = AIState.foundPlayer;
            }
        }
    }

    private void AttackPlayer(int damage)
    {
        if (attackCountdown == 0)
        {
            playerStats.TakeDamage(damage);
            state = AIState.foundPlayer;
        }
    }

    public void SetStateFoundPlayer()
    {
        state = AIState.foundPlayer;
    }

    public void SetStateDead()
    {
        state = AIState.dead;
    }

    public void SetStateStanned(bool isStanned)
    {
        if (isStanned)
        {
            state = AIState.stanned;
        }
        else
        {
            state = AIState.idle;
        }
    }

    private void spawnDropObjects() { //FIXME Rework to have random to objectsToDrop too
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= 0.3f)
        {
            foreach (GameObject dropItem in enemyStats.objectsToDrop)
            {
                Instantiate(dropItem, transform.position * randomValue, Quaternion.identity);
            }
        }
    }
}
