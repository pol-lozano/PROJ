using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SaveData
{

    public Vector3 PlayerPos;
    public Quaternion PlayerRot;
    public DictionaryOfIntAndBool allPuzzles;


    //Screenshot. Time. 
    //SCENER
    

}

[Serializable]
public class SaveDataHolder
{

    public DictionaryOfStringAndSaveData saves = new DictionaryOfStringAndSaveData();
    public int currentNrOfSaves;

}





[Serializable] 
public class DictionaryOfStringAndSaveData : SerializableDictionary<string, SaveData> { }