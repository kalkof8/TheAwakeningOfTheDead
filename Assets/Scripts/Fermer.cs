using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FermerState
{
    Idle,
    WolkToWood,
    MineWood
}

public class Fermer : Unit
{
    [SerializeField] private FermerState _currentFermerState;
    [SerializeField] private int _periodMining = 1;
    [SerializeField] private Wood _targetWood;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _distanceToMining = 3f;

    private ObjectsFind _objectsFind;
    private float _timer;

    public override void Start()
    {
        base.Start();
        _objectsFind = FindObjectOfType<ObjectsFind>();
        SetStateFermer(FermerState.WolkToWood);
    }

    private void Update()
    {
        if (_currentFermerState == FermerState.Idle)
        {
            FindNearestWood();
        }
        if(_currentFermerState == FermerState.WolkToWood)
        {
            FindNearestWood();
            if (_targetWood)
            {
                WhenClickOnGround(_targetWood.transform.position);
                float distance = Vector3.Distance(transform.position, _targetWood.transform.position);
                if(distance < _distanceToMining)
                {
                    SetStateFermer(FermerState.MineWood);
                }
            }
        }
        if(_currentFermerState == FermerState.MineWood)
        {
            if (_targetWood)
            {
                _timer += Time.deltaTime;
                if (_timer >= _periodMining)
                {
                    _timer = 0f;
                    _targetWood.GiveWood();
                }
                float distance = Vector3.Distance(transform.position, _targetWood.transform.position);
                if (distance > _distanceToMining)
                {
                    SetStateFermer(FermerState.WolkToWood);
                }
            }
            else
            {
                FindNearestWood();
                SetStateFermer(FermerState.WolkToWood);
            }
        }
    }

    private void FindNearestWood()
    {
        float minDistance = Mathf.Infinity;
        Wood nearestWood = null;

        for (int i = 0; i < _objectsFind.AllWoods.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _objectsFind.AllWoods[i].transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                nearestWood = _objectsFind.AllWoods[i];
            }
        }
        _targetWood = nearestWood;
        if (_targetWood)
        {
            SetStateFermer(FermerState.WolkToWood);
        }
        else
        {
            SetStateFermer(FermerState.Idle);
        }
    }

    public void SetStateFermer(FermerState fermerState)
    {
        if (_currentFermerState == fermerState)
            return;

        _currentFermerState = fermerState;

        if (_currentFermerState == FermerState.Idle)
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWolk", false);
            _animator.SetBool("isMine", false);
        }
        if (_currentFermerState == FermerState.WolkToWood)
        {
            _animator.SetBool("isWolk", true);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isMine", false);

        }
        if (_currentFermerState == FermerState.MineWood)
        {
            _animator.SetBool("isMine", true);
            _animator.SetBool("isWolk", false);
            _animator.SetBool("isIdle", false);
        }
    }
}
