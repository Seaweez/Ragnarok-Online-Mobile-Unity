using UnityEngine;
using System.Collections;
using RO;

public class TextureScale2 : Singleton<TextureScale2>
{
	private GameObject m_goTextureScale;
	private Camera m_camera;
	private UITexture m_uiTexture;
	private RO.ScreenShotHelper m_screenShotHelper;

	public void Get(Texture2D tex2D, float coefficient, System.Action<Texture2D> callback)
	{
		if (m_goTextureScale == null) {
			m_goTextureScale = GameObject.Find ("TextureScale");
			m_screenShotHelper = m_goTextureScale.GetComponent<RO.ScreenShotHelper> ();
			m_camera = m_goTextureScale.transform.Find ("Camera").GetComponent<Camera> ();
			m_camera.hideFlags = HideFlags.HideAndDontSave;
			m_camera.enabled = false;
			m_uiTexture = m_goTextureScale.transform.Find ("Texture").GetComponent<UITexture> ();
		}

		float texWidth = tex2D.width;
		float texHeight = tex2D.height;
		float newTexWidth = texWidth * coefficient;
		float newTexHeight = texHeight * coefficient;
		float ratio = newTexWidth / newTexHeight;
		m_camera.aspect = ratio;

		m_uiTexture.mainTexture = tex2D;
		m_uiTexture.height = Mathf.FloorToInt(1280.0f * m_camera.pixelHeight / m_camera.pixelWidth);
		m_uiTexture.width = Mathf.FloorToInt(m_uiTexture.height * ratio);

		m_screenShotHelper.Setting (newTexWidth, newTexHeight, TextureFormat.RGB24, 24, RO.ScreenShot.AntiAliasing.None);
		m_screenShotHelper.GetScreenShot ((x) => {
			m_uiTexture.mainTexture = null;

			if (callback != null) {
				callback (x);
			}
		}, new Camera[1]{ m_camera });
		return;
	}
}
