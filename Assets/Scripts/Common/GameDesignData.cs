using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameDesignData
{
    private static GameDesignData _instance;
    public static GameDesignData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameDesignData();
                ReadGameDesignData();
            }
            return _instance;
        }
        private set => _instance = value;
    }

    public List<Rakugo> RakugoList = new List<Rakugo>();
    public List<Level> LevelList = new List<Level>();

    private static void ReadGameDesignData()
    {
        var rakugoData = Resources.Load<TextAsset>("Config/RakugoData").text;
        _instance.RakugoList = JsonMapper.ToObject<List<Rakugo>>(rakugoData);

        var levelData = Resources.Load<TextAsset>("Config/LevelData").text;
        _instance.LevelList = JsonMapper.ToObject<List<Level>>(levelData);
    }
}
