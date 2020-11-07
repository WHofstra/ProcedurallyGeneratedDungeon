using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseTexture : MonoBehaviour
{
    [SerializeField] float _noiseScale;

    public Texture2D Texture { get { return texture; } set { value = texture; } }

    Texture2D texture;
    Color[] color;
    Renderer rendr;

    Vector2 originPoint = new Vector2(0, 0);

    void Awake()
    {
        rendr = GetComponent<Renderer>();
        texture = new Texture2D(888, 500);

        color = new Color[texture.width * texture.height];
        rendr.material.mainTexture = texture;

        _noiseScale = CheckNoiseScale(_noiseScale);
        CalculateNoise();
    }

    void CalculateNoise()
    {
        for (float y = 0f; y < texture.height; y++) {
            for (float x = 0f; x < texture.width; x++)
            {
                float xCoord = originPoint.x + x / (texture.width  / _noiseScale) * transform.localScale.x;
                float yCoord = originPoint.y + y / (texture.height / _noiseScale) * transform.localScale.y;

                //This Adds Randomization
                int[] newNoise = { Random.Range(0, 1000), Random.Range(0, 1000) };

                float sample = Mathf.PerlinNoise(xCoord + (newNoise[0] / 10f), yCoord + (newNoise[1] / 10f));
                color[(int)y * texture.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        texture.SetPixels(color);
        texture.Apply();
    }

    float CheckNoiseScale(float aValue)
    {
        //Prevent a 0-value
        if (aValue <= 0) {
            return 0.1f;
        }
        return aValue;
    }
}
