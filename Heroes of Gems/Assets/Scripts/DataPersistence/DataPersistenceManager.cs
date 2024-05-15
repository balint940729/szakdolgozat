using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour {

    //Singleton class
    public static DataPersistenceManager instance { get; private set; }

    [Header("File Storage Container")]
    [SerializeField] private string fileName = default;

    private static List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
    private static GameData gameData;

    private FileDataHandler dataHandler;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        if (MainMenuScript.isNewGame) {
            NewGame();
        }
        else {
            LoadGame();
        }
    }

    public void NewGame() {
        gameData = new GameData();
    }

    public void SaveGame() {
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects) {
            dataPersistence.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void LoadGame() {
        gameData = dataHandler.Load();
        if (gameData == null) {
            NewGame();
        }

        foreach (IDataPersistence dataPersistence in dataPersistenceObjects) {
            dataPersistence.LoadData(gameData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    public static void AddDataPersistence(IDataPersistence dataPersistence) {
        dataPersistenceObjects.Add(dataPersistence);
    }
}