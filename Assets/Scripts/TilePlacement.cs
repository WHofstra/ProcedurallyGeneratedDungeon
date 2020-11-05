using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    [SerializeField] Tile _tile;

    Tilemap tileMap;
    //Vector3Int pos;

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        //pos = new Vector3Int(0, 0, 0);

        //tileMap.SetTile(pos, _tile);
    }
}
