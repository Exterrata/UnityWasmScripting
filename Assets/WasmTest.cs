using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WasmTest : MonoBehaviour
{
    public int width = 50;
    private Transform[][] _objects;

    private void Start()
    {
        _objects = new Transform[width][];
        for (int i = 0; i < width; i++)
        {
            _objects[i] = new Transform[width];
            for (int j = 0; j < width; j++)
            {
                GameObject obj = new();
                _objects[i][j] = obj.transform;
            }
        }

        Renderer renderer = transform.GetComponent("Renderer") as Renderer;

        List<Material> materials = new();
        renderer.GetSharedMaterials(materials);

        Debug.Log(materials.Count);
        Debug.Log(materials[0].ToString());
    }

    private void Update()
    {
        double time = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                double phase = (i + j) * 0.3;
                double y = Math.Sin(time + phase);
                _objects[i][j].position = new Vector3(i, (float)y, j);
            }
        }
    }
}
