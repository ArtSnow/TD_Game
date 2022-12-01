using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Transform pfProjectileBall;
    public Transform pfTower;
    public Transform pfEnemy;
    public Transform pfTerrain;
    public Sprite[] mapSprites;
}
