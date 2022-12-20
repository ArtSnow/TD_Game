

using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    private Vector3 projectileShootFromPosition;
    private float range;
    private int index;
    private float damageAmount;
    private float shootTimerMax;
    private float shootTimer;
    private float damageUC;
    private float rangeUC;
    private float shootTimerUC;
    private string title;
    private int price;
    private Sprite sprite;
    

    private void Awake()
    {
        projectileShootFromPosition = transform.Find("Sprite").Find("ProjectileShootFromPosition").position;
    }


    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;

            Enemy enemy = GetClosestEnemy();
            if (enemy != null)
            {
                // Enemy in range!
                ProjectileBall.Create(projectileShootFromPosition, enemy, damageAmount, index);
            }
        }
    }

    private void SetTowerType(int towerIndex)
    {
        string outTitle;
        Sprite outSprite;
        float outRange;
        float outDamageAmount;
        float outShootTimerMax;
        float outRangeUC;
        float outDamageUC;
        float outShootTimerUC;
        int outPrice;
        GameResources.i.getTower(towerIndex, out outTitle, out outSprite, out outRange, out outDamageAmount, out outShootTimerMax, out outPrice, out outDamageUC, out outRangeUC, out outShootTimerUC);
        range = outRange;
        damageAmount = outDamageAmount;
        shootTimerMax = outShootTimerMax;
        damageUC = outDamageUC;
        rangeUC = outRangeUC;
        shootTimerUC = outShootTimerUC;
        title = outTitle;
        price = (int)(outPrice/2);
        sprite = outSprite;
        index = towerIndex;

        transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = outSprite;
    }

    public static Tower Create(Vector3 position, int towerIndex, Transform go)
    {
        Transform towerTransform = Instantiate(GameAssets.i.pfTower, position, Quaternion.identity, go);
        Tower towerHandler = towerTransform.GetComponent<Tower>();
        towerHandler.SetTowerType(towerIndex);

        return towerHandler;
    }


    private Enemy GetClosestEnemy()
    {
        return Enemy.GetClosestEnemy(transform.position, range);
    }
    public void GetStats(out string outTitle, out Sprite outSprite, out float outRange, out float outDamageAmount, out float outShootTimerMax, out int outPrice)
    {
        outTitle = title;
        outSprite = sprite;
        outRange = range;
        outDamageAmount = damageAmount;
        outShootTimerMax = shootTimerMax;
        outPrice = price;
    }
    public float GetRange()
    {
        return range;
    }

    public void UpgradeRange()
    {
        range += 10f;
    }

    public void UpgradeDamageAmount()
    {
        damageAmount += 5;
    }
    public void UpgradeShootTimer()
    {
        shootTimerMax -= 0.1f;
    }

    public void Upgrade()
    {
        damageAmount *= damageUC;
        range *= rangeUC;
        shootTimerMax /= shootTimerUC;
        price = (int)(price * 1.25);
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        TowerRangeOverlay.Show_Static(this);
        HUDStats.hUDStats.SetupHUD(this);
    }


}
