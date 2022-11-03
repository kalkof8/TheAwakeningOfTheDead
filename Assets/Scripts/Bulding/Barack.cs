using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barack : Building
{
    [SerializeField] private Knight _prefabKnight;
    [SerializeField] private Transform _spawn;

    private ObjectsFind _objectsFind;

    public override void Start()
    {
        base.Start();
        _objectsFind = FindObjectOfType<ObjectsFind>();
        _healthBar.Height = 3f;
    }

    public void BuyKnigth(GameObject unitPrefab)
    {
        GameObject newUnit = Instantiate(unitPrefab, _spawn.position, _spawn.rotation);
        _objectsFind.AllUnits.Add(newUnit.GetComponent<Unit>());
        Vector3 position = _spawn.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }

}
