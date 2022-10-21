using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Controller : MonoBehaviour
{
    [SerializeField] private List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableObject _howered;
    [SerializeField] private Image _frameImage;
    [SerializeField] private SelectionState _currentSelectionState;

    private Vector2 _frameStart;
    private Vector2 _frameEnd;
    private ObjectsFind _objectsFind;

    private void Start()
    {
        //Application.targetFrameRate = 60;
        _frameImage.enabled = false;
        _objectsFind = FindObjectOfType<ObjectsFind>();
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>())
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
      
                if (_howered)
                {
                    if(_howered != hitSelectable)
                    {
                        _howered.OnUnhover();
                        _howered = hitSelectable;
                        _howered.OnHover();
                    }
                }
                else
                {
                    _howered = hitSelectable;
                    _howered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }
        if (Input.GetMouseButtonUp(0) && _frameImage.enabled == false)
        {
            if (_howered)
            {
                Building build = hit.collider.GetComponent<SelectableCollider>().SelectableObject.GetComponent<Building>();
                if (Input.GetKey(KeyCode.LeftControl) == false || build)
                    UnselectAll();
                Select(_howered);
                _currentSelectionState = SelectionState.UnitsSelected;
            }
        }
        if(_currentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (hit.collider.tag == "Ground")
                        {
                            int rowNamber = Mathf.CeilToInt(Mathf.Sqrt(_listOfSelected.Count));

                            for (int i = 0; i < _listOfSelected.Count; i++)
                            {
                                int row = i / rowNamber;
                                int column = i % rowNamber;
                                Vector3 position = hit.point + new Vector3(row, 0f, column);
                                if (_listOfSelected[i].GetComponent<Knight>())
                                    _listOfSelected[i].GetComponent<Knight>().SetStateKnight(KnightState.WalkToPoint);
                                _listOfSelected[i].WhenClickOnGround(position);
                            }
                        }
                    }
                }
            }
        }
      
        if (Input.GetMouseButtonDown(1))
        {
            UnselectAll();
        }
        DrawFrame();
    }

    private void DrawFrame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = max - min;

            if(size.magnitude > 10)
            {
                _frameImage.enabled = true;
                _frameImage.rectTransform.anchoredPosition = min;
                _frameImage.rectTransform.sizeDelta = size;

                UnselectAll();
                Rect rect = new Rect(min, size);
       
                for (int i = 0; i < _objectsFind.AllUnits.Count; i++)
                {
                    Vector2 screnPosition = _camera.WorldToScreenPoint(_objectsFind.AllUnits[i].transform.position);
                    if (rect.Contains(screnPosition))
                    {
                        Select(_objectsFind.AllUnits[i]);
                    }
                }
                _currentSelectionState = SelectionState.Frame;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;

            if(_listOfSelected.Count > 0)
                _currentSelectionState = SelectionState.UnitsSelected;
            else
                _currentSelectionState = SelectionState.Other;
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (_listOfSelected.Contains(selectableObject) == false)
        {
            _listOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
        _currentSelectionState = SelectionState.UnitsSelected;
    }
    private void UnselectAll()
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            _listOfSelected[i].UnSelect();
        }
        _listOfSelected.Clear();
        _currentSelectionState = SelectionState.Other;
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if (_listOfSelected.Contains(selectableObject))
        {
            _listOfSelected.Remove(selectableObject);
        }
    }

    private void UnhoverCurrent()
    {
        if (_howered)
        {
            _howered.OnUnhover();
            _howered = null;
        }
    }
}
