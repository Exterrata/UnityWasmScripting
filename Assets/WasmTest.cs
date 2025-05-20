using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WasmTest : MonoBehaviour {
	public int width = 50;
	private Transform[][] _objects;

	private void Start() {
		_objects = new Transform[width][];
		for (int i = 0; i < width; i++) {
			_objects[i] = new Transform[width];
			for (int j = 0; j < width; j++) {
				GameObject obj = new();
				_objects[i][j] = obj.transform;
			}
		}

		Renderer renderer = transform.GetComponent("Renderer") as Renderer;

		Material[] sharedMaterials = renderer.sharedMaterials;
		Debug.Log("Shared Materials:");
		Debug.Log(sharedMaterials.Length);
		Debug.Log(sharedMaterials[0].ToString());
		renderer.sharedMaterials = sharedMaterials;

		string[] keywords = sharedMaterials[0].shaderKeywords;
		Debug.Log("Shader Keywords:");
		Debug.Log(keywords.Length);
		Debug.Log(keywords[0]);
		//sharedMaterials[0].shaderKeywords = keywords;

		RaycastHit[] hits = new RaycastHit[8];
		int count = Physics.SphereCastNonAlloc(new(0, 0, 0), 1, new(0, -1, 0), hits, 5, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);

		Debug.Log("Sphere Cast:");
		Debug.Log(count);
		for (int i = 0; i < count; i++) {
			Debug.Log(hits[i].collider.ToString());
		}
	}

	private void Update() {
		double time = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < width; j++) {
				double phase = (i + j) * 0.3;
				double y = Math.Sin(time + phase);
				_objects[i][j].position = new Vector3(i, (float)y, j);
			}
		}
	}
}