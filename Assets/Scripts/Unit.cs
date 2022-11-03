using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public int Price = 2;
    public NavMeshAgent NavMeshAgent;

    [SerializeField] private int _health;
    [SerializeField] private GameObject _healthBarPrefab;

    private Sound _sound;
    private HealthBar _healthBar;
    private int _maxHealth;

    public override void Start()
    {
        base.Start();
        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
        _sound = FindObjectOfType<Sound>();
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        if(NavMeshAgent)
            NavMeshAgent.SetDestination(point);
    }

    public virtual void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _sound.StartAudioBattle();
        _healthBar.SetHealth(_health, _maxHealth);
        if(_health <= 0)
        {
            FindObjectOfType<Controller>().Unselect(this);
            FindObjectOfType<ObjectsFind>().AllUnits.Remove(this);
            _sound.StartAudioDie();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_healthBar)
            Destroy(_healthBar.gameObject);
    }
}
