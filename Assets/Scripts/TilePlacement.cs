using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    [SerializeField] Tile _tile;

    Grid grid;
    Tilemap tileMap;
    Renderer rendr;

    Vector3Int tilePos;
    Vector2Int gridSize;

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        rendr   = GetComponent<Renderer>();
        grid    = FindObjectOfType<Grid>();

        if (grid != null) {
            grid = grid.GetComponent<Grid>();
        }

        PlaceGroundTiles(_tile);
    }

    void PlaceGroundTiles(Tile aTile)
    {
        gridSize = new Vector2Int(Mathf.CeilToInt(rendr.material.mainTexture.width),
                                  Mathf.CeilToInt(rendr.material.mainTexture.height));

        /*
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++)
            {
                tilePos = new Vector3Int(x - Mathf.RoundToInt(gridSize.x / 2f), -(y - Mathf.RoundToInt(gridSize.y / 2f)), 0);
                tileMap.SetTile(tilePos, aTile);
            }
        }//*/
    }
}
