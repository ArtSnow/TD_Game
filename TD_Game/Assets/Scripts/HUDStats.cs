using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDStats : MonoBehaviour
{
    public static HUDStats hUDStats { get; private set; }
    private int type;
    private TMP_Text title;
    private SpriteRenderer sprite;
    private TMP_Text description;
    private Transform upgradeButton;
    private Transform sellButton;

    private void Awake()
    {
        title = transform.Find("Title").GetComponent<TMP_Text>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        description = transform.Find("Description").GetComponent<TMP_Text>();
        upgradeButton = transform.Find("UpgradeButton");
        sellButton = transform.Find("SellButton");

        hUDStats = this;
    }

    public void SetupHUD(string inTitle, Sprite inSprite, string inDescription, int inType)
    {
        title.text = inTitle;
        sprite.sprite = inSprite;
        description.text = inDescription;
        type = inType;
        if (type == 0)
        {
            upgradeButton.gameObject.SetActive(false);
            sellButton.gameObject.SetActive(false);
        }

    }

    public void SetupHUD(Tower tower)
    {
        string outTitle;
        Sprite outSprite;
        float outRange;
        float outDamageAmount;
        float outShootTimerMax;
        int outPrice;
        tower.GetStats(out outTitle, out outSprite, out outRange, out outDamageAmount, out outShootTimerMax, out outPrice);
        title.text = outTitle;
        sprite.sprite = outSprite;
        type = 1;
        string outDescription = "Range: " + outRange.ToString("0.00") + "\nDamage: " + outDamageAmount.ToString("0.00") + "\nAttackTime: " + outShootTimerMax.ToString("0.00") + "\nPrice: " + outPrice.ToString();
        description.text = outDescription;
        upgradeButton.gameObject.SetActive(true);
        sellButton.gameObject.SetActive(true);
        upgradeButton.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        sellButton.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        upgradeButton.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            if(GameResources.i.getEnergy() > outPrice)
            {
                GameResources.i.addEnergy(-outPrice);
                tower.Upgrade();
                SetupHUD(tower);
            }
        });
        sellButton.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            GameResources.i.addEnergy((int)(outPrice));
            Destroy(tower.gameObject);
            StartCoroutine(Sold());
            
        });
    }

    IEnumerator Sold()
    {
        yield return new WaitForSeconds(0.1f);
        SetupHUD("Sold!", sprite.sprite, "", 0);
    }
}