using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentInfo : MonoBehaviour
{
    [SerializeField] Multiplayer mp;
    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(GetInfo);
    }

    private async void GetInfo()
    {
        JSONNode info = await mp.GetInfo();
        Debug.Log(info);
        string description = "Health: " + info["health"].AsFloat.ToString("0.00") + "\nEnergy Income: " + info["energyIncome"].AsFloat.ToString("0.00") + "\nTowers Count: " + info["towersCount"].AsInt.ToString();
        HUDStats.hUDStats.SetupHUD("Opponent", GameAssets.i.towerSprites[0], description, 0);
    }
}
