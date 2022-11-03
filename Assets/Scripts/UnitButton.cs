using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private Text _priceText;
    [SerializeField] private Barack _barack;
    [SerializeField] private Mine _mine;
    [SerializeField] private AudioSource _audioClick;

    private UI _ui;

    private void Start()
    {
        _ui = FindObjectOfType<UI>();
        _priceText.text = _unitPrefab.GetComponent<Unit>().Price.ToString();
    }

    public void TryBuyKnight()
    {
        int price = _unitPrefab.GetComponent<Unit>().Price;

        Resources resources = FindObjectOfType<Resources>();
        if (resources.Money >= price)
        {
            _audioClick.Play();
            resources.Money -= price;
            _ui.UpdateText();
            _barack.BuyKnigth(_unitPrefab);
        }
        else
        {
            _ui.StartAnimationMone();
            Debug.Log("noMone");
        }
    }

    public void TryBuyFermer()
    {
        int price = _unitPrefab.GetComponent<Unit>().Price;

        Resources resources = FindObjectOfType<Resources>();
        if (resources.Money >= price)
        {
            _audioClick.Play();
            resources.Money -= price;
            _ui.UpdateText();
            _mine.BuyFermer(_unitPrefab);
        }
        else
        {
            _ui.StartAnimationMone();
            Debug.Log("noMone");
        }
    }
}
