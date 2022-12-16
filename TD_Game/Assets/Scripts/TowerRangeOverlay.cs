using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TowerRangeOverlay : MonoBehaviour
{
    private static TowerRangeOverlay Instance { get; set; }
    private Tower tower;
    private void Awake()
    {
        Instance = this;

        Hide();
    }
    
    public static void Show_Static(Tower tower)
    {
        Instance.Show(tower);
    }

    private void Show(Tower tower)
    {
        this.tower = tower;
        gameObject.SetActive(true);
        transform.position = tower.transform.position;
        RefreshRangeVisual();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RefreshRangeVisual()
    {
        transform.localScale = Vector3.one * tower.GetRange() / 3f;
    }

    private void OnMouseExit()
    {
        Hide();
    }
}
