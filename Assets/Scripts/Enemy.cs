using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle, 
    WalkToBuilding,
    WalkToUnit, 
    AttackUnit,
    AttackBuilding
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyState _currentEnemyState;

    [SerializeField] private Building _targetBuilding;
    [SerializeField] private Unit _targetUnit;
    [SerializeField] private int _health;
    [SerializeField] private float _distanceToAttack = 1f;
    [SerializeField] private float _distanceToAttackBuilding = 3f;
    [SerializeField] private float _distanceToFollow = 7f;
    [SerializeField] private int _damageValue = 1;

    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _attackPeriod = 1f;
    [SerializeField] private GameObject _healthBarPrefab;

    private ObjectsFind _objectsFind;
    private HealthBar _healthBar;

    private Sound _sound;
    private int _maxHealth;
    private float _timer;

    private void Start()
    {
        _objectsFind = FindObjectOfType<ObjectsFind>();
        _sound = FindObjectOfType<Sound>();

        SetStateEnemy(EnemyState.WalkToBuilding);

        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    private void Update()
    {
        if (_currentEnemyState == EnemyState.Idle)
        {
            FindNearestBuilding();
            if (_targetBuilding)
            {
                SetStateEnemy(EnemyState.WalkToBuilding);
            }
            FindNearestUnit();
        } 
        else if (_currentEnemyState == EnemyState.WalkToBuilding)
        {
            FindNearestUnit();
            if (_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
                float distance = Vector3.Distance(_targetBuilding.transform.position, transform.position);
                if (distance < _distanceToAttackBuilding)
                {
                    SetStateEnemy(EnemyState.AttackBuilding);
                    _timer = 0;
                }
            }
            if (_targetBuilding == null)
            {
                SetStateEnemy(EnemyState.Idle);
            }
        }
        else if (_currentEnemyState == EnemyState.WalkToUnit)
        {
            if (_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(_targetUnit.transform.position, transform.position);
                if (distance > _distanceToFollow)
                {
                    SetStateEnemy(EnemyState.WalkToBuilding);
                }
                if (distance < _distanceToAttack)
                {
                    SetStateEnemy(EnemyState.AttackUnit);
                    _timer = 0;
                }
            }
            else
            {
                SetStateEnemy(EnemyState.WalkToBuilding);
            }
        }
        else if (_currentEnemyState == EnemyState.AttackBuilding)
        {
            if (_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.CenterBuilding.position);
                float distance = Vector3.Distance(_targetBuilding.CenterBuilding.position, transform.position);
                if (distance > _distanceToAttackBuilding)
                {
                    SetStateEnemy(EnemyState.WalkToBuilding);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    _targetBuilding.TakeDamage(_damageValue);
                }
            }
            else
            {
                SetStateEnemy(EnemyState.WalkToBuilding);
            }
        }
        else if (_currentEnemyState == EnemyState.AttackUnit)
        {
            if (_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(_targetUnit.transform.position, transform.position);
                if (distance > _distanceToAttack)
                {
                    SetStateEnemy(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    _targetUnit.TakeDamage(_damageValue);
                }
            }
            else
            {
                SetStateEnemy(EnemyState.WalkToBuilding);
            }
        }
    }

    public void FindNearestBuilding()
    {
        //Building[] allBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building nearestBuilding = null;

        for (int i = 0; i < _objectsFind.AllBuildings.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _objectsFind.AllBuildings[i].CenterBuilding.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                nearestBuilding = _objectsFind.AllBuildings[i];
            }
        }
        //for (int i = 0; i < allBuildings.Length; i++)
        //{
        //    float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
        //    if (distance <= minDistance)
        //    {
        //        minDistance = distance;
        //        nearestBuilding = allBuildings[i];
        //    }
        //}

        _targetBuilding = nearestBuilding;

    }

    public void FindNearestUnit()
    {
        //Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit nearestUnit = null;

        for (int i = 0; i < _objectsFind.AllUnits.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _objectsFind.AllUnits[i].transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                nearestUnit = _objectsFind.AllUnits[i];
            }
        }
        //for (int i = 0; i < allUnits.Length; i++)
        //{
        //    float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
        //    if (distance <= minDistance)
        //    {
        //        minDistance = distance;
        //        nearestUnit = allUnits[i];
        //    }
        //}
        _targetUnit = nearestUnit;

        if (minDistance < _distanceToFollow)
        {
            _targetUnit = nearestUnit;
            SetStateEnemy(EnemyState.WalkToUnit);
        }
    }

    public void SetStateEnemy(EnemyState enemyState)
    {
        if (_currentEnemyState == enemyState)
            return;

        _currentEnemyState = enemyState;
        if (_currentEnemyState == EnemyState.Idle)
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isAttack", false);
        }
        else if (_currentEnemyState == EnemyState.WalkToBuilding)
        {
            FindNearestBuilding();
            if (_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.CenterBuilding.position);
                _animator.SetBool("isRunning", true);
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isAttack", false);
            }
            else
            {
                SetStateEnemy(EnemyState.Idle);
            }
        }
        else if (_currentEnemyState == EnemyState.WalkToUnit)
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttack", false);
        }
        else if (_currentEnemyState == EnemyState.AttackUnit)
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttack", true);
        }
        else if (_currentEnemyState == EnemyState.AttackBuilding)
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttack", true);
        }
    }
    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _sound.StartAudioBattle();
        _healthBar.SetHealth(_health, _maxHealth);
        if (_health <= 0)
        {
            _sound.StartAudioDie();
            _objectsFind.AllEnemy.Remove(this);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_healthBar)
            Destroy(_healthBar.gameObject);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}
