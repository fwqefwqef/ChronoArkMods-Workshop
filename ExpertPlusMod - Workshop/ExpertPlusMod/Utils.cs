using System;
using ChronoArkMod;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExpertPlusMod // Code by surprise4u
{
	// Token: 0x0200000E RID: 14
	public class Utils
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00005778 File Offset: 0x00003978
		public static void getSprite(string path, Image img)
		{
			string path2 = ModManager.getModInfo("ExpertPlusMod").assetInfo.ImageFromFile(path);
			AddressableLoadManager.LoadAsyncAction(path2, AddressableLoadManager.ManageType.None, img);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000057A8 File Offset: 0x000039A8
		public static Sprite getSprite(string path)
		{
			string path2 = ModManager.getModInfo("ExpertPlusMod").assetInfo.ImageFromFile(path);
			return AddressableLoadManager.LoadAsyncCompletion<Sprite>(path2, AddressableLoadManager.ManageType.None);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000057D8 File Offset: 0x000039D8
		public static GameObject creatGameObject(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.SetActive(false);
			gameObject.transform.SetParent(parent, false);
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = 8;
			return gameObject;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005824 File Offset: 0x00003A24
		public static string getTranslation(string key)
		{
			try
			{
				return ModManager.getModInfo("RandomCharacter").localizationInfo.SyetemLocalizationUpdate("RandomCharacter/UI/" + key);
			}
			catch
			{
			}
			return key;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005870 File Offset: 0x00003A70
		public static GameObject GetChildByName(GameObject obj, string name)
		{
			Transform transform = obj.transform.Find(name);
			bool flag = transform != null;
			GameObject result;
			if (flag)
			{
				result = transform.gameObject;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000058A8 File Offset: 0x00003AA8
		public static void ImageResize(Image img, Vector2 size, Vector2 pos)
		{
			img.rectTransform.anchorMin = new Vector2(0f, 1f);
			img.rectTransform.anchorMax = new Vector2(0f, 1f);
			img.rectTransform.sizeDelta = size;
			img.rectTransform.transform.localPosition = pos;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005910 File Offset: 0x00003B10
		public static void TextResize(TextMeshProUGUI txt, Vector2 size, Vector2 pos, string text, float fontSize)
		{
			txt.rectTransform.anchorMin = new Vector2(0f, 1f);
			txt.rectTransform.anchorMax = new Vector2(0f, 1f);
			txt.rectTransform.sizeDelta = size;
			txt.rectTransform.transform.localPosition = pos;
			txt.text = text;
			txt.fontSize = fontSize;
			txt.color = Color.white;
			txt.alignment = TextAlignmentOptions.Left;
		}
	}
}
