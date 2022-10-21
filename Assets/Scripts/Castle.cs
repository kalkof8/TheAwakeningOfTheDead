using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Building
{
    private UI _ui;

    public override void Start()
    {
        base.Start();
        _healthBar.Height = 10f;
        _ui = FindObjectOfType<UI>();
    }

    public override void Select()
    {
    }

    public override void UnSelect()
    {
    }

    public override void OpenloseWindow()
    {
        base.OpenloseWindow();
        _ui.OpenLoseWindow();
    }
}
