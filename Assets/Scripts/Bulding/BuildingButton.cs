using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingPlacer _buildingPlacer;
    [SerializeField] private GameObject _buildPrefab;
    [SerializeField] private AudioSource _audioClick;
    [SerializeField] private UI _ui;
    [SerializeField] private Animator _animatorMone;
    [SerializeField] private Animator _animatorWood;
   
    public void TryBuy()
    {
        int price = _buildPrefab.GetComponent<Building>().AmountOfMoney;
        int droow = _buildPrefab.GetComponent<Building>().AmountOfWood;

        Resources resources = FindObjectOfType<Resources>();

        if(resources.Money >= price && resources.Wood >= droow)
        {
            _audioClick.Play();
            resources.Money -= price;
            resources.Wood -= droow;
            _ui.UpdateText();
            _buildingPlacer.CreatBuilding(_buildPrefab);
        }
        else if(resources.Money < price && resources.Wood < droow)
        {
            _animatorMone.SetTrigger("noMone");
            _animatorWood.SetTrigger("noWood");
        }
        else if(resources.Money < price)
        {
            _animatorMone.SetTrigger("noMone");
        }
        else if(resources.Wood < droow)
        {
            _animatorWood.SetTrigger("noWood");
        }
    }

}
