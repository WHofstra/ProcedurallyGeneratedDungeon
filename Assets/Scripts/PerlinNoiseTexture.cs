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
    Camera cam;

    Vector2 originPoint = new Vector2(0, 0);

    void Awake()
    {
        rendr = GetComponent<Renderer>();
        cam   = FindObjectOfType<Camera>();

        if (cam != null) {
            cam = cam.GetComponent<Camera>();
            texture = new Texture2D(cam.pixelWidth, cam.pixelHeight);
        }
        else {
            texture = new Texture2D(668, 376);
        }

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
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                color[(int)y * texture.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        texture.SetPixels(color);
        texture.Apply();
    }

    float CheckNoiseScale(float aValue)
    {
        if (aValue <= 0) {
            return 0.1f;
        }
        return aValue;
    }
}
