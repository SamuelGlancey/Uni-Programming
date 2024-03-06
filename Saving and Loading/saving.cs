using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class saving : MonoBehaviour

{

    DataScript data;


    void Awake()

    {

        Debug.Log(Application.persistentDataPath);


        saveFileCheck();

    }

    public void saveFileCheck()

    {

        if (!System.IO.File.Exists(Application.persistentDataPath + "/dataFile.json"))

        {

            createBlankFile();

            loadFromData();

            save();

        }

        else

        {

            loadFromFile();

        }

    }

    public void loadFromData()

    {

        //game variables = stored data


    }

    public void createBlankFile()

    {

        //instantiate the script then load default data

        data = new DataScript();

        save();

    }

    public void save()

    {

        //current data class = game variables


        //stored data class = current data class

        string jsonObject = JsonUtility.ToJson(data, true);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/dataFile.json", jsonObject);

    }


    public void loadFromFile()

    {

        //current data class = stored data class

        string jsonObject = System.IO.File.ReadAllText(Application.persistentDataPath + "/dataFile.json");

        data = JsonUtility.FromJson<DataScript>(jsonObject);


        loadFromData();    

    }

}