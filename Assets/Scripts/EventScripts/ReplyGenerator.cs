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
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private async void Start()
	{
		string systemPrompt = await FileManager.ReadConfigTextAsync(SystemPromptFileName);
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
