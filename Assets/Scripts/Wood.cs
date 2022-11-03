using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private int _numberWoods = 20;

    private Resources _resources;
    private UI _ui;
    private ObjectsFind _objectsFind;

    private void Start()
    {
        _objectsFind = FindObjectOfType<ObjectsFind>();
        _resources = FindObjectOfType<Resources>();
        _ui = FindObjectOfType<UI>();
    }

    public void GiveWood()
    {
        if(_numberWoods > 0)
        {
            _numberWoods -= 1;
            _resources.Wood += 1;
            _ui.UpdateText();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _objectsFind.AllWoods.Remove(this);
    }
}
