﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }

            return _instance;
        }
    }

    [SerializeField]
    protected Material _islandMaterial;

    private int _ID;
    private string _userName;               //Not used as of now
    private string _islandFileName;
    private float _attackPower;             //Not used as of now
    private float _defensePower;            //Not used as of now
    private GameObject _island;

    private TerrainData data;               //Could be made into an object that is temporary

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _ID = PlayerPrefs.GetInt("ISLANDID");
        _islandFileName = "/island" + _ID;

        float[,] map = IslandSaveManager.LoadIsland(_islandFileName, _ID);
        data = new TerrainData {
            size = new Vector3(Const.size, Const.islandHeight, Const.size),
            heightmapResolution = Const.size - 1
            };
        data.SetHeights(0, 0, map);

        _island = Terrain.CreateTerrainGameObject(data);
        _island.transform.position = new Vector3((-Const.size * 10) / 2, -0.3f, (-Const.size * 10) / 2);
        _island.GetComponent<Terrain>().materialTemplate = _islandMaterial;
    }
  
}
