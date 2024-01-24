using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManagerTheDudeMakingThisTutorialIsANerd : MonoBehaviour
{

    [SerializeField] private string fileName;
    // Start is called before the first frame update
    public static DataPersistenceManagerTheDudeMakingThisTutorialIsANerd instance {get; private set;}


    void Start(){
        //dataPersObjs=new List<IDataPers>();
        saveHandler = new SaveFileHandle(Application.persistentDataPath, fileName);
        dataPersObjs= findDataPers();
        LoadGame();
        
    }
    GameData gameData;

    private List<IDataPers> dataPersObjs;

    SaveFileHandle saveHandler;
    private void Awake(){
        if(instance!= null){
            Debug.LogError("Multiple save managers in scene!");
        }
        instance=this;
    }


    public void NewGame(){
        gameData=new GameData();
    }

    public void LoadGame(){
        gameData = saveHandler.Load();

        if(this.gameData==null){
            Debug.Log("No Save Found! Initializing to defaults...");
            NewGame();
        }
        foreach (IDataPers dataPersObj in dataPersObjs){
            dataPersObj.LoadData(gameData);
        }
        Debug.Log("Loaded Level: "+gameData.currentLevel);

        
    }

    public void SaveGame(){
         foreach (IDataPers dataPersObj in dataPersObjs){
            dataPersObj.SaveData(ref gameData);
        }
        Debug.Log("Saved Level: "+gameData.currentLevel);
        saveHandler.Save(gameData);
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPers> findDataPers(){
        IEnumerable<IDataPers> dataPersObjs=FindObjectsOfType<MonoBehaviour>().OfType<IDataPers>();

        return new List<IDataPers>(dataPersObjs);
    }
}
