using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _droowText;
    [SerializeField] private Resources _resources;
    [SerializeField] private Animator _animatorMone;

    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private GameObject _menuBatton;
    [SerializeField] private GameObject _menuLose;
    [SerializeField] private GameObject _resourseGroup;
    [SerializeField] private GameObject _buildingImage;
    [SerializeField] private MonoBehaviour[] _objectsToDeactivate;

    private float _startFixedUpdate;

    private void Start()
    {
        UpdateText();
        _startFixedUpdate = Time.fixedDeltaTime;
    }

    public void UpdateText()
    {
        _moneyText.text = _resources.Money.ToString();
        _droowText.text = _resources.Wood.ToString();
    }

    public void StartAnimationMone()
    {
        _animatorMone.SetTrigger("noMone");
    }

    public void OpenMenuWindow()
    {
        _menuWindow.SetActive(true);
        _menuBatton.SetActive(false);
        for (int i = 0; i < _objectsToDeactivate.Length; i++)
        {
            _objectsToDeactivate[i].enabled = false;
        }
        Time.timeScale = 0.0f;
    }
    public void CloseMenuWindow()
    {
        _menuWindow.SetActive(false);
        _menuBatton.SetActive(true);
        for (int i = 0; i < _objectsToDeactivate.Length; i++)
        {
            _objectsToDeactivate[i].enabled = true;
        }
        Time.timeScale = 1f;
    }
    public void OpenLoseWindow()
    {
        for (int i = 0; i < _objectsToDeactivate.Length; i++)
        {
            _objectsToDeactivate[i].enabled = false;
        }

        _menuBatton.SetActive(false);
        _menuWindow.SetActive(false);
        _resourseGroup.SetActive(false);
        _buildingImage.SetActive(false);
        _menuLose.SetActive(true);
        Invoke(nameof(Restart), 5f);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _menuLose.SetActive(false);
    }

    private void OnDestroy()
    {
        Time.fixedDeltaTime = _startFixedUpdate;
        Time.timeScale = 1f;
    }
}
