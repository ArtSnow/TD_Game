using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Threading.Tasks;

public class Enemy : MonoBehaviour
{


    private float speed = 30f;
    private int energyReward = 0;
    private int coinsReward = 0;
    private int damage = 1;
    private int armor = 1;
    private int energyIncome = 1;
    private Animator animator;


    private HealthSystem healthSystem;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

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

    public static Enemy Create(Vector3 position, int enemyIndex, int waveIndex)
    {
        Transform enemyTransform = Instantiate(GameAssets.i.pfEnemy, position, Quaternion.identity);

        Enemy enemyHandler = enemyTransform.GetComponent<Enemy>();
        enemyHandler.SetEnemyType(enemyIndex, waveIndex);

        return enemyHandler;
    }



    private void Awake()
    {
        enemyList.Add(this);
        healthSystem = new HealthSystem(100);
        animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    private void Start()
    {
        World_Bar healthBar = new World_Bar(transform, new Vector3(0, 9), new Vector3(7, 1.5f), Color.grey, Color.red, 1f, "Objects2", 1000, new World_Bar.Outline { color = Color.black, size = .5f });
        healthSystem.OnHealthChanged += (object sender, EventArgs e) => {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        };
    }

    private void SetEnemyType(int enemyIndex, int waveIndex)
    {
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
        float multi = Mathf.FloorToInt(waveIndex / 10) + 1;

        GameResources.i.getEnemy(enemyIndex, out outTitle, out outSprite, out outDamage, out outMaxHealth, out outPrice, out outEnergyReward, out outCoinsReward, out outArmor, out outSpeed, out outEnergyIncome);
        damage = outDamage;
        healthSystem.SetHealthMax(Mathf.RoundToInt(outMaxHealth * multi), true);
        energyReward = outEnergyReward;
        coinsReward = outCoinsReward;
        armor = Mathf.RoundToInt(outArmor * multi - (multi - 1) / 2);
        speed = Mathf.RoundToInt(outSpeed * multi - (multi - 1) / 2);
        energyIncome = outEnergyIncome;
        transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = outSprite;
        transform.Find("Sprite").GetComponent<Animator>().runtimeAnimatorController = GameAssets.i.monsterAnimators[enemyIndex];
    }


    private void Update()
    {
        HandleMovement();
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public async void Damage(float damageAmount)
    {
        healthSystem.Damage(damageAmount-armor);
        if (IsDead())
        {
            speed = 0;
            GameResources.i.addEnergy(energyReward);
            GameResources.i.addCoins(coinsReward);
            transform.GetComponent<AudioSource>().PlayOneShot(GameAssets.i.enemyExplode);
            animator.SetTrigger("Die");
            await Task.Delay(500);
            Destroy(gameObject);
        } else
        {
            animator.SetTrigger("Hurt");
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
        GameResources.i.addHealth(-damage);
        GeneralTower.animator.SetTrigger("Hurt");
        GeneralTower.audioS.PlayOneShot(GameAssets.i.GeneralTowerHurt);
        healthSystem.Damage(healthSystem.GetHealthMax());
        Destroy(gameObject);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
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
