using System.IO;
using UnityEngine;

public class WriteDebugToFile : MonoBehaviour {
    string filename = "";

    void OnEnable() {
        Application.logMessageReceived += Log;
    }

    void OnDisable() {
        Application.logMessageReceived -= Log;
    }

    void Awake() {
        filename = Application.dataPath + "/LogFile.txt";
    }

    public void Log(string logString, string stackTrace, LogType type) {
        TextWriter tw = new StreamWriter(filename, true);

        // tw.WriteLine("[" + System.DateTime.Now + "] " + logString);
        tw.WriteLine(logString);
        tw.Close();
    }
}
