using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject blockPrefab; // кубик 0.25x0.25

    [SerializeField]
    int numX = 10;
    [SerializeField]
    int numY = 10;

    float blockSize = 0.25f;

    GameObject[,] blocks; // розмір 2*numX+1, 2*numY+1

    bool[,] visited; // для клітинок (не для стін!)

    Stack<Vector2Int> stack = new Stack<Vector2Int>();

    bool generating = false;

    void Start()
    {
        CreateGrid();
        CreateMaze();
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

                if (x % 2 == 1 && y % 2 == 1)
                {
                    blocks[x, y] = null;
                }
                else
                {
                    GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
                    block.name = $"Block_{x}_{y}";
                    blocks[x, y] = block;
                }
            }
        }
    }

    List<Vector2Int> GetUnvisitedNeighbours(int x, int y)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // 4 напрямки (по клітинках, не по блоках)
        if (x > 0 && !visited[x - 1, y]) neighbours.Add(new Vector2Int(x - 1, y));
        if (x < numX - 1 && !visited[x + 1, y]) neighbours.Add(new Vector2Int(x + 1, y));
        if (y > 0 && !visited[x, y - 1]) neighbours.Add(new Vector2Int(x, y - 1));
        if (y < numY - 1 && !visited[x, y + 1]) neighbours.Add(new Vector2Int(x, y + 1));

        return neighbours;
    }

    void CreateMaze()
    {
        if (generating) return;

        for (int x = 0; x < numX; x++)
            for (int y = 0; y < numY; y++)
                visited[x, y] = false;

        stack.Clear();

        Vector2Int current = new Vector2Int(0, 0);
        visited[0, 0] = true;
        stack.Push(current);

        StartCoroutine(GenerateCoroutine());
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

                // Видаляємо стіну між current і chosen
                RemoveWallBetween(current, chosen);

                stack.Push(chosen);
            }
            else
            {
                stack.Pop();
            }

            yield return new WaitForSeconds(0.01f);
        }

        generating = false;
    }

    void RemoveWallBetween(Vector2Int a, Vector2Int b)
    {
        // Розрахунок позиції між двома клітинками
        int wallX = a.x + b.x + 1; // +1 через індексацію в blocks
        int wallY = a.y + b.y + 1;

        // В системі координат blocks вони вдвічі більші і зсунути

        // Залишаємо блоки клітинок, а стіну між ними видаляємо:
        if (blocks[wallX, wallY] != null)
        {
            Destroy(blocks[wallX, wallY]);
            blocks[wallX, wallY] = null;
        }
    }
}