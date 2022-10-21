using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsFind : MonoBehaviour
{
    public List<Building> AllBuildings = new List<Building>();
    public List<Unit> AllUnits = new List<Unit>();
    public List<Enemy> AllEnemy = new List<Enemy>();
    public List<Wood> AllWoods = new List<Wood>();

    private void Start()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        Building[] allBuildgs = FindObjectsOfType<Building>();
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        Wood[] allWoods = FindObjectsOfType<Wood>();

        for (int i = 0; i < allUnits.Length; i++)
        {
            AllUnits.Add(allUnits[i]);
        }
        for (int i = 0; i < allBuildgs.Length; i++)
        {
            AllBuildings.Add(allBuildgs[i]);
        }
        for (int i = 0; i < allEnemies.Length; i++)
        {
            AllEnemy.Add(allEnemies[i]);
        }
        for (int i = 0; i < allWoods.Length; i++)
        {
            AllWoods.Add(allWoods[i]);
        }
    }

}
