﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    const float GRID_GEN_OFFSET     = 50f;
    const float COLOR_INTENSITY_MAX = 100f;
    const int BRIDGE_LENGTH_MAX     = 300;

    [SerializeField] Tile[] _tile;
    [SerializeField] float _tileOccurrence;
    [SerializeField] Vector2Int _pixelSize;

    Grid grid;
    Tilemap tileMap;
    PerlinNoiseTexture texture;

    List<Vector2Int> centerPoint = new List<Vector2Int>();
    Vector3Int[] tileNeighbor    = new Vector3Int[2];
    Vector3Int tilePos;
    Vector2Int gridSize;

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        texture = GetComponent<PerlinNoiseTexture>();
        grid    = FindObjectOfType<Grid>();

        if (grid != null) {
            grid = grid.GetComponent<Grid>();
            transform.position = new Vector3(transform.position.x - (grid.cellSize.x / 2f),
                                             transform.position.y - grid.cellSize.y, transform.position.z);
        }

        _tileOccurrence = CheckColorIntensity(_tileOccurrence);
        _pixelSize      = CheckPixelSize(_pixelSize);
        PlaceGroundTiles(_tile[0], _tile[1]);
    }

    void PlaceGroundTiles(Tile aTile, Tile otherTile)
    {
        float intensityBar = (COLOR_INTENSITY_MAX - _tileOccurrence)/ COLOR_INTENSITY_MAX;
        gridSize           = new Vector2Int(Mathf.CeilToInt(texture.Texture.width  / (GRID_GEN_OFFSET * grid.cellSize.x)),
                                            Mathf.CeilToInt(texture.Texture.height / (GRID_GEN_OFFSET * grid.cellSize.y)));

        //Tile Placement Based on Texture Pixel Intensity Values
        for (int j = 0; j < gridSize.y; j++) {
            for (int i = 0; i < gridSize.x; i++)
            {
                //Look for the 'r'-value per Pixel
                if (texture.Texture.GetPixel(Mathf.FloorToInt((i / grid.cellSize.x) / _pixelSize.x),
                    Mathf.FloorToInt((j / grid.cellSize.y) / _pixelSize.y)).r > intensityBar)
                {
                    tileNeighbor[0] = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f) - 1,
                                                   -(j - Mathf.RoundToInt(gridSize.y / 2f)), 0);
                    tileNeighbor[1] = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f),
                                                   -(j - Mathf.RoundToInt(gridSize.y / 2f) - 1), 0);

                    tilePos = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f), -(j - Mathf.RoundToInt(gridSize.y / 2f)), 0);

                    //Find the Center of a Room
                    if (tileMap.GetTile(tileNeighbor[0]) == null && tileMap.GetTile(tileNeighbor[1]) == null)
                    {
                        tilePos = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f) + Mathf.FloorToInt(grid.cellSize.x * _pixelSize.x * 0.5f),
                                               -(j - Mathf.RoundToInt(gridSize.y / 2f) + Mathf.FloorToInt(grid.cellSize.y * _pixelSize.y * 0.5f)), 0);
                        tileMap.SetTile(tilePos, otherTile);
                        centerPoint.Add(new Vector2Int(tilePos.x, tilePos.y));
                    }

                    tilePos = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f),
                                           -(j - Mathf.RoundToInt(gridSize.y / 2f)), 0);

                    //Place Tile if There is None Placed Yet
                    if (tileMap.GetTile(tilePos) == null) {
                        tileMap.SetTile(tilePos, aTile);
                    }
                }
            }
        }

        //Draw Connecting Bridges
        if (centerPoint.Count > 0) {
            for (int i = 0; i < (centerPoint.Count - 1); i++) {
                PlaceHorizontalBridges(centerPoint[i], centerPoint[i + 1], aTile);
            }

            centerPoint = SortOnXValues(centerPoint);

            /* //To Check Sort in Debug
            foreach (Vector2Int point in centerPoint) {
                Debug.Log(point);
            }//*/

            //*
            for (int i = (centerPoint.Count - 1); i > 0; i--) {
                PlaceVerticalBridges(centerPoint[i], centerPoint[i - 1], aTile);
            }//*/
        }
    }

    void PlaceHorizontalBridges(Vector2Int posA, Vector2Int posB, Tile aTile)
    {
        //Check for the Exact Same 'y'-value
        if (posA.y == posB.y)
        {
            int killSwitch = 0;
            tileNeighbor[0] = new Vector3Int(posA.x + 1, posA.y, 0);
            tileNeighbor[1] = new Vector3Int(posB.x, posB.y, 0);

            //Walk Till You Get There
            while (tileNeighbor[0] != tileNeighbor[1] && killSwitch < BRIDGE_LENGTH_MAX)
            {
                if (tileMap.GetTile(tileNeighbor[0]) == null) {
                    tileMap.SetTile(tileNeighbor[0], aTile);
                }
                tileNeighbor[0].x++;
                killSwitch++;
            }
        }
    }

    void PlaceVerticalBridges(Vector2Int posA, Vector2Int posB, Tile aTile)
    {
        //Check for the Exact Same 'x'-value
        if (posA.x == posB.x)
        {
            int killSwitch  = 0;
            tileNeighbor[0] = new Vector3Int(posA.x, posA.y - 1, 0);
            tileNeighbor[1] = new Vector3Int(posB.x, posB.y, 0);

            //Walk Till You Get There
            while (tileNeighbor[0] != tileNeighbor[1] && killSwitch < BRIDGE_LENGTH_MAX)
            {
                if (tileMap.GetTile(tileNeighbor[0]) == null) {
                    tileMap.SetTile(tileNeighbor[0], aTile);
                }
                tileNeighbor[0].y--;
                killSwitch++;
            }
        }
    }

    float CheckColorIntensity(float aColor)
    {
        //Check for a Value Between 0 and 100
        if (aColor > COLOR_INTENSITY_MAX) {
            return COLOR_INTENSITY_MAX;
        }
        else if (aColor < 0f) {
            return 0f;
        }
        return aColor;
    }

    Vector2Int CheckPixelSize(Vector2Int aSize)
    {
        //Prevent a 0-value
        if (aSize.x <= 0) {
            aSize.x = 1;
        }

        if (aSize.y <= 0) {
            aSize.y = 1;
        }

        return aSize;
    }

    List<Vector2Int> SortOnXValues(List<Vector2Int> currentList)
    {
        Vector2Int[] sorter = currentList.ToArray();

        //Selection Sorting
        for (int i = 0; i < sorter.Length; i++)
        {
            int minimumIndex = i;
            for (var j = i + 1; j < sorter.Length; j++)
            {
                if (sorter[minimumIndex].x > sorter[j].x ||
                   (sorter[minimumIndex].x == sorter[j].x && sorter[minimumIndex].y > sorter[j].y)) {
                    minimumIndex = j;
                }
            }

            if (minimumIndex != i)
            {
                Vector2Int temp      = sorter[minimumIndex];
                sorter[minimumIndex] = sorter[i];
                sorter[i]            = temp;
            }
        }

        return sorter.ToList<Vector2Int>();
    }
}
