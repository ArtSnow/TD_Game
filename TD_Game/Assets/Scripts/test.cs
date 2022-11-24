using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    // Start is called before the first frame update
    void Start()
    {
        GridMap<MapGridObject> grid = new GridMap<MapGridObject>(20, 10, 14f, new Vector3(-10, -5), (GridMap<MapGridObject> grid, int x, int y) => new MapGridObject(grid, x, y));
        _cam.transform.position = new Vector3(10, 5, -10);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}

public class MapGridObject
{
    private GridMap<MapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public MapGridObject(GridMap<MapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
