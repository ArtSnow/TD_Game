using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;
using System.Threading.Tasks;
using SimpleJSON;

public class TowerDefenseAI : MonoBehaviour
{

    [SerializeField] private Transform _cam;
    [SerializeField] private HUDMovement hUDMovement;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text maxWaveText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text timeToWaveText;
    [SerializeField] private Transform cellInfo;
    [SerializeField] private Transform loader;
    [SerializeField] private Transform canvas;
    [SerializeField] private Multiplayer mp;
    private SpriteRenderer infoColor;

    private GridMap<GridNode> grid;
    private int map_width = 20;
    private int map_height = 10;
    private bool isBuild = false;
    private int tIndex = -1;
    private int[] map;
    private JSONNode wave;

    private void Awake()
    {
        grid = new GridMap<GridNode>(map_width, map_height, 25f, Vector3.zero, (GridMap<GridNode> g, int x, int y) => new GridNode(g, x, y));
        _cam.transform.position = new Vector3(map_width*25/2,map_height*25/2,-10);
        CreateMap();
        infoColor = cellInfo.GetComponent<SpriteRenderer>();
        Loading();
        
    }

    private async void Loading()
    {
        canvas.gameObject.SetActive(false);
        loader.gameObject.SetActive(true);
        loader.Find("Code").GetComponent<TMP_Text>().text = Multiplayer.code;
        await CheckMode();
        loader.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);
        maxWaveText.text = (await mp.getWavesCount(Multiplayer.code)).ToString();
        await WaveManage();
    }

    private async Task WaveManage()
    {
        int duration = 30;
        int maxDuration = 120;
        int wavesCount = await mp.getWavesCount(Multiplayer.code);
        for (int i = 0; i < wavesCount; i++)
        {
            int waveTimer = duration;
            while (waveTimer > 0)
            {
                float timer = 0f;
                while (timer < 1f)
                {
                    timer = Mathf.Min(timer + Time.deltaTime / 1f, 1f);
                    await Task.Yield();
                }
                timer = 0f;
                waveTimer--;
                timeToWaveText.text = "Next wave: " + waveTimer.ToString() + " seconds.";
            }
            await SpawnWave(i);
            waveText.text = i.ToString();
            duration = Mathf.Min(++duration, maxDuration);
        }
    }
    private async Task CheckMode()
    {
        float timer = 0f;
        int closeTimer = 0;
        while (Multiplayer.mode != 1)
        {
            while (timer < 1f)
            {
                timer = Mathf.Min(timer + Time.deltaTime / 1f, 1f);
                await Task.Yield();
            }
            timer = 0f;
            closeTimer += 1;
            if (closeTimer > 360)
                break;
            Multiplayer.mode = await mp.CheckMode(Multiplayer.code);
            Debug.Log(Multiplayer.mode);
        }
    }
    private void Update()
    {
        energyText.text = GameResources.i.getEnergy().ToString();
        coinsText.text = GameResources.i.getCoins().ToString();
        healthText.text = GameResources.i.getHealth().ToString();
        if (isBuild)
        {
            Building();
        }
    }
    private void Building()
    {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
        cellInfo.position = spawnPosition;
        int celltype = 0;
        Tower t = null;
        try
        {
            GridNode gnode = grid.GetGridObject(spawnPosition);
            celltype = gnode.GetCellType();
            t = gnode.GetTower();
            if (t == null & celltype == 11)
            {
                gnode.SetCellType(0);
                celltype = 0;
            }
            cellInfo.gameObject.SetActive(true);
        }
        catch
        {
            celltype = -1;
            cellInfo.gameObject.SetActive(false);
        }
        if (celltype == 0)
        {
            infoColor.color = new Color(0f, 255f/255f, 0f, 100f/255f);
            if (Input.GetMouseButtonDown(0))
            {
                SpawnTower(tIndex);
                isBuild = false;
                cellInfo.gameObject.SetActive(false);
                hUDMovement.Show();
            }
        }
        else
        {
            infoColor.color = new Color(255f/255f, 0f, 0f, 100f/255f);
        }
        if (Input.GetMouseButtonDown(1))
        {
            isBuild = false;
            cellInfo.gameObject.SetActive(false);
            hUDMovement.Show();
        }
        
    }
    public void HighlightPlaceForBuilding(int towerIndex)
    {
        hUDMovement.Hide();
        isBuild = true;
        cellInfo.gameObject.SetActive(true);
        tIndex = towerIndex;
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
            if (gnode.GetCellType() == 0)
            {
                GameResources.i.addEnergy(-price);
                Tower tower = Tower.Create(spawnPosition, towerIndex, transform);
                gnode.SetCellType(11);
                gnode.SetTower(tower);
            }
        }
    }
    private void CreateMap()
    {
        int[] mapt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int y = 0; y < map_height; y++)
        {
            for (int x = 0; x < map_width; x++)
            {
                int index = mapt[y * map_width + x];
                GridNode gnode = grid.GetGridObject(x, y);
                gnode.SetCellType(index);
                Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
                Transform terrain = Instantiate(GameAssets.i.pfTerrain, spawnPosition, Quaternion.identity);
                terrain.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameAssets.i.mapSprites[index];
            }
        }
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }
    private async Task SpawnWave(int index)
    {
        float spawnTime = 0f;
        float timePerSpawn = .25f;
        JSONNode wave = await mp.getWave(Multiplayer.code, index);
        foreach(var kvp in wave)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                FunctionTimer.Create(() => SpawnEnemy(int.Parse(kvp.Key)), spawnTime); spawnTime += timePerSpawn;
            }
        }
    }
    private void SpawnEnemy(int index)
    {
        Vector3 spawnPosition = new Vector3(37.5f, 250f);
        List<Vector3> waypointPositionList = new List<Vector3> {
            new Vector3(37.5f, 62.5f),
            new Vector3(387.5f, 62.5f),
            new Vector3(387.5f, 0f),
        };
        Enemy enemy = Enemy.Create(spawnPosition, index);
        enemy.SetPathVectorList(waypointPositionList);
    }

    public class GridNode
    {

        private GridMap<GridNode> grid;
        private int x;
        private int y;
        private int type;
        private Tower tower = null;

        public GridNode(GridMap<GridNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);
        }
        public void SetCellType(int setType)
        {
            type = setType;
            grid.TriggerGridObjectChanged(x,y);
        }

        public void SetTower(Tower setTower)
        {
            tower = setTower;
        }
        public Tower GetTower()
        {
            return tower;
        }
        public int GetCellType()
        {
            return type;
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }

}
