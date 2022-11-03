using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float Height = 2f;

    [SerializeField] private Transform _scaleTransform;
    [SerializeField] private Transform _target;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = _target.position + Vector3.up * Height;
        transform.rotation = _cameraTransform.rotation;
    }

    public void Setup(Transform target)
    {
        _target = target;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = (float)health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        _scaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
