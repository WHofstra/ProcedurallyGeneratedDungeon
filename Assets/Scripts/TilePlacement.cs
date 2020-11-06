using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    const float GRID_GEN_OFFSET     = 45f;
    const float COLOR_INTENSITY_MAX = 100f;

    [SerializeField] Tile _tile;
    [SerializeField] float _tileOccurrence;

    Grid grid;
    Tilemap tileMap;
    PerlinNoiseTexture texture;

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
        PlaceGroundTiles(_tile);
    }

    void PlaceGroundTiles(Tile aTile)
    {
        float intensityBar = (COLOR_INTENSITY_MAX - _tileOccurrence)/ COLOR_INTENSITY_MAX;
        gridSize = new Vector2Int(Mathf.CeilToInt(texture.Texture.width  / (GRID_GEN_OFFSET * grid.cellSize.x)),
                                  Mathf.CeilToInt(texture.Texture.height / (GRID_GEN_OFFSET * grid.cellSize.y)));

        for (int j = 0; j < gridSize.y; j++) {
            for (int i = 0; i < gridSize.x; i++)
            {
                if (texture.Texture.GetPixel(Mathf.FloorToInt(i / grid.cellSize.x),
                    Mathf.FloorToInt(j / grid.cellSize.y)).r > intensityBar)
                {
                    tilePos = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f), -(j - Mathf.RoundToInt(gridSize.y / 2f)), 0);
                    tileMap.SetTile(tilePos, aTile);
                }
            }
        }
    }

    float CheckColorIntensity(float aColor)
    {
        if (aColor > COLOR_INTENSITY_MAX) {
            return COLOR_INTENSITY_MAX;
        }
        else if (aColor < 0f) {
            return 0;
        }
        return aColor;
    }
}
