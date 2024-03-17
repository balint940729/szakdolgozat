using System;
using System.IO;
using UnityEngine;

public class FileDataHandler {
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load() {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath)) {
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) {
                Debug.LogError("Error when we want to load from file " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData gameData) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError("Error when we want to save to file " + fullPath + "\n" + e);
        }
    }
}