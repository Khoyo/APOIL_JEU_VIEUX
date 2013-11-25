using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowSystem : MonoBehaviour
{
	
	
	// How dark a color has to be for it to count as a shadow
	[SerializeField]
	private float minLuminance = 0.01f;
	[SerializeField]
	private float shadowBias = 0.001f;
	[SerializeField]
	private Camera shadowCamera;
	[SerializeField]
	private bool highQuality = false;
	[SerializeField]
	private Shader lightDistanceShader;
	[SerializeField]
	private int shadowMapSize = 512;
	//
	private RenderTexture _texShadowTexture;
	private RenderTexture _texTarget;
	private Dictionary<Shader, Material> _shaderMap = new Dictionary<Shader, Material> ();
	private List<RenderTexture> _tempRenderTextures = new List<RenderTexture> ();

	private Material GetMaterial (Shader shader)
	{
		Material material;
		if (_shaderMap.TryGetValue (shader, out material)) {
			return material;
		} else {
			material = new Material (shader);
			_shaderMap.Add (shader, material);
			return material;
		}
	}

	private void OnEnable ()
	{
		_texShadowTexture = new RenderTexture (shadowMapSize, shadowMapSize, 16, RenderTextureFormat.Default);
		_texTarget = new RenderTexture (shadowMapSize, shadowMapSize, 0, RenderTextureFormat.Default);

		shadowCamera.targetTexture = _texShadowTexture;
		// hack to fix broken aspect
		shadowCamera.rect = new Rect (0, 0, 1, 1);

		// Match plane to orthographic size
		transform.localScale = Vector3.one * shadowCamera.orthographicSize / 5;
		renderer.material.mainTexture = _texTarget;

		shadowMapSize = Mathf.NextPowerOfTwo (shadowMapSize);
		shadowMapSize = Mathf.Clamp (shadowMapSize, 8, 2048);
	}

	private void OnDestroy ()
	{
		foreach (KeyValuePair<Shader, Material> item in _shaderMap) {
			Destroy (item.Value);
		}
		_shaderMap.Clear ();

		Destroy (_texTarget);
		Destroy (_texShadowTexture);

		ReleaseAllRenderTextures ();
	}

	private void OnWillRenderObject ()
	{
		shadowCamera.Render ();
		
		GetMaterial (lightDistanceShader).SetFloat ("_MinLuminance", minLuminance);
		GetMaterial (lightDistanceShader).SetFloat ("_ShadowOffset", shadowBias);
		
		RenderTexture texLightDistance = PushRenderTexture (_texShadowTexture.width, _texShadowTexture.height);
		Graphics.Blit (_texShadowTexture, texLightDistance, GetMaterial (lightDistanceShader), 0);

//		Graphics.Blit(texLightDistance, _texTarget);
//		ReleaseAllRenderTextures();
//		return;

		RenderTexture texStretched = PushRenderTexture (_texShadowTexture.width, _texShadowTexture.height);
		Graphics.Blit (texLightDistance, texStretched, GetMaterial (lightDistanceShader), 1);

// 		Graphics.Blit(texStretched, _texTarget);
//		ReleaseAllRenderTextures();
//		return;

		int width = texStretched.width;
		int height = texStretched.height;

		RenderTexture texDownSampled = texStretched;
		while (width > 2) {

			width /= 2;

			RenderTexture texDownSampleTemp = PushRenderTexture (width, height);
			Graphics.Blit (texDownSampled, texDownSampleTemp, GetMaterial (lightDistanceShader), 2);
			texDownSampled = texDownSampleTemp;
		}

//		Graphics.Blit(texDownSampled, _texTarget);
//		ReleaseAllRenderTextures();
//		return;

		Graphics.Blit (texDownSampled, _texTarget, GetMaterial (lightDistanceShader), highQuality ? 4 : 3);

		ReleaseAllRenderTextures ();
	}

	private RenderTexture PushRenderTexture (int width, int height, int depth = 0, RenderTextureFormat format = RenderTextureFormat.Default)
	{
		RenderTexture tex = RenderTexture.GetTemporary (width, height, depth, format);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		_tempRenderTextures.Add (tex);
		return tex;
	}

	private void ReleaseAllRenderTextures ()
	{
		foreach (var item in _tempRenderTextures) {
			RenderTexture.ReleaseTemporary (item);
		}
		_tempRenderTextures.Clear ();
	}
}
