using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using static UnityEditor.Progress;

public class ChestInteraction : MonoBehaviour
{
    public GameObject canvasPrefab;
    private GameObject canvasInstance;
    private GameObject player;

    private bool isPlayerNearby = false;

    private bool isOpened = false;

    public Sprite opened;
    private SpriteRenderer spriteRenderer;


    private TMP_Text chestText;
    private Button close;
    private Button take_item;

    private List<Item> items = new List<Item>();
    private ItemDatabase item_data;

    private Item toggle_item;

    private void Awake()
    {
        // Получаем ссылку на SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        item_data = FindObjectOfType<ItemDatabase>(); // Найти ItemDatabase в сцене
        GenerateRandomItems();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        close = canvasPrefab.GetComponent<Button>();
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            isOpened= true;
            Set_Chest_Open(isOpened);
            if (canvasInstance == null)
            {
                canvasInstance = Instantiate(canvasPrefab);
                ToggleChestUI();
            }
            else
            {
                Destroy(canvasInstance);
            }
        }
        

        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (canvasInstance != null)
            {
                Destroy(canvasInstance);
            }
        }
    }

    private void ToggleChestUI()
    {
        // Получаем TMP_Text из экземпляра канваса, а не из префаба
        chestText = canvasInstance.GetComponentInChildren<TMP_Text>();
        close = canvasInstance.transform.Find("Close_BUT").GetComponent<Button>();
        
        if (chestText != null) // Проверяем, что chestText не null
        {
            //Debug.Log(chestName);
            //chestText.text += $"\n{chestName}";
            Show_Item(items);
        }
        else
        {
            Debug.LogWarning("TMP_Text не найден в канвасе!");
        }

        if (close != null)
        {
            close.onClick.AddListener(CloseCanvas); // Добавляем обработчик нажатия на кнопку
        }

    }

        private void CloseCanvas()
        {
            if (canvasInstance != null)
            {
                Destroy(canvasInstance);
                canvasInstance = null; // Обнуляем ссылку на экземпляр канваса после его уничтожения
            }
        }

        private void Take_Item()
        {
        player.GetComponent<Player_Inventory>().Items.Add(toggle_item);

        }


    private void Show_Item(List<Item> items)
    {
        Transform itemContainer_1 = canvasInstance.transform.Find("Scroll_Area");
        Transform itemContainer_2 = itemContainer_1.transform.Find("Scroll View");
        Transform itemContainer_3 = itemContainer_2.transform.Find("Viewport");
        Transform itemContainer = itemContainer_3.transform.Find("Item_Container");

        take_item = canvasInstance.transform.Find("Take_BUT").GetComponent<Button>();

        if (itemContainer == null)
        {
            Debug.LogWarning("ItemContainer не найден в интерфейсе!");
            return;
        }

        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        Button last_button = null;

        foreach (var item in items)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>("Item_Con");
            GameObject buttonInstance = Instantiate(buttonPrefab, itemContainer);

            TMP_Text toggleLabel = buttonInstance.GetComponentInChildren<TMP_Text>();
            Button toggle = buttonInstance.GetComponent<Button>();
            Image icon = buttonInstance.GetComponentInChildren<Image>();

            Transform quantity = buttonInstance.transform.Find("Quantity");
            TMP_Text quan = quantity.GetComponent<TMP_Text>();
            
            if (toggleLabel != null && toggle != null && icon != null)
            {
                quan.text = item.Count.ToString();
                icon.sprite = item.Icon;
                toggleLabel.text = item.Name;


                toggle.onClick.AddListener(() =>
                {
                    // Деактивируем предыдущий переключатель
                    if (last_button != toggle)
                    {
                        ShowItemDetails(item, true);
                        
                        last_button = toggle;
                        toggle_item = item;
                        Debug.Log("Item_Chose");

                    }
                    else
                    {
                        ShowItemDetails(item, false);
                        last_button = null;
                        toggle_item = null;
                        Debug.Log("Item_Dechose");
                        EventSystem.current.SetSelectedGameObject(null);
                    }
            });
                
            }
            else
            {
                Debug.LogWarning("Toggle или TMP_Text не найдены в префабе!");
            }
        }
        take_item.onClick.AddListener(() =>
        {

            if (toggle_item != null)
            {
                
                Take_Item();
                
                items.Remove(toggle_item);

                toggle_item = null;
                Show_Item(items);

                Hide_Item_Details();

                

            }

        });
    }


    private void ShowItemDetails(Item item, bool see)
    {
        // Найти поле для отображения деталей предмета

        Transform itemDetails = canvasInstance.transform.Find("Details").transform.Find("ItemDetails");
        TMP_Text itemDetailsText = itemDetails.GetComponent<TMP_Text>();

        if (itemDetailsText != null)
        {
            if (see)
            {
                itemDetailsText.text = $"{item.Name}\n{item.Description}";
            }
            else
            {
                itemDetailsText.text = "";
            }
        }
        else
        {
            Debug.LogWarning("ItemDetails не найден!");
        }

    }

    private void GenerateRandomItems()
    {
        if (item_data != null)
        {
            var allItems = item_data.GetItems();
            int itemCountToGenerate = Random.Range(0, 3);

            List<Item> itemsToAdd = new List<Item>();

            for (int i = 0; i < itemCountToGenerate; i++)
            {
                float totalChance = 0;

                // Считаем общую вероятность всех предметов
                foreach (var item in allItems)
                {
                    totalChance += item.DropChance;
                }

                // Генерируем случайное число в диапазоне от 0 до общей вероятности
                float randomValue = Random.Range(0f, totalChance);
                float currentSum = 0;

                // Определяем, какой предмет выпал
                foreach (var item in allItems)
                {
                    currentSum += item.DropChance;
                    if (randomValue <= currentSum)
                    {
                        Item selectedItem = item.Clone();
                        selectedItem.Count = Random.Range(1, selectedItem.MaxStack + 1);
                        itemsToAdd.Add(selectedItem);
                        break;
                    }
                }
            }

            items = itemsToAdd;
        }
    }




    private void Set_Chest_Open(bool isOpen)
    {
        if (isOpen)
        {
            spriteRenderer.sprite = opened;


        }

    }

    private void Hide_Item_Details()
    {
        Transform itemDetails = canvasInstance.transform.Find("Details").transform.Find("ItemDetails");
        TMP_Text itemDetailsText = itemDetails.GetComponent<TMP_Text>();
        itemDetailsText.text = "";
    }

}
