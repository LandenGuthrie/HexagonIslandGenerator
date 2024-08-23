using System;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonIslandGenerator : MonoBehaviour
{
    public Transform HexagonPrefab;
    public Transform MapHolder;

    public int GridWidth = 11;
    public int GridLength = 11;
    public float Spacing = 0.0f;

    public float DropOffThreshold;
    public float HeightMultiplier;

    private float hexWidth = 1.732f;
    private float hexHeight = 2.0f;
    private Vector3 startPos;

    public int Seed = -1;

    void Start()
    {
        // Setting hexagon size
        hexWidth *= HexagonPrefab.localScale.x;
        hexHeight *= HexagonPrefab.localScale.z;

        // Updating seed
        if (Seed == -1)
        {
            Seed = UnityEngine.Random.Range(-5000, 5000);
        }

        // Setting properties
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    public void AddGap()
    {
        hexWidth += hexWidth * Spacing;
        hexHeight += hexHeight * Spacing;
    }
    public void CalcStartPos()
    {
        float offset = 0;
        if (GridLength / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (GridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (GridWidth / 2);

        startPos = new Vector3(x, 0, z);
    }
    public Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    public void ClearGrid()
    {
        foreach(Transform obj in MapHolder)
        {
            GameObject.Destroy(obj.gameObject);
        }
    }
    public float GetSeededPerlinNoise(float x, float y)
    {

        // Apply the seed to the input for Perlin noise
        return Mathf.PerlinNoise(x / 10f + Seed, y / 10f + Seed); // Adjust the scale as needed
    }
    public void CreateGrid()
    {
        Vector2 center = new Vector2(GridWidth, GridLength) / 2;

        for (int y = 0; y < GridLength; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {

                float distance = Vector2.Distance(new Vector2(x, y), center);
                float Noise = GetSeededPerlinNoise(x, y) / distance;

                float Height = (GetSeededPerlinNoise(x, y) * HeightMultiplier) * distance;

                if (Noise > DropOffThreshold)
                {
                    Transform hex = Instantiate(HexagonPrefab) as Transform;
                    Vector2 gridPos = new Vector2(x, y);
                    hex.position = CalcWorldPos(gridPos);
                    hex.parent = MapHolder;
                    hex.name = "Hexagon" + x + "|" + y;

                    hex.position = new Vector3(hex.position.x, hex.position.y + Height, hex.position.z);
                    

                }
            }
        }
    }

}

