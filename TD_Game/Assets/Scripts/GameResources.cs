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

    private int gold;
    private int energy;
    private int[] towerPrice = { 100, 200, 300 };
    private int[] monsterPrice = { 10, 20, 30 };

    public void addGold(int value)
    {
        gold += value;
    }
    public void setGold(int value)
    {
        gold = value;
    }
    public int getGold()
    {
        return gold;
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
    public GameResources()
    {
        energy = 300;
        gold = 0;
    }
}
