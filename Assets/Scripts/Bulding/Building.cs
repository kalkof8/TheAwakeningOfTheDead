using UnityEngine;

public class Building : SelectableObject
{
    public Transform CenterBuilding;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _menuImage;
    [SerializeField] private int _health;
    [SerializeField] private GameObject _healthBarPrefab;
    
    protected HealthBar _healthBar;
    protected Sound _sound;

    private int _maxHealth;

    public int AmountOfMoney;
    public int AmountOfWood;
    public int XSize = 3;
    public int ZSize = 3;

    private Color _startColor;

    private void Awake()
    {
        _startColor = _renderer.material.color;
    }

    public override void Start()
    {
        base.Start();
        _sound = FindObjectOfType<Sound>();
        _menuImage.SetActive(false);
        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(CenterBuilding);
    }

    public override void Select()
    {
        base.Select();
        _menuImage.SetActive(true);
       
    }

    public override void UnSelect()
    {
        base.UnSelect();
        _menuImage.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0f, z) * cellSize, new Vector3(1f, 0f, 1f)*cellSize);
            }
        }
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _sound.StartAudioAttackBuilding();
        _healthBar.SetHealth(_health, _maxHealth);
        if (_health <= 0)
        {
            FindObjectOfType<Controller>().Unselect(this);
            FindObjectOfType<ObjectsFind>().AllBuildings.Remove(this);
            Destroy(gameObject);
            OpenloseWindow();
        }
    }

    public virtual void OpenloseWindow()
    {

    }

    public void DisplayUnacceptablePosition()
    {
        _renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        _renderer.material.color = _startColor;
    }
    protected void OnDestroy()
    {
        if (_healthBar)
            Destroy(_healthBar.gameObject);
    }

}
