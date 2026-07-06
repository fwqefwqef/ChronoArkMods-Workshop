using System;
using System.Collections.Generic;
using System.IO;
using GameDataEditor;
using UnityEngine;

namespace ExpertPlusMod // Code by surprise4u
{
	// Token: 0x02000002 RID: 2
	public class BanSave
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Init()
		{
			BanSave.SavePath = Application.persistentDataPath + "/ExpertPlusClear.txt";
			BanSave.ReadBanKeys();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public static void ReadBanKeys()
		{
			Debug.Log("Reading ExpertPlus Characters...");
			try
			{
				bool flag = File.Exists(BanSave.SavePath);
				if (flag)
				{
					using (StreamReader streamReader = new StreamReader(BanSave.SavePath))
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							bool flag2 = !string.IsNullOrEmpty(text);
							if (flag2)
							{
								BanSave.BanCharacterKeys.Add(text);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("ReadBanKeys Catch : " + ex.ToString());
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002120 File Offset: 0x00000320
		public static void WriteBanKeys()
		{
			Debug.Log("Writing ExpertPlus Characters...");
			try
			{
				bool flag = !File.Exists(BanSave.SavePath);
				if (flag)
				{
					File.Create(BanSave.SavePath);
				}
				using (StreamWriter streamWriter = new StreamWriter(BanSave.SavePath))
				{
					foreach (string value in BanSave.BanCharacterKeys)
					{
						bool flag2 = !string.IsNullOrEmpty(value);
						if (flag2)
						{
							streamWriter.WriteLine(value);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log("WriteBanKeys Catch : " + ex.ToString());
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002204 File Offset: 0x00000404
		public static List<GDECharacterData> FilterCharacterData(List<GDECharacterData> charDatas)
		{
			return charDatas.FindAll((GDECharacterData charData) => !BanSave.BanCharacterKeys.Contains(charData.Key));
		}

		// Token: 0x04000001 RID: 1
		public static HashSet<string> BanCharacterKeys = new HashSet<string>();

		// Token: 0x04000002 RID: 2
		public static string SavePath;
	}
}
