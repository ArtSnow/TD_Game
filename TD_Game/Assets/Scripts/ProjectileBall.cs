using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ProjectileBall : MonoBehaviour
{
    public static void Create(Vector3 spawnPosition, Enemy enemy, float damageAmount, int towerIndex)
    {
        Transform arrowTransform = Instantiate(GameAssets.i.pfProjectileBall, spawnPosition, Quaternion.identity);
        arrowTransform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = GameAssets.i.projectileSprites[towerIndex];
        ProjectileBall projectileArrow = arrowTransform.GetComponent<ProjectileBall>();
        projectileArrow.Setup(enemy, damageAmount);
    }

    private Enemy enemy;
    private float damageAmount;

    private void Setup(Enemy enemy, float damageAmount)
    {
        this.enemy = enemy;
        this.damageAmount = damageAmount;
    }

    private void Update()
    {
        if (enemy == null || enemy.IsDead())
        {
            // Enemy already dead
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = enemy.GetPosition();
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 130f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float angle = UtilsClass.GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);

        float destroySelfDistance = 1f;
        if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
        {
            enemy.Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
