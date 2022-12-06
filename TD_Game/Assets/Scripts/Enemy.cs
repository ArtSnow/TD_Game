using System;
using System.Collections.Generic;
using UnityEngine;
//using V_AnimationSystem;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour
{

    public interface IEnemyTargetable
    {
        Vector3 GetPosition();
        void Damage(Enemy attacker);
    }

    public static List<Enemy> enemyList = new List<Enemy>();

    public static Enemy GetClosestEnemy(Vector3 position, float maxRange)
    {
        Enemy closest = null;
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.IsDead()) continue;
            if (Vector3.Distance(position, enemy.GetPosition()) <= maxRange)
            {
                if (closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if (Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition()))
                    {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }


    public static Enemy Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.i.pfEnemy, position, Quaternion.identity);

        Enemy enemyHandler = enemyTransform.GetComponent<Enemy>();

        return enemyHandler;
    }

    public static Enemy Create(Vector3 position, EnemyType enemyType)
    {
        Transform enemyTransform = Instantiate(GameAssets.i.pfEnemy, position, Quaternion.identity);

        Enemy enemyHandler = enemyTransform.GetComponent<Enemy>();
        enemyHandler.SetEnemyType(enemyType);

        return enemyHandler;
    }

    public enum EnemyType
    {
        Skelet,
        Orc,
        Bat,
    }

    private float speed = 30f;
    private int reward = 0;
    private float damage = 1f;



    private HealthSystem healthSystem;
    private Character_Base characterBase;
    private State state;
    private Vector3 lastMoveDir;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private float pathfindingTimer;
    private Func<IEnemyTargetable> getEnemyTarget;

    //private UnitAnimType idleUnitAnim;
    //private UnitAnimType walkUnitAnim;

    private enum State
    {
        Normal,
        Attacking,
        Busy,
    }

    private void Awake()
    {
        enemyList.Add(this);
        characterBase = gameObject.GetComponent<Character_Base>();
        healthSystem = new HealthSystem(100);
        SetStateNormal();
    }

    private void Start()
    {
        //*
        World_Bar healthBar = new World_Bar(transform, new Vector3(0, 9), new Vector3(7, 1.5f), Color.grey, Color.red, 1f, "Objects2", 1000, new World_Bar.Outline { color = Color.black, size = .5f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };
        //*/
    }

    private void SetEnemyType(EnemyType enemyType)
    {
        Sprite sprite;
        switch (enemyType)
        {
            default:
            case EnemyType.Skelet:
                sprite = GameAssets.i.monsterSprites[0];
                healthSystem.SetHealthMax(80, true);
                reward = 10;
                speed = 30f;
                break;
            case EnemyType.Orc:
                sprite = GameAssets.i.monsterSprites[1];
                healthSystem.SetHealthMax(130, true);
                reward = 20;
                speed = 10f;
                break;
            case EnemyType.Bat:
                sprite = GameAssets.i.monsterSprites[2];
                healthSystem.SetHealthMax(50, true);
                reward = 30;
                speed = 50f;
                break;
        }
       transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetGetTarget(Func<IEnemyTargetable> getEnemyTarget)
    {
        this.getEnemyTarget = getEnemyTarget;
    }

    private void Update()
    {
        pathfindingTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Normal:
                HandleMovement();
                break;
            case State.Attacking:
                break;
            case State.Busy:
                break;
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    private void SetStateNormal()
    {
        state = State.Normal;
    }

    private void SetStateAttacking()
    {
        state = State.Attacking;
    }


    public void Damage(int damageAmount)
    {
        Vector3 bloodDir = UtilsClass.GetRandomDir();

        healthSystem.Damage(damageAmount);
        if (IsDead())
        {
            GameResources.i.addEnergy(reward);
            Destroy(gameObject);
        }
    }

    public void Damage(IEnemyTargetable attacker)
    {
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;

        healthSystem.Damage(30);
        if (IsDead())
        {
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        //pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(GetPosition(), targetPosition).pathVectorList;
        pathVectorList = new List<Vector3> { targetPosition };
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    public void SetPathVectorList(List<Vector3> pathVectorList)
    {
        this.pathVectorList = pathVectorList;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
