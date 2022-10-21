using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    [SerializeField] private int _periodMining = 8;
    [SerializeField] private int _amountOfMoney = 1;
    [SerializeField] private AudioSource _audioAddMoney;
    [SerializeField] private Transform _spawn;

    private UI _ui;
    private Resources _resources;
    private ObjectsFind _objectsFind;

    private float _timer;

    public override void Start()
    {
        base.Start();
        _ui = FindObjectOfType<UI>();
        _resources = FindObjectOfType<Resources>();
        _objectsFind = FindObjectOfType<ObjectsFind>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _periodMining)
        {
            _timer = 0f;
            _resources.Money += _amountOfMoney;
            _audioAddMoney.Play();
            _ui.UpdateText();
        }
    }

    public void BuyFermer(GameObject unitPrefab)
    {
        GameObject newUnit = Instantiate(unitPrefab, _spawn.position, _spawn.rotation);
        _objectsFind.AllUnits.Add(newUnit.GetComponent<Unit>());
        Vector3 position = _spawn.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }

}
