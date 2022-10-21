using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum KnightState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{
    [SerializeField] private KnightState _currentUnitState;
    [SerializeField] private Animator _animator;

    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private Enemy _targetEnemy;
    
    [SerializeField] private float _distanceToAttack = 1f;
    [SerializeField] private float _distanceToFollow = 7f;


    [SerializeField] private float _attackPeriod = 1f;

    
    private ObjectsFind _objectsFind;
    private float _timer;
   

    public override void Start()
    {
        base.Start();
        _objectsFind = FindObjectOfType<ObjectsFind>();
    }

    private void Update()
    {
        if (_currentUnitState == KnightState.Idle)
        {
            FindNearestEnemy();
            
        }
        else if (_currentUnitState == KnightState.WalkToPoint)
        {
            if (NavMeshAgent.hasPath && NavMeshAgent.remainingDistance < 0.3f)
            {
                SetStateKnight(KnightState.Idle);
            }
        }
        else if (_currentUnitState == KnightState.WalkToEnemy)
        {
            if (_targetEnemy)
            {
                NavMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(_targetEnemy.transform.position, transform.position);
                if (distance > _distanceToFollow)
                {
                    SetStateKnight(KnightState.Idle);
                }
                if (distance < _distanceToAttack)
                {
                    SetStateKnight(KnightState.Attack);
                    _timer = 0;
                }
            }
            else
            {
                SetStateKnight(KnightState.Idle);
                NavMeshAgent.SetDestination(transform.position);
            }
        }
        else if (_currentUnitState == KnightState.Attack)
        {
            if (_targetEnemy)
            {
                NavMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(_targetEnemy.transform.position, transform.position);
                if (distance > _distanceToAttack)
                {
                    SetStateKnight(KnightState.Idle);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    _targetEnemy.TakeDamage(1);
                }
            }
            else
            {
                SetStateKnight(KnightState.Idle);
                NavMeshAgent.SetDestination(transform.position);
            }
        }
    }

    private void FindNearestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        for (int i = 0; i < _objectsFind.AllEnemy.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _objectsFind.AllEnemy[i].transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                nearestEnemy = _objectsFind.AllEnemy[i];
            }
        }
        _targetEnemy = nearestEnemy;

        if (minDistance < _distanceToFollow)
        {
            _targetEnemy = nearestEnemy;
            SetStateKnight(KnightState.WalkToEnemy);
        } 
    }

    public void SetStateKnight(KnightState unitState)
    {
        if (_currentUnitState == unitState)
            return;

        _currentUnitState = unitState;
        if (_currentUnitState == KnightState.Idle)
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isAttack", false);
        }
        else if (_currentUnitState == KnightState.WalkToPoint)
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttack", false);
        }
        else if (_currentUnitState == KnightState.WalkToEnemy)
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttack", false);
        }
        else if (_currentUnitState == KnightState.Attack)
        {
            _animator.SetBool("isAttack", true);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isRunning", false);
        }
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
