using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    [SerializeField] private Building _currentBuilding;
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GameObject _menuImage;

    private Dictionary<Vector2Int, Building> _buildingsDictionary = new Dictionary<Vector2Int, Building>();
    private Plane _plane;
    private ObjectsFind _objectsFind;

    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _objectsFind = FindObjectOfType<ObjectsFind>();
    }

    private void Update()
    {
        if(_currentBuilding == null)
        {
            return;
        }

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance)/CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        _currentBuilding.transform.position = new Vector3(x, 0f, z) * CellSize;

        if (CheekAllow(x, z, _currentBuilding))
        {
            _currentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0))
            {
                InstallBuilding(x, z, _currentBuilding);
                _currentBuilding = null;
            }
        }
        else
        {
            _currentBuilding.DisplayUnacceptablePosition();
        }
    }
    
    private bool CheekAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (_buildingsDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                _buildingsDictionary.Add(coordinate, building);
            }
        }
        foreach (var item in _buildingsDictionary)
        {
            Debug.Log(item);
        }
    }

    //public void RemoveBuilding(Building building)
    //{
    //    for (int i = 0; i < _buildingsDictionary.Count; i++)
    //    {
    //        _buildingsDictionary.
    //    }
    //}

    public void CreatBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        Building building = newBuilding.GetComponent<Building>();
        _currentBuilding = building;
        _objectsFind.AllBuildings.Add(building);
    }
}
