using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTower : MonoBehaviour
{
    public static Animator animator { get; private set; }
    public static AudioSource audioS { get; private set; }
    private void Awake()
    {
        animator = transform.Find("Sprite").GetComponent<Animator>();
        audioS = transform.GetComponent<AudioSource>();
    }
    private void OnMouseEnter()
    {
        string description = "EnergyIncome: " + GameResources.i.getEnergyIncome().ToString("0.00");
        HUDStats.hUDStats.SetupHUD("Your Tower", GameAssets.i.generalTowerSprite, description, 0);
    }
}
