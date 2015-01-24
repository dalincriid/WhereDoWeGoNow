using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Labyrinth : MonoBehaviour
{
    private enum Direction
    {
        EAST,
        WEST,
        NORTH,
        SOUTH
    }

    #region VARIABLES
    [SerializeField]
    private int width = 0;
    [SerializeField]
    private int height = 0;
    [SerializeField]
    private GameObject wall = null;
    [SerializeField]
    private GameObject floor = null;

    public GameObject TMP = null;

    private int[,] maze;
    private int roomRate = 0;
    private System.Random random = null;
    #endregion

    #region PROPERTIES
    #endregion
    
    #region FUNCTIONS
    private void Shuffle<T>(T[] array)
    {
        for (int index = array.Length; index > 1; index--)
        {
            int spot = this.random.Next(index);
            T temporary = array[spot];
            array[spot] = array[index - 1];
            array[index - 1] = temporary;
        }
    }

    private T GetRandomEnum<T>()
    {
        System.Array array = System.Enum.GetValues(typeof(T));
        T value = (T)array.GetValue(this.random.Next(0, array.Length));
        return value;
    }

    private void PlaceWall(int line, int column)
    {
        GameObject wall = Instantiate(this.wall) as GameObject;
        Vector3 scale = wall.transform.localScale;
        Vector3 position = new Vector3(line * scale.x, scale.y / 2, column * scale.z);

        if (wall != null)
            wall.transform.position = position;
        
    }

    private void PlaceFloor()
    {
        Vector3 scale = this.wall.transform.localScale;
        GameObject floor = Instantiate(this.floor) as GameObject;
        Vector3 size = new Vector3(this.width, 1, this.height);

        if (!floor)
            return;
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = size;
    }

    private void DigRoom(Vector2 location, int breadth, int length, int horizontal, int vertical)
    {
        int width = (int)location.x;
        int height = (int)location.y;
        int lengthLimit = height + length * vertical;
        int breadthLimit = width + breadth * horizontal;

        if (breadthLimit <= 0 || breadthLimit >= this.width - 1)
        {
            this.DigPath(location);
            return;
        }
        if (lengthLimit <= 0 || lengthLimit >= this.height - 1)
        {
            this.DigPath(location);
            return;
        }

        while (width != breadthLimit)
        {
            height = (int)location.y;
            while (height != lengthLimit)
            {
                this.maze[width, height] = 0;

                height = height + 1 * vertical;
            }
            width = width + 1 * horizontal;
        }
    }

    private void DigPath(Vector2 location)
    {
        Direction[] directions = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        this.Shuffle(directions);

        for (int index = 0; index < directions.Length; index++)
        {
            switch (directions[index])
            {
                case Direction.NORTH:
                    if (location.y + 2 >= this.height - 1 || this.maze[(int)location.x, (int)location.y + 2] == 0)
                        continue;
                    this.maze[(int)location.x, (int)location.y + 1] = 0;
                    this.maze[(int)location.x, (int)location.y + 2] = 0;
                    this.Dig(new Vector2(location.x, location.y + 2));
                    break;
                    
                case Direction.EAST:
                    if (location.x + 2 >= this.width -1 || this.maze[(int)location.x + 2, (int)location.y] == 0)
                        continue;
                    this.maze[(int)location.x + 1, (int)location.y] = 0;
                    this.maze[(int)location.x + 2, (int)location.y] = 0;
                    this.Dig(new Vector2(location.x + 2, location.y));
                    break;
                case Direction.SOUTH:
                    if (location.y - 2 <= 0 || this.maze[(int)location.x, (int)location.y - 2] == 0)
                        continue;
                    this.maze[(int)location.x, (int)location.y - 1] = 0;
                    this.maze[(int)location.x, (int)location.y - 2] = 0;
                    this.Dig(new Vector2(location.x, location.y - 2));
                    break;
                case Direction.WEST:
                    if (location.x - 2 <= 0 || this.maze[(int)location.x - 2, (int)location.y] == 0)
                        continue;
                    this.maze[(int)location.x - 1, (int)location.y] = 0;
                    this.maze[(int)location.x - 2, (int)location.y] = 0;
                    this.Dig(new Vector2(location.x - 2, location.y));
                    break;
            }
        }

    }

    private void Dig(Vector2 location)
    {
        if (this.random.Next(1, 100) > this.roomRate)
            this.DigPath(location);
        else
            this.DigRoom(location, this.random.Next(1, this.width / 10), this.random.Next(1, this.height / 10), (this.random.Next(1) > 0) ? 1 : -1, (this.random.Next(1) > 0) ? 1 : -1);
    }

    private void Generate()
    {
        this.roomRate = this.random.Next(0, 50);
        this.maze = new int[this.width, this.height];

        for (int width = 0; width < this.width; width++)
            for (int height = 0; height < this.height; height++)
                this.maze[width, height] = 1;

        // Random Starting Cell
        Vector2 startingCell = new Vector2(1, 1);
        this.maze[(int)startingCell.x, (int)startingCell.y] = 0;

        // Depth-First Search Algorithm
        this.Dig(startingCell);
    }

    private void Build()
    {
        for (int width = 0; width < this.width; width++)
            for (int height = 0; height < this.height; height++)
                if (this.maze[width, height] == 1)
                    this.PlaceWall(width, height);
        this.PlaceFloor();
    }

    public List<Vector3> SitePlayer(int players)
    {
        Vector3 spawn = Vector3.zero;
        System.Random random = new System.Random();
        List<Vector3> spawers = new List<Vector3>();
        Vector3 scale = this.wall.transform.localScale;

        for (int index = 0; index < players; index++)
        {
            int length = 0;
            int breadth = 0;
            int spawner = 1;

            while (spawner == 1 || spawn == Vector3.zero || spawers.Contains(spawn))
            {
                breadth = random.Next(this.width);
                length = random.Next(this.height);
                spawner = this.maze[breadth, length];
                spawn = new Vector3(breadth * scale.x, 2, length * scale.z);
            }
            spawers.Add(spawn);
        }
        return spawers;
    }

    public void Compute(int seed)
    {
        this.random = new System.Random(seed);
        /* GENERATE MAZE */
        this.Generate();
        /* RENDER MAZE */
        this.Build();
    }
    #endregion
}
