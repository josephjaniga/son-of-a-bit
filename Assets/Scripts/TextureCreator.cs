using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureCreator : MonoBehaviour {

	[Range(2, 1024)]
	public int resolution = 256;

	[Range(1, 3)]
	public int dimensions = 3;

	[Range(0, 9999)]
	public int seed = 0;

	private Texture2D texture;

	private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	}

	private void OnEnable(){
		if (texture == null) {
			texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
			texture.name = "Procedural Texture";
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;
			texture.anisoLevel = 9;
			GetComponent<MeshRenderer>().material.mainTexture = texture;
		}
		FillTexture();
	}

	public float frequency = 1f;

	public void FillTexture(){

		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

		// Vector3 point00 = new Vector3(-0.5f,-0.5f);
		// Vector3 point10 = new Vector3( 0.5f,-0.5f);
		// Vector3 point01 = new Vector3(-0.5f, 0.5f);
		// Vector3 point11 = new Vector3( 0.5f, 0.5f);
		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		NoiseMethod method = Noise.valueMethods[dimensions - 1];
		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				// texture.SetPixel(x, y, Color.white * Noise.SeededValue2D(point, frequency, seed));
				//texture.SetPixel(x, y, Color.white * Noise.HorizontalGradient1D(point, 3f, 0f));
				// if ( valueAtPoint(point, frequency) > 0.5f )
				// 	texture.SetPixel(x, y, Color.white);
				// else 
				// 	texture.SetPixel(x, y, Color.black);
				texture.SetPixel(x, y, Color.white * valueAtPoint(point, frequency));
			}
		}
		texture.Apply();
	}

	public float valueAtPoint(Vector3 point, float frequency){
		point *= frequency;
		float value = 0f;
		value = Noise.HorizontalGradient1D(point, 3f, 0f);
		// skipping noise
		value += Mathf.PerlinNoise(point.x, point.y);
		return value & 1;
	}

}
