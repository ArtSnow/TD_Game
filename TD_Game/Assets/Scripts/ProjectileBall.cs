using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBall : MonoBehaviour
{
    public static void Create(Vector3 spawnPosition)
    {
        Instantiate(GameAssets.i.pfProjectileBall, spawnPosition, Quaternion.identity);
    }
}
