using UnityEngine;

public class EnemyCreater : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnPeriod;
    [SerializeField] private Transform[] _spawns;
    
    private ObjectsFind _objectsFind;
    private float _timer;

    private void Start()
    {
        _objectsFind = FindObjectOfType<ObjectsFind>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _spawnPeriod)
        {
            _timer = 0f;
           GameObject newEnemyPrefab = Instantiate(
               _enemyPrefab, 
               _spawns[Random.Range(0, _spawns.Length)].position, 
               _spawns[0].rotation
               );
            _objectsFind.AllEnemy.Add(newEnemyPrefab.GetComponent<Enemy>());
        }
    }
}
