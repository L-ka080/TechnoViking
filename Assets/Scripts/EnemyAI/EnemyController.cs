using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControlller : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private Vector2 targetPosition;
    NavMeshAgent agent;

    private enum AIState
    {
        idle,
        wondering,
        foundPlayer,
        attack
    }

    private AIState state = AIState.idle;
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

    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDistance;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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

            if (Vector2.Distance(transform.position, targetPosition) <= 1f)
            {
                attackCountdown = attackDelay;
                agent.ResetPath();

                state = AIState.attack;
            }
        }
        if (state == AIState.attack)
        {
            attackCountdown -= Time.deltaTime;

            if (attackCountdown == 0 && Vector2.Distance(playerStats.transform.position, transform.position) <= attackDistance)
            {
                AttackPlayer(1);

                state = AIState.foundPlayer;
            }
            else if (Vector2.Distance(playerStats.transform.position, transform.position) > attackDistance)
            {
                state = AIState.foundPlayer;
            }
        }
    }

    private void AttackPlayer(int damage)
    {
        playerStats.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            state = AIState.foundPlayer;
        }
    }
}
