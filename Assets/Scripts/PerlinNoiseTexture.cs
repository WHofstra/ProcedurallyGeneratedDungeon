using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseTexture : MonoBehaviour
{
    [SerializeField] int _pixWidth;
    [SerializeField] int _pixHeight;

    Texture2D texture;
    Color[] color;
    Renderer rendr;

    Vector2 originPoint = new Vector2(0, 0);
    float scale = 1f;

    void Start()
    {
        rendr = GetComponent<Renderer>();

        texture = new Texture2D(_pixWidth, _pixHeight);
        color = new Color[texture.width * texture.height];
        rendr.material.mainTexture = texture;

        CalculateNoise();
    }

    void CalculateNoise()
    {
        for (float y = 0f; y < texture.height; y++) {
            for (float x = 0f; x < texture.width; x++)
            {
                float xCoord = originPoint.x + x / texture.width * scale;
                float yCoord = originPoint.y + y / texture.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                color[(int)y * texture.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        texture.SetPixels(color);
        texture.Apply();
    }
}
