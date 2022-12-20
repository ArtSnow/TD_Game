using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour

{
    private static GameResources _i;
    public static GameResources i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load("GameResources") as GameObject).GetComponent<GameResources>();
            return _i;
        }
    }

    private int coins;
    private int energy;
    private int health;
    private int energyIncome;
    private int towersCount;
    private int wave;

    public struct TowerStats 
    {
        public string title;
        public Sprite sprite;
        public float range;
        public float damage;
        public float shootTimer;
        public int price;

        public float damageUC;
        public float rangeUC;
        public float shootTimerUC;
        public TowerStats(string inTitle, Sprite inSprite, float inRange, int inDamageAmount, float inShootTimerMax, int inPrice, float inDamageUC, float inRangeUC, float inShootTimerUC)
        {
            title = inTitle;
            sprite = inSprite;
            range = inRange;
            damage = inDamageAmount;
            shootTimer = inShootTimerMax;
            price = inPrice;
            damageUC = inDamageUC;
            rangeUC = inRangeUC;
            shootTimerUC = inShootTimerUC;
        }
    }

    private TowerStats[] towers;

    public struct EnemyStats
    {
        public string title;
        public Sprite sprite;
        public int damage;
        public int maxHealth;
        public int price;
        public int energyReward;
        public int coinsReward;
        public int armor;
        public float speed;
        public int energyIncome;

        public EnemyStats(string inTitle, Sprite inSprite, int inDamage, int inMaxHealth, int inPrice, int inEnergyReward, int inCoinsReward, int inArmor, float inSpeed, int inEnergyIncome)
        {
            title = inTitle;
            sprite = inSprite;
            damage = inDamage;
            maxHealth = inMaxHealth;
            price = inPrice;
            energyReward = inEnergyReward;
            coinsReward = inCoinsReward;
            armor = inArmor;
            speed = inSpeed;
            energyIncome = inEnergyIncome;
        }
    }

    private EnemyStats[] enemies;



    public void addCoins(int value)
    {
        coins += value;
    }
    public void setCoins(int value)
    {
        coins = value;
    }
    public int getCoins()
    {
        return coins;
    }
    public void addEnergy(int value)
    {
        energy += value;
    }
    public void addEnergyIncome(int value)
    {
        energyIncome += value;
    }
    public int getEnergyIncome()
    {
        return energyIncome;
    }

    public void setEnergy(int value)
    {
        energy = value;
    }
    public int getEnergy()
    {
        return energy;
    }
    public void getTower(int index, out string outTitle, out Sprite outSprite, out float outRange, out float outDamageAmount, out float outShootTimerMax, out int outPrice, out float outDamageUC, out float outRangeUC, out float outShootTimerUC)
    {
        TowerStats tower = towers[index];
        outTitle = tower.title;
        outSprite = tower.sprite;
        outRange = tower.range;
        outDamageAmount = tower.damage;
        outShootTimerMax = tower.shootTimer;
        outPrice = tower.price;
        outDamageUC = tower.damageUC;
        outRangeUC = tower.rangeUC;
        outShootTimerUC = tower.shootTimerUC;
    }

    public void getEnemy(int index, out string outTitle, out Sprite outSprite, out int outDamage, out int outMaxHealth, out int outPrice, out int outEnergyReward, out int outCoinsReward, out int outArmor, out float outSpeed, out int outEnergyIncome)
    {
        EnemyStats enemy = enemies[index];
        outTitle = enemy.title;
        outSprite = enemy.sprite;
        outDamage = enemy.damage;
        outMaxHealth = enemy.maxHealth;
        outPrice = enemy.price;
        outEnergyReward = enemy.energyReward;
        outCoinsReward = enemy.coinsReward;
        outArmor = enemy.armor;
        outSpeed = enemy.speed;
        outEnergyIncome = enemy.energyIncome;
    }

    public int getTowerPrice(int index)
    {
        TowerStats tower = towers[index];
        return tower.price;
    }

    public int getEnemyPrice(int index)
    {
        EnemyStats enemy = enemies[index];
        return enemy.price;
    }

    public void setHealth(int value)
    {
        health = value;
    }

    public void addHealth(int value)
    {
        health += value;
    }

    public int getHealth()
    {
        return health;
    }

    public void setWave(int value)
    {
        wave = value;
    }

    public int getWave()
    {
        return wave;
    }

    public void addTowersCount(int value)
    {
        towersCount += value;
    }

    public int getTowersCount()
    {
        return towersCount;
    }
    public GameResources()
    {
        energy = 75;
        coins = 0;
        health = 30;
        energyIncome = 0;
        towersCount = 0;
        wave = 0;
        towers = new TowerStats[5];
        towers[0] = new TowerStats("First", GameAssets.i.towerSprites[0], 70f, 25, .8f, 25, 1.1f, 1.05f, 1.05f);
        towers[1] = new TowerStats("Second", GameAssets.i.towerSprites[1], 100f, 100, 2.5f, 25, 1.05f, 1.1f, 1.05f);
        towers[2] = new TowerStats("Third", GameAssets.i.towerSprites[2], 50f, 10, .2f, 25, 1.05f, 1.05f, 1.1f);
        towers[3] = new TowerStats("Locked", GameAssets.i.towerSprites[3], 0, 0, 0, -1, 0, 0, 0);
        towers[4] = new TowerStats("Locked", GameAssets.i.towerSprites[4], 0, 0, 0, -1, 0, 0, 0);
        enemies = new EnemyStats[5];
        enemies[0] = new EnemyStats("Skeleton", GameAssets.i.monsterSprites[0], 2, 80, 10, 1, 1, 1, 30f, 1);
        enemies[1] = new EnemyStats("Orc", GameAssets.i.monsterSprites[1], 3, 200, 20, 2, 2, 3, 10f, 2);
        enemies[2] = new EnemyStats("Bat", GameAssets.i.monsterSprites[2], 1, 40, 20, 2, 2, 0, 70f, 2);
        enemies[3] = new EnemyStats("Locked", GameAssets.i.monsterSprites[3], 0, 0, -1, 0, 0, 0, 0, 0);
        enemies[4] = new EnemyStats("Locked", GameAssets.i.monsterSprites[4], 0, 0, -1, 0, 0, 0, 0, 0);
    }
}
