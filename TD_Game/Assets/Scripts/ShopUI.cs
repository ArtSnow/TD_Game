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
    [SerializeField] private Sprite[] shopTabsSprites;
    [SerializeField] private TowerDefenseAI towerDefenseAI;
    [SerializeField] private Button[] shops;
    private Button[] buttons = new Button[5];
    private SpriteRenderer[] buttonsSprite = new SpriteRenderer[5];
    private void Awake()
    {
        for (int i = 0; i < shops.Length; i++)
        {
            int shIndex = i;
            shops[i].onClick.AddListener(() => 
            {
                shopIndex = shIndex;
                SetupShop();
            });
            Debug.Log(i);
        }
        for (int i = 0; i < 5; i++)
        {
            Transform button = transform.Find("B" + i).Find("Button");
            buttons[i] = button.GetComponent<Button>();
            buttonsSprite[i] = button.Find("Sprite").GetComponent<SpriteRenderer>();
        } 
        SetupShop();
    }

    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            switch (shopIndex) {
                default:
                    break;
                case 0:
                    buttons[i].interactable = GameResources.i.getTowerPrice(i) > 0 & GameResources.i.getTowerPrice(i) <= GameResources.i.getEnergy();
                    break;
                case 1:
                    buttons[i].interactable = GameResources.i.getEnemyPrice(i) > 0 & GameResources.i.getEnemyPrice(i) <= GameResources.i.getCoins();
                    break;
            }
                
        }
    }

    private void SetupShop()
    {
        for (int i = 0; i < shops.Length; i++)
        {
            shops[i].transform.GetComponent<Image>().sprite = shopTabsSprites[i != shopIndex ? 0 : 1];
        }
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

                string outTitle;
                Sprite outSprite;
                int outDamage;
                int outMaxHealth;
                int outPrice;
                int outEnergyReward;
                int outCoinsReward;
                int outArmor;
                float outSpeed;
                int outEnergyIncome;

                GameResources.i.getEnemy(i, out outTitle, out outSprite, out outDamage, out outMaxHealth, out outPrice, out outEnergyReward, out outCoinsReward, out outArmor, out outSpeed, out outEnergyIncome);

                string description = "Damage: " + outDamage.ToString("0.00") + "\nMaxHealth: " + outMaxHealth.ToString("0.00") + "\nArmor: " + outArmor.ToString("0.00") + "\nSpeed: " + outSpeed.ToString("0.00") + "\nEnergy Income: " + outEnergyIncome.ToString("0.00") + "\nPrice: " + outPrice.ToString();

                buttons[i].transform.GetComponent<UIPointer>().SetupInfo(outTitle, outSprite, description, 0);
                buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                buttons[i].GetComponent<Button>().onClick.AddListener(() => BuyEnemy(sIndex, outEnergyIncome));
            }
        }

    }
    private void BuyTower(int towerIndex)
    {
        towerDefenseAI.HighlightPlaceForBuilding(towerIndex);
    }

    private void BuyEnemy(int enemyIndex, int outEnergyIncome)
    {
        towerDefenseAI.BuyEnemy(enemyIndex, outEnergyIncome);
    }

}
