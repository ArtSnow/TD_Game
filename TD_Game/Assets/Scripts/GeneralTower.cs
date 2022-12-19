using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTower : MonoBehaviour
{
    private void OnMouseEnter()
    {
        string description = "EnergyIncome: " + GameResources.i.getEnergyIncome().ToString("0.00");
        HUDStats.hUDStats.SetupHUD("Your Tower", GameAssets.i.generalTowerSprite, description, 0);
    }
}
