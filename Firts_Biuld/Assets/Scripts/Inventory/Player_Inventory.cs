using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    
    [SerializeField] Weapon wearpon_equiped_item;
    [SerializeField] Armor armor_equiped_item;
    [SerializeField] Ring ring_equiped_item;

    [SerializeField] private GameObject inventory_window;
    [SerializeField] private GameObject inventory_cell_prefab;
    [SerializeField] private int total_money = 0;

    List<Item> itemsToRemove = new List<Item>();

    private Item toggle_item;
    

    private bool isInventoryOpen = false;
    private bool isItemelected = false;

    public List<Item> Items => items;
    void Start()
    {
        
        inventory_window = GameObject.FindWithTag("Inv_Win");
        inventory_cell_prefab = Resources.Load<GameObject>("Item_Cell");
        if (inventory_window != null)
        {
            inventory_window.SetActive(false);
            
        }

       

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            foreach (var item in items)
            {

                if (item.Id == 4)
                {
                    total_money += item.Count;
                    itemsToRemove.Add(item);

                }

            }

            foreach (var item in itemsToRemove)
            {
                items.Remove(item);
            }
            
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        if (inventory_window != null)
        {
            isInventoryOpen = !isInventoryOpen;
            inventory_window.SetActive(isInventoryOpen);
        }
        if (isInventoryOpen)
        {
            ShowInvetortWindow();
        }
    }

    void ShowInvetortWindow()
    {
        Transform grid_parent = GameObject.Find("Content")?.transform;
        Image first_item_img = GameObject.FindGameObjectWithTag("1_item").GetComponent<Image>();

        Image sec_item_img = GameObject.FindGameObjectWithTag("2_item").GetComponent<Image>();

        Image third_item_img = GameObject.FindGameObjectWithTag("3_item").GetComponent<Image>();




        if (grid_parent == null || inventory_cell_prefab == null)
        {
            Debug.LogWarning("Grid parent or cell prefab is not assigned!");
            return;
        }

        foreach (Transform child in grid_parent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items)
        {
            Debug.LogWarning("Pass_1");
            GameObject cell = Instantiate(inventory_cell_prefab, grid_parent);
            Button toggle = cell.GetComponentInChildren<Button>();
            Image iconImage = cell.GetComponentInChildren<Image>();
            Transform text = inventory_window.transform.Find("Item_Info");
            Transform count_text_1 = text.transform.Find("Money");
            TMP_Text count_text = count_text_1.GetComponent<TMP_Text>();

            Button equip_button = GameObject.FindGameObjectWithTag("Equip").GetComponent<Button>();


            if (iconImage != null && item.Icon != null)
            {
                Debug.LogWarning("Pass_2");
                iconImage.sprite = item.Icon;
                count_text.text = total_money.ToString();
            }

            toggle.onClick.AddListener(() =>
            {
                ShowItemDetails(item, true);
            
                if (item is Weapon weapon)
                {
                    toggle_item = item;
                }
                if (item is Armor armor)
                {
                    toggle_item = item;
                }
                if (item is Ring ring)
                {
                    toggle_item = item;
                }
            });

            equip_button.onClick.AddListener(() =>
            {

                if (wearpon_equiped_item != null && toggle_item != null && toggle_item is Weapon weapon)
                    wearpon_equiped_item = (Weapon)toggle_item;
                    first_item_img.sprite = wearpon_equiped_item.Icon;

                if (armor_equiped_item != null && toggle_item != null && toggle_item is Armor armor)
                    armor_equiped_item = (Armor)toggle_item;
                    sec_item_img.sprite = armor_equiped_item.Icon;

                if (ring_equiped_item != null && toggle_item != null && toggle_item is Ring ring)
                    ring_equiped_item = (Ring)toggle_item;
                    third_item_img.sprite = ring_equiped_item.Icon;



            });

        }
    }


    private void ShowItemDetails(Item item, bool see)
    {

        TMP_Text itemDetails = inventory_window.transform.Find("Item_Info").transform.GetComponent<TMP_Text>();


        if (itemDetails != null)
        {
            if (see)
            {
                itemDetails.text = $"{item.Name}\n{item.Description}";
            }
            else
            {
                itemDetails.text = "";
            }
        }
        else
        {
            Debug.LogWarning("ItemDetails не найден!");
        }

    }

    private void Equip_Item()
    {

    }
}
