using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectableIndicator;

    public virtual void Start()
    {
        SelectableIndicator.SetActive(false);
    }
    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }
    public virtual void OnUnhover()
    {
        transform.localScale = Vector3.one;
    }
    public virtual void Select()
    {
        SelectableIndicator.SetActive(true);
       
    }
    public virtual void UnSelect()
    {
        if (SelectableIndicator)
        {
            SelectableIndicator.SetActive(false);
        }
        
    }
    public virtual void WhenClickOnGround(Vector3 point)
    {

    }
}
