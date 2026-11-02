using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    readonly List<string> lines = new List<string>();
    const int MaxLines = 8;

    void OnEnable()  { Application.logMessageReceived += OnLog; }
    void OnDisable() { Application.logMessageReceived -= OnLog; }

    void Start()
    {
        lines.Add("DebugOverlay v1 running");
    }

    void OnLog(string msg, string stack, LogType type)
    {
        lines.Add(msg);
        if (lines.Count > MaxLines) lines.RemoveAt(0);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = Mathf.Max(18, Screen.height / 35);
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height / 2), string.Join("\n", lines), style);
    }
}