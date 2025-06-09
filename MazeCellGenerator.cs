using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject blockPrefab; // The cube prefab (recommended size: 0.25x0.25)

    [SerializeField]
    int numX = 10; // Number of horizontal maze cells
    [SerializeField]
    int numY = 10; // Number of vertical maze cells

    float blockSize = 0.25f;

    GameObject[,] blocks; // Grid including walls: size = (2*numX + 1, 2*numY + 1)

    bool[,] visited; // Tracks visited maze cells (not walls)

    Stack<Vector2Int> stack = new Stack<Vector2Int>(); // For DFS traversal

    bool generating = false;

    void Start()
    {
        CreateGrid();   // Create full grid with walls and empty maze cells
        CreateMaze();   // Start maze generation
    }

    void CreateGrid()
    {
        int width = numX * 2 + 1;
        int height = numY * 2 + 1;

        blocks = new GameObject[width, height];
        visited = new bool[numX, numY];

        float offsetX = (width - 1) / 2f * blockSize;
        float offsetY = (height - 1) / 2f * blockSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 localPos = new Vector3(x * blockSize - offsetX, y * blockSize - offsetY, 0);
                Vector3 pos = transform.TransformPoint(localPos);

                // Every odd cell (x % 2 == 1 && y % 2 == 1) is a maze path cell, not a wall
                if (x % 2 == 1 && y % 2 == 1)
                {
                    blocks[x, y] = null; // Maze cell (walkable)
                }
                else
                {
                    GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
                    block.name = $"Block_{x}_{y}";
                    blocks[x, y] = block; // Wall block
                }
            }
        }
    }

    List<Vector2Int> GetUnvisitedNeighbours(int x, int y)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // Check 4-directional neighbors (within maze cell range)
        if (x > 0 && !visited[x - 1, y]) neighbours.Add(new Vector2Int(x - 1, y));
        if (x < numX - 1 && !visited[x + 1, y]) neighbours.Add(new Vector2Int(x + 1, y));
        if (y > 0 && !visited[x, y - 1]) neighbours.Add(new Vector2Int(x, y - 1));
        if (y < numY - 1 && !visited[x, y + 1]) neighbours.Add(new Vector2Int(x, y + 1));

        return neighbours;
    }

    void CreateMaze()
    {
        if (generating) return;

        // Reset visited state
        for (int x = 0; x < numX; x++)
            for (int y = 0; y < numY; y++)
                visited[x, y] = false;

        stack.Clear();

        Vector2Int current = new Vector2Int(0, 0);
        visited[0, 0] = true;
        stack.Push(current);

        StartCoroutine(GenerateCoroutine()); // Coroutine for step-by-step generation
    }

    IEnumerator GenerateCoroutine()
    {
        generating = true;

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();

            var neighbours = GetUnvisitedNeighbours(current.x, current.y);

            if (neighbours.Count > 0)
            {
                Vector2Int chosen = neighbours[Random.Range(0, neighbours.Count)];
                visited[chosen.x, chosen.y] = true;

                RemoveWallBetween(current, chosen); // Break the wall between cells

                stack.Push(chosen); // Continue traversal
            }
            else
            {
                stack.Pop(); // Backtrack
            }

            yield return new WaitForSeconds(0.01f); // Small delay for visualization
        }

        generating = false;
    }

    void RemoveWallBetween(Vector2Int a, Vector2Int b)
    {
        // Calculate the wall block between two maze cells in the block grid
        int wallX = a.x + b.x + 1; // +1 to convert to block index space
        int wallY = a.y + b.y + 1;

        if (blocks[wallX, wallY] != null)
        {
            Destroy(blocks[wallX, wallY]); // Destroy the wall between two maze cells
            blocks[wallX, wallY] = null;
        }
    }
}
