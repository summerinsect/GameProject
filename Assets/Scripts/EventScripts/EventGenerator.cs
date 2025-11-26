using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class ChatHistoryWrapper {
    public List<ChatMessage> messages;
}

public class EventGenerator : MonoBehaviour
{
    public static EventGenerator instance { get; private set; }
    public string currentEvent;
    private object lockObj = new object();
    private List<ChatMessage> chatHistory = new List<ChatMessage>();
    private const string SystemPromptFileName = "EventSystemPrompt.txt"; // config file in StreamingAssets
    private const string HistoryFileName = "EventHistory.json"; // persisted history file in persistentDataPath

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
            Debug.LogError($"Failed to load system prompt file '{SystemPromptFileName}'. EventGenerator will not function correctly.");
            return;
        }

        if (!LoadHistory(systemPrompt))
        {
            // Initialize fresh history with system prompt
            chatHistory.Clear();
            chatHistory.Add(new ChatMessage { role = "system", content = systemPrompt });
        }
        GenerateEvent();
    }

    private bool LoadHistory(string latestSystemPrompt)
    {
        try
        {
            string path = FileManager.GetSavePath(HistoryFileName);
            if (!File.Exists(path)) return false;
            string json = File.ReadAllText(path, Encoding.UTF8);
            ChatHistoryWrapper wrapper = JsonUtility.FromJson<ChatHistoryWrapper>(json);
            if (wrapper == null || wrapper.messages == null || wrapper.messages.Count == 0)
                return false;
            chatHistory = wrapper.messages;
            // Ensure first message is system prompt and refresh its content
            if (chatHistory[0].role == "system")
                chatHistory[0].content = latestSystemPrompt;
            else
                chatHistory.Insert(0, new ChatMessage { role = "system", content = latestSystemPrompt });
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Failed to load history: {ex.Message}");
            return false;
        }
    }

    private void SaveHistory()
    {
        try
        {
            string path = FileManager.GetSavePath(HistoryFileName);
            ChatHistoryWrapper wrapper = new ChatHistoryWrapper { messages = chatHistory };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(path, json, new UTF8Encoding(false));
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Failed to save history: {ex.Message}");
        }
    }
    
    private async void GenerateEvent()
    {
        chatHistory.Add(new ChatMessage
        {
            role = "user",
            content = "请按照系统提示词生成事件"
        });

        Debug.Log("History:\n" + string.Join("\n", chatHistory.ConvertAll(m => $"[{m.role}] {m.content}")));

        var ev = await LLM.instance.Chat(chatHistory.ToArray());
        
        if (!string.IsNullOrEmpty(ev))
        {
            chatHistory.Add(new ChatMessage
            {
                role = "assistant",
                content = ev
            });
            
            int maxMessages = 1 + (3 * 2);
            if (chatHistory.Count > maxMessages)
            {
                var systemMsg = chatHistory[0];
                var recentMsgs = chatHistory.GetRange(chatHistory.Count - (maxMessages - 1), maxMessages - 1);
                chatHistory.Clear();
                chatHistory.Add(systemMsg);
                chatHistory.AddRange(recentMsgs);
            }
        }
        
        lock (lockObj)
        {
            currentEvent = ev;
        }

        SaveHistory();
        Debug.Log("generation done");
    }
    
    public async Task<string> GetEvent()
    {
        while (true)
        {
            lock (lockObj)
            {
                if (!string.IsNullOrEmpty(currentEvent))
                {
                    string ev = currentEvent;
                    currentEvent = "";
                    Debug.Log("Event: " + ev);
                    GenerateEvent();
                    return ev;
                }
            }
            await Task.Delay(100);
        }
    }
}
