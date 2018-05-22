﻿using System.Collections;
using UnityEngine;

public class BoardCreator2 : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor, playerSpawn, Enemi,
    }


    [SerializeField] public int columns = 100;
    [SerializeField] public int rows = 100;
    [SerializeField] private IntRange numRooms = new IntRange(15, 20);
    [SerializeField] private IntRange roomWidth = new IntRange(3, 10);
    [SerializeField] private IntRange roomHeight = new IntRange(3, 10);
    [SerializeField] private IntRange corridorLength = new IntRange(6, 10);
    [SerializeField] private GameObject[] floorTiles;
    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject player;

    public TileType[][] tiles;               
    public Vector2[][] TilesPosition;
    public Room[] rooms;                      
    private Corridor[] corridors;                  
    private GameObject FloorHolder;                  
    private GameObject WallHolder;
    private bool isPlayerSpawn = false;
    private PlayerControler Player;
    private Vector2Int PlayerSpawn;
    public Vector2Int[][] PatrolPoint;


    private void Awake()
    {
        FloorHolder = new GameObject("FloorHolder");
        WallHolder = new GameObject("WallHolder");
        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstancingWorld();
        SpawnPlayer();
    }
    


    void SetupTilesArray()
    {
        tiles = new TileType[columns][];
        TilesPosition = new Vector2[columns][];
        
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[rows];
            TilesPosition[i] = new Vector2[rows];

        }
    }

        void CreateRoomsAndCorridors()
        {
            rooms = new Room[numRooms.Random];
            PatrolPoint = new Vector2Int[rooms.Length][];
            for(int i = 0; i<rooms.Length; i++)
            {
            PatrolPoint[i] = new Vector2Int[4];
            }
            corridors = new Corridor[rooms.Length - 1];
        
            rooms[0] = new Room();
            corridors[0] = new Corridor();
        
            rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
        
            corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);
            for (int i = 1; i < rooms.Length; i++)
            {
                rooms[i] = new Room();

                rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);
            
                if (i < corridors.Length)
                {
                    corridors[i] = new Corridor();
                
                    corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
                }
            }

        }


        void SetTilesValuesForRooms()
        {
        bool IsEnemi = false;
            for (int i = 0; i < rooms.Length; i++)
            {
                Room currentRoom = rooms[i];
            
                for (int j = 0; j < currentRoom.roomWidth; j++)
                {
                    int xCoord = currentRoom.xPos + j;
                
                    for (int k = 0; k < currentRoom.roomHeight; k++)
                    {
                        int yCoord = currentRoom.yPos + k;
                    
                        tiles[xCoord][yCoord] = TileType.Floor;
                        if (!isPlayerSpawn)
                        {
                            tiles[xCoord][yCoord] = TileType.playerSpawn;
                            PlayerSpawn = new Vector2Int(xCoord, yCoord);
                            isPlayerSpawn = true;
                            IsEnemi = true;
                        }
                        else if(!IsEnemi)
                        {
                        tiles[xCoord][yCoord] = TileType.playerSpawn;
                        IsEnemi = true;
                        PatrolPoint[i][0] = new Vector2Int(xCoord, yCoord);
                        PatrolPoint[i][1] = new Vector2Int(xCoord + currentRoom.roomWidth, yCoord);
                        PatrolPoint[i][2] = new Vector2Int(xCoord + currentRoom.roomWidth, yCoord + currentRoom.roomHeight);
                        PatrolPoint[i][3] = new Vector2Int(xCoord, yCoord + currentRoom.roomHeight);
                    }

                    }
                }
            }
        }


        void SetTilesValuesForCorridors()
        {
            for (int i = 0; i < corridors.Length; i++)
            {
                Corridor currentCorridor = corridors[i];
            
                for (int j = 0; j < currentCorridor.corridorLength; j++)
                {
                    int xCoord = currentCorridor.startXPos;
                    int yCoord = currentCorridor.startYPos;
                
                    switch (currentCorridor.direction)
                    {
                        case Direction.North:
                            yCoord += j;
                            break;
                        case Direction.East:
                            xCoord += j;
                            break;
                        case Direction.South:
                            yCoord -= j;
                            break;
                        case Direction.West:
                            xCoord -= j;
                            break;
                    }

                    // Set the tile at these coordinates to Floor.
                    if(tiles[xCoord][yCoord] != TileType.playerSpawn)
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }

        private void InstancingWorld()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    int _x = i * 2 + 1 - columns;
                    int _y = j * 2 + 1 - rows;
                    TilesPosition[i][j] = new Vector2(_x, _y);

                    if (tiles[i][j] == TileType.Floor || tiles[i][j] == TileType.playerSpawn)
                    {
                        int randomIndex = Random.Range(0, floorTiles.Length);
                        GameObject tileInstance = Instantiate(floorTiles[randomIndex], new Vector3(_x, _y, 0f), Quaternion.identity) as GameObject;
                        tileInstance.transform.parent = FloorHolder.transform;
                    }

                    else if (tiles[i][j] == TileType.Wall)
                    {
                        int randomIndex = Random.Range(0, wallTiles.Length);
                        GameObject tileInstance = Instantiate(wallTiles[randomIndex], new Vector3(_x, _y, 0f), Quaternion.identity);
                        tileInstance.transform.parent = WallHolder.transform;
                    }
                }
            }
        }

        private void SpawnPlayer()
        {
            Vector3 playerPos = new Vector3(TilesPosition[PlayerSpawn.x][PlayerSpawn.y].x, TilesPosition[PlayerSpawn.x][PlayerSpawn.y].y, 0);
            Instantiate(player, playerPos, Quaternion.identity);
        }
}