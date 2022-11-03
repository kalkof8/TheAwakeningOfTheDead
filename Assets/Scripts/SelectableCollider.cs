using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    public SelectableObject SelectableObject;

    [SerializeField] private Knight _knight;

    private void OnTriggerEnter(Collider other)
    {
        _knight.SetStateKnight(KnightState.Attack);
    }
}
