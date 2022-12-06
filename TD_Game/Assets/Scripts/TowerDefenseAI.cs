using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CodeMonkey.Utils;
using TMPro;

public class TowerDefenseAI : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    private GridMap<GridNode> grid;
    private int map_width = 20;
    private int map_height = 10;
    [SerializeField] private TMP_Text energyt;
    private void Awake()
    {
        grid = new GridMap<GridNode>(map_width, map_height, 25f, Vector3.zero, (GridMap<GridNode> g, int x, int y) => new GridNode(g, x, y));
        _cam.transform.position = new Vector3(map_width*25/2,map_height*25/2,-10);
        CreateMap();
        StartCoroutine(GETA());
    }
    IEnumerator GETA()
    {
        
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        UnityWebRequest www = UnityWebRequest.Get("https://artsnow.pythonanywhere.com");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    private void Update()
    {
        energyt.text = GameResources.i.getEnergy().ToString();
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnEnemyWave_1();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemyWave_2();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnEnemyWave_3();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnTower(0);
        }
    }

    private void SpawnTower(int towerIndex)
    {
        int price = GameResources.i.getTowerPrice(towerIndex);
        if (GameResources.i.getEnergy() >= price)
        {
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
            GridNode gnode = grid.GetGridObject(spawnPosition);
            if (gnode.GetType() == 0)
            {
                GameResources.i.addEnergy(-price);
                Instantiate(GameAssets.i.pfTower, spawnPosition, Quaternion.identity);
                gnode.SetType(11);
            }
        }
    }
    private void CreateMap()
    {
        //int[] mapt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] mapt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int y = 0; y < map_height; y++)
        {
            for (int x = 0; x < map_width; x++)
            {
                GridNode gnode = grid.GetGridObject(x, y);
                gnode.SetType(mapt[y*map_width + x]);
                Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
                Transform terrain = Instantiate(GameAssets.i.pfTerrain, spawnPosition, Quaternion.identity);
                terrain.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameAssets.i.mapSprites[mapt[y * map_width + x]];
            }
        }
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

    private void SpawnEnemyWave_1()
    {
        float spawnTime = 0f;
        float timePerSpawn = .6f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemyWave_2()
    {
        float spawnTime = 0f;
        float timePerSpawn = .5f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Bat), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Bat), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemyWave_3()
    {
        float spawnTime = 0f;
        float timePerSpawn = .4f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Skelet), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Bat), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orc), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Bat), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Bat), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemy(Enemy.EnemyType enemyType)
    {
        Vector3 spawnPosition = new Vector3(37.5f, 262.5f);
        List<Vector3> waypointPositionList = new List<Vector3> {
            new Vector3(37.5f, 62.5f),
            new Vector3(387.5f, 62.5f),
            new Vector3(387.5f, -12.5f),
        };

        Enemy enemy = Enemy.Create(spawnPosition, enemyType);
        enemy.SetPathVectorList(waypointPositionList);

        //FunctionTimer.Create(() => enemy.Damage(15), .2f);
    }

    public class GridNode
    {

        private GridMap<GridNode> grid;
        private int x;
        private int y;
        private int type;

        public GridNode(GridMap<GridNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);
            //Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            //Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            //Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            //Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }
        public void SetType(int setType)
        {
            type = setType;
            grid.TriggerGridObjectChanged(x,y);
        }

        public int GetType()
        {
            return type;
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }

}
