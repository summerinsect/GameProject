using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;

public static class FileManager
{
	// 获取配置文件路径（只读）
	public static string GetConfigPath(string fileName)
	{
		string path;

#if UNITY_EDITOR
		path = Path.Combine(Application.dataPath, "StreamingAssets", fileName);
#elif UNITY_STANDALONE
        path = Path.Combine(Application.dataPath, "StreamingAssets", fileName);
#elif UNITY_ANDROID
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_IOS
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#else
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

		return path;
	}

	// 获取存档文件路径（可读写）
	public static string GetSavePath(string fileName)
	{
		// 使用 persistentDataPath，这个路径在所有平台都是可写的
		string directory = Application.persistentDataPath;

		// 确保目录存在
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		return Path.Combine(directory, fileName);
	}

	// 读取配置文件内容（处理 StreamingAssets 在不同平台的差异）
	public static async Task<string> ReadConfigTextAsync(string fileName)
	{
		string path = GetConfigPath(fileName);

		// On some platforms (Android) streamingAssetsPath is inside the apk and must be read via UnityWebRequest
		if (path.Contains("://") || path.StartsWith("jar:"))
		{
			using (UnityWebRequest uwr = UnityWebRequest.Get(path))
			{
				var op = uwr.SendWebRequest();
				while (!op.isDone)
					await Task.Yield();

				#if UNITY_2020_1_OR_NEWER
				if (uwr.result != UnityWebRequest.Result.Success)
				#else
				if (uwr.isNetworkError || uwr.isHttpError)
				#endif
				{
					Debug.LogError($"Failed to read config from {path}: {uwr.error}");
					return null;
				}

				return uwr.downloadHandler.text;
			}
		}
		else
		{
			if (!File.Exists(path))
			{
				Debug.LogError($"Config file not found at path: {path}");
				return null;
			}
			return File.ReadAllText(path, Encoding.UTF8);
		}
	}

	// 读取存档文件（持久化，可写），同步
	public static string ReadSaveText(string fileName)
	{
		string path = GetSavePath(fileName);
		if (!File.Exists(path)) return null;
		return File.ReadAllText(path, Encoding.UTF8);
	}

	// 写入存档文件（持久化），同步
	public static void WriteSaveText(string fileName, string content)
	{
		string path = GetSavePath(fileName);
		File.WriteAllText(path, content, new UTF8Encoding(false));
	}
}