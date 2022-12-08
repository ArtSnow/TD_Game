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
    private int health = 30;
    private int wave = 1;
    private int maxWave = 3;
    private int[] towerPrice = { 100, 200, 300 };
    private int[] monsterPrice = { 10, 20, 30 };

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
    public void setEnergy(int value)
    {
        energy = value;
    }
    public int getEnergy()
    {
        return energy;
    }
    public int getTowerPrice(int index)
    {
        return towerPrice[index];
    }
    public int getMonsterPrice(int index)
    {
        return monsterPrice[index];
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
    public int getWave()
    {
        return wave;
    }
    public void nextVawe()
    {
        wave++;
    }

    public void setMaxWave(int value)
    {
        maxWave = value;
    }
    public int getMaxWave()
    {
        return maxWave;
    }
    public GameResources()
    {
        energy = 300;
        coins = 0;
    }
}
