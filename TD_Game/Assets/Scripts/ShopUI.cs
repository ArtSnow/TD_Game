using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour
{
    private int shopIndex = 0;
    private int index = 0;
    [SerializeField] private TowerDefenseAI towerDefenseAI;
    private Button[] buttons = new Button[5];
    private SpriteRenderer[] buttonsSprite = new SpriteRenderer[5];
    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            int sIndex = i + index;
            Transform button = transform.Find("B" + i).Find("Button");
            buttons[i] = button.GetComponent<Button>();
            buttonsSprite[i] = button.Find("Sprite").GetComponent<SpriteRenderer>();
        } 
        SetupShop();
        
    }

    private void Update()
    {
        if (shopIndex == 0) {
            for (int i = 0; i < 5; i++)
            {
                if (GameResources.i.getTowerPrice(i) > 0 & GameResources.i.getTowerPrice(i) <= GameResources.i.getEnergy())
                {
                    buttons[i].interactable = true;
                } else
                {
                    buttons[i].interactable = false;
                }
            }
        }

    }
    private void SetupShop()
    {
        if (shopIndex == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                int sIndex = i + index;
                buttonsSprite[i].sprite = GameAssets.i.towerSprites[sIndex];
                string outTitle;
                Sprite outSprite;
                float outRange;
                float outDamageAmount;
                float outShootTimerMax;
                int outPrice;
                float outRangeUC;
                float outDamageUC;
                float outShootTimerUC;
                GameResources.i.getTower(i, out outTitle, out outSprite, out outRange, out outDamageAmount, out outShootTimerMax, out outPrice, out outDamageUC, out outRangeUC, out outShootTimerUC);
                string description = "Range: " + outRange.ToString("0.00") + "\nDamage: " + outDamageAmount.ToString("0.00") + "\nAttackTime: " + outShootTimerMax.ToString("0.00") + "\nPrice: " + outPrice.ToString();
                buttons[i].transform.GetComponent<UIPointer>().SetupInfo(outTitle, outSprite, description, 0);
                buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                buttons[i].GetComponent<Button>().onClick.AddListener(() => BuyTower(sIndex));
            }
        } else if (shopIndex == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                int sIndex = i + index;
                buttonsSprite[i].sprite = GameAssets.i.monsterSprites[sIndex];
                buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                buttons[i].GetComponent<Button>().onClick.AddListener(() => BuyTower(sIndex));
            }
        }

    }
    private void BuyTower(int towerIndex)
    {
        towerDefenseAI.HighlightPlaceForBuilding(towerIndex);
    }

}
