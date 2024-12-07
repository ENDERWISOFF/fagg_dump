using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Reflection;

public class Tile_Filler : MonoBehaviour
{
    public Tilemap Paths;
    public TileBase tile_1_0;
    public TileBase Dirt_0;
    public TileBase tileEmpty;
    

    private Vector3Int player_pos;
    private List<Vector3Int> chest_pos = new List<Vector3Int>();
    private List<Vector3Int> enemy_pos = new List<Vector3Int>();
    private List<Vector3Int> exit_pos = new List<Vector3Int>();

    [SerializeField] private GameObject player;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject exit;

    [SerializeField] GameObject Astra;

    private List<GameObject> enemies_list = new List<GameObject>();
    private List<GameObject> chests = new List<GameObject>();
    private List<GameObject> exits = new List<GameObject>();

    public char[,] grid;

    public int width = 50;  // Ширина карты
    public int height = 50; // Высота карты


    void Start()
    {
        
        GenerateTilemap();
        Astra.SetActive(true);
        StartCoroutine(ScanPath());
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 worldPosition = Paths.CellToWorld(player_pos) + new Vector3(0.8f, 0.8f, 0);
            player.transform.position = worldPosition; 
        }
    }

    void GenerateTilemap()
    {
        PlayerCharacter player_map = new PlayerCharacter(5, 5);
        MapClass map = new MapClass(width, height, 3, player_map);

        grid = map.Display();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                TileBase tile = null;

                switch (grid[x, y])
                {
                    case '#':
                        tile = tile_1_0;

                        break;

                    case ' ':
                        tile = tileEmpty;

                        // Добавляем ShadowCaster2D для клеток стен
                        AddShadowCasterToTile(x, y);

                        break;

                    case '@':
                        player_pos = new Vector3Int(x, y, 0);
                        tile = tile_1_0;
                        break;

                    case 'E':
                        enemy_pos.Add(new Vector3Int(x, y, 0));
                        tile = tile_1_0;
                        break;

                    case 'C':
                        chest_pos.Add(new Vector3Int(x, y, 0));
                        tile = tile_1_0;
                        break;
                    case 'X':
                        exit_pos.Add(new Vector3Int(x, y, 0));
                        tile = tile_1_0;
                        break;
                }

                Paths.SetTile(new Vector3Int(x, y, 0), tile);

                if (grid[x, y] == '@' || grid[x, y] == 'E')
                {
                    Paths.SetTile(new Vector3Int(x + 1, y, 0), tile_1_0);
                    Paths.SetTile(new Vector3Int(x - 1, y, 0), tile_1_0);
                    Paths.SetTile(new Vector3Int(x, y + 1, 0), tile_1_0);
                    Paths.SetTile(new Vector3Int(x, y - 1, 0), tile_1_0);
                }
            }
        }

        Vector3 worldPosition = Paths.CellToWorld(player_pos) + new Vector3(0.8f, 0.8f, 0);
        Instantiate(player, worldPosition, Quaternion.identity);

        // Создаем объекты врагов
        for (int i = 0; i < enemy_pos.Count; i++)
        {
            Vector3 enemyWorldPosition = Paths.CellToWorld(enemy_pos[i]) + new Vector3(0.5f, 0.5f, 0);
            GameObject randomEnemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];


            GameObject instantiatedEnemy = Instantiate(randomEnemy, enemyWorldPosition, Quaternion.identity);
            instantiatedEnemy.name = randomEnemy.name;
            enemies_list.Add(instantiatedEnemy);
        }

        // Создаем объекты сундуков
        for (int i = 0; i < chest_pos.Count; i++)
        {
            Vector3 chestWorldPosition = Paths.CellToWorld(chest_pos[i]) + new Vector3(0.5f, 0.5f, 0);
            GameObject instantiatedChest = Instantiate(chest, chestWorldPosition, Quaternion.identity);
            instantiatedChest.name = "Chest_" + i;
            chests.Add(instantiatedChest);
        }

        for (int i = 0; i < exit_pos.Count; i++)
        {
            Vector3 chestWorldPosition = Paths.CellToWorld(exit_pos[i]) + new Vector3(0.5f, 0.5f, 0);
            GameObject instantiatedChest = Instantiate(exit, chestWorldPosition, Quaternion.identity);
            instantiatedChest.name = "Exit_" + i;
            chests.Add(instantiatedChest);
        }
    }

    void AddShadowCasterToTile(int x, int y)
    {
        Vector3 worldPosition = Paths.CellToWorld(new Vector3Int(x, y, 0));
        GameObject shadowCaster = new GameObject($"ShadowCaster_{x}_{y}");
        shadowCaster.transform.position = worldPosition + new Vector3(1f, 1f, 0); // Центр 2x2 тайла
        shadowCaster.transform.SetParent(this.transform);

        ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
        shadowCasterComponent.selfShadows = false;

        // Определяем новую увеличенную форму
        Vector3[] shape = new Vector3[]
        {
        new Vector3(-1f, -1f, 0), // Левый нижний угол
        new Vector3( 1f, -1f, 0), // Правый нижний угол
        new Vector3( 1f,  1f, 0), // Правый верхний угол
        new Vector3(-1f,  1f, 0), // Левый верхний угол
        };

        // Используем рефлексию для изменения shapePath
        var fieldInfo = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(shadowCasterComponent, shape);

            // Обновляем полигоны для тени
            var methodInfo = typeof(ShadowCaster2D).GetMethod("OnValidate", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo?.Invoke(shadowCasterComponent, null);
        }
    }


    private IEnumerator ScanPath()
    {
        // Предварительно выполняем некоторые действия, если необходимо
        yield return new WaitForSeconds(0.1f); // Задержка для демонстрации (при необходимости)

        // Выполняем сканирование пути
        AstarPath.active.Scan();

        // В дополнение, возможно, вам нужно будет ожидать завершения. 
        // В A* могут быть события, которые уведомляют о завершении сканирования.
        // Однако на момент написания этого ответа AstarPath не использует coroutine для завершения сканирования.

        yield return null; // Ждем следующего кадра
    }




}


