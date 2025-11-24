using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Text;

[Serializable]
public class EventOutcome
{
	public int gold;
	public int hp;
}

[Serializable]
public class EventReply
{
	public string narrative;
	public EventOutcome outcome;
}

public class ReplyGenerator : MonoBehaviour
{
	public static ReplyGenerator instance { get; private set; }
	private List<ChatMessage> chatHistory = new List<ChatMessage>();
	string eventDescription;
	private const string SystemPromptFileName = "ReplySystemPrompt.txt"; // same directory config file

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		instance = this;

		string systemPrompt = LoadSystemPrompt();
		if (string.IsNullOrEmpty(systemPrompt))
		{
			Debug.LogError($"Failed to load system prompt file '{SystemPromptFileName}'. ReplyGenerator will not function correctly.");
			return;
		}
		chatHistory.Add(new ChatMessage
		{
			role = "system",
			content = systemPrompt
		});
	}

	private string LoadSystemPrompt()
	{
		try
		{
			// Build path relative to Assets folder
			string path = Path.Combine(Application.dataPath, "Scripts", "EventScripts", SystemPromptFileName);
			if (!File.Exists(path))
			{
				Debug.LogError($"System prompt file not found at path: {path}");
				return null;
			}
			// Force UTF-8 decoding to avoid mojibake with Chinese characters
			return File.ReadAllText(path, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error reading system prompt file: {ex.Message}");
			return null;
		}
	}

	public void InitEvent(string ev)
	{
		eventDescription = ev;
	}

	public async Task<string> GenerateReply(string playerAction)
	{
		chatHistory.Add(new ChatMessage
		{
			role = "user",
			content = $"事件描述：{eventDescription}\n玩家行动：{playerAction}\n请根据系统提示词的要求生成回复。"
		});
		
		var reply = await LLM.instance.Chat(chatHistory.ToArray());
		
		if (string.IsNullOrEmpty(reply))
		{
			Debug.LogError("LLM returned null or empty reply");
			return null;
		}
		
		chatHistory.Add(new ChatMessage
		{
			role = "assistant",
			content = reply
		});
		
		EventReply eventReply = null;
		try
		{
			string cleanedReply = reply.Trim();
			eventReply = JsonUtility.FromJson<EventReply>(cleanedReply);
			
			if (eventReply == null)
			{
				Debug.LogError("Failed to parse EventReply: result is null");
				return null;
			}
			
		}
		catch (Exception ex)
		{
			Debug.LogError($"Failed to parse JSON reply: {ex.Message}\nReply content: {reply}");
			return null;
		}
		
		return eventReply.narrative;
	}
}
