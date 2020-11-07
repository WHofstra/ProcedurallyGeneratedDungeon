using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    const float GRID_GEN_OFFSET     = 45f;
    const float COLOR_INTENSITY_MAX = 100f;

    [SerializeField] Tile[] _tile;
    [SerializeField] float _tileOccurrence;
    [SerializeField] Vector2Int _pixelSize;

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
        _pixelSize      = CheckPixelSize(_pixelSize);
        PlaceGroundTiles(_tile[0], _tile[1]);
    }

    void PlaceGroundTiles(Tile aTile, Tile otherTile)
    {
        float intensityBar         = (COLOR_INTENSITY_MAX - _tileOccurrence)/ COLOR_INTENSITY_MAX;

        gridSize = new Vector2Int(Mathf.FloorToInt(texture.Texture.width  / (GRID_GEN_OFFSET * grid.cellSize.x)),
                                  Mathf.FloorToInt(texture.Texture.height / (GRID_GEN_OFFSET * grid.cellSize.y)));

        //Tile Placement Based on Texture Pixel Intensity Values
        for (int j = 0; j < gridSize.y; j++) {
            for (int i = 0; i < gridSize.x; i++)
            {
                //Look for the 'r'-value per Pixel
                if (texture.Texture.GetPixel(Mathf.FloorToInt((i / grid.cellSize.x) / _pixelSize.x),
                    Mathf.FloorToInt((j / grid.cellSize.y) / _pixelSize.y)).r > intensityBar)
                {
                    tilePos = new Vector3Int(i - Mathf.RoundToInt(gridSize.x / 2f), -(j - Mathf.RoundToInt(gridSize.y / 2f)), 0);
                    tileMap.SetTile(tilePos, aTile);
                }
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
}
