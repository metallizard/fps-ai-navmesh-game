using UnityEngine;
using UnityEngine.AI; // Import library from UnityEngine.AI

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{   
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyAnimator _enemyAnimator;
    private NavMeshAgent _navMeshAgent;

    private EnemyState _state;

    [SerializeField]
    private float _patrolSpeed = 0.5f;
    [SerializeField]
    private float _chaseSpeed = 4;
    [SerializeField]
    private float _attackDistance = 1.9f;
    [SerializeField]
    private float _chaseAfterAttackDistance = 2;
    
    [SerializeField]
    private float _patrolRadiusMin = 20;
    [SerializeField]
    private float _patrolRadiusMax = 60;
    [SerializeField]
    private float _patrolTime = 15;
    private float _patrolTimer;

    [SerializeField]
    private float _waitBeforeAttack = 1.5f;
    private float _attackTimer;

    [SerializeField]
    private float _aggroDistance = 20f;

    private float _currentAggroDistance;

    private Transform _target;

    private Health _health;

    void Awake()
    {
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _target = GameObject.FindWithTag("Player").transform;

        _health = GetComponent<Health>();

        _health.OnHealthReduced += OnGetDamaged;
    }

    void Start()
    {
        // AI will go to patrol mode.
        _state = EnemyState.Patrol;

        // Set patroltimer to patrol time.
        _patrolTimer = _patrolTime;

        // Set attack timer to wait before attack
        _attackTimer = _waitBeforeAttack;

        // Set Aggro distance to aggro distance.
        _currentAggroDistance = _aggroDistance;
    }

    void Update()
    {
        if (_state == EnemyState.Patrol)
            Patrol();
        if (_state == EnemyState.Chase)
            Chase();
        if (_state == EnemyState.Attack)
            Attack();
    }

    private void OnGetDamaged(float currentHealth)
    {
        if (currentHealth > 0)
        {
            if (_state == EnemyState.Patrol)
            {
                _aggroDistance = 50;
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void Patrol()
    {
        // Set nav mesh agent to start.
        _navMeshAgent.isStopped = false;

        // Set nav mesh speed.
        _navMeshAgent.speed = _patrolSpeed;

        // Update patrol timer.
        _patrolTimer += Time.deltaTime;

        // If we already patrol long enough we move to another patrol area.
        if(_patrolTimer > _patrolTime)
        {
            SetNewRandomPatrolArea();

            _patrolTimer = 0;
        }

        // Play walk animation if our AI is moving
        if (IsMoving())
        {
            _enemyAnimator.Walk(true);
        }
        else
        {
            _enemyAnimator.Walk(false);
        }

        // Check for player
        if (Vector3.Distance(transform.position, _target.position) <= _aggroDistance)
        {
            _state = EnemyState.Chase;
            _enemyAnimator.Walk(false);
        }
    }

    private void Chase()
    {
        _navMeshAgent.isStopped = false;

        _navMeshAgent.speed = _chaseSpeed;

        _navMeshAgent.SetDestination(_target.position);

        if (IsMoving())
        {
            _enemyAnimator.Run(true);
        }
        else
        {
            _enemyAnimator.Run(false);
        }

        if (Vector3.Distance(transform.position, _target.position) <= _attackDistance)
        {
            _enemyAnimator.Walk(false);
            _enemyAnimator.Run(false);

            _state = EnemyState.Attack;

            if (_aggroDistance != _currentAggroDistance)
                _aggroDistance = _currentAggroDistance;
        }
        else if(Vector3.Distance(transform.position, _target.position) > _aggroDistance)
        {
            // Run away.
            _enemyAnimator.Run(false);

            _state = EnemyState.Patrol;

            // reset patrol timer.
            _patrolTimer = _patrolTime;

            if(_aggroDistance != _currentAggroDistance)
            {
                _aggroDistance = _currentAggroDistance;
            }
        }
    }

    private void Attack()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;

        _attackTimer += Time.deltaTime;

        if(_attackTimer > _waitBeforeAttack)
        {
            _enemyAnimator.Attack();

            _attackTimer = 0;
        }

        if(Vector3.Distance(transform.position, _target.position) > _attackDistance + _chaseAfterAttackDistance)
        {
            _state = EnemyState.Chase;
        }
    }

    private void SetNewRandomPatrolArea()
    {
        // Get new random patrol position.
        float newRadius = Random.Range(_patrolRadiusMin, _patrolRadiusMax);
        Vector3 direction = Random.insideUnitSphere * newRadius;
        direction += transform.position;

        // Make sure new random patrol area is valid.
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(direction, out navMeshHit, newRadius, -1);

        // Set new random destination.
        _navMeshAgent.SetDestination(navMeshHit.position);
    }

    private bool IsMoving()
    {
        return _navMeshAgent.velocity.sqrMagnitude > 0;
    }
}
