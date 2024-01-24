using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileHandle {

private string dataDirPath = "";
private string dataFileName="";

private string fullPath="";

public SaveFileHandle(string path, string name){
    dataDirPath=path;
    dataFileName=name;
    fullPath = Path.Combine(path,name);
}

public GameData Load(){
    GameData loadedData =null;

    if(File.Exists(fullPath)){
        try{
            string dataToLoad = "";
            using(FileStream stream = new FileStream(fullPath, FileMode.Open)){
                using (StreamReader reader = new StreamReader(stream)){
                    dataToLoad= reader.ReadToEnd();
                }
            }

            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        }catch(Exception e){
            Debug.LogError("Error Loading data from: "+fullPath+"\n"+e);
        }
    }

    return loadedData;
}

public void Save(GameData data){

    try{
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string dataToStore = JsonUtility.ToJson(data, true);

        using (FileStream stream = new FileStream (fullPath, FileMode.Create)){
            using (StreamWriter writer = new StreamWriter(stream)){
                
                writer.Write(dataToStore);
            }
        }
    }catch (Exception e){

        Debug.LogError("Error when trying to save data to: "+fullPath+ "\n"+ e);
    }

}

}
