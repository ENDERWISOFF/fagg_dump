using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    private void Awake()
    {
        items = new List<Item>();

        //1 add
        items.Add(new Potion(1, "������", Resources.Load<Sprite>("Icons/Apple"), 1, "������� ������", 0, 3, 80.0f));
        items.Add(new Weapon(2, "�����", Resources.Load<Sprite>("Icons/Axe"), 1, "������� �����.", 0, 3, 30.0f));
        items.Add(new Weapon(3, "����� ����", Resources.Load<Sprite>("Icons/Endurance_Potion"), 1, "�������������� ����", 0, 3, 10.0f));
        items.Add(new Weapon(4, "������", Resources.Load<Sprite>("Icons/Gold"), 99, "���� �����", 0, 1, 30.0f));
        items.Add(new Potion(5, "����� �����", Resources.Load<Sprite>("Icons/HP_Potion"), 1, "�������������� �����", 0, 10, 30.0f));
        items.Add(new Weapon(7, "���", Resources.Load<Sprite>("Icons/Sword_1"), 1, "������� ��������� ���", 0, 2, 30.0f));
        items.Add(new Weapon(8, "�����", Resources.Load<Sprite>("Icons/Wand"), 1, "���! ... �����..", 0, 1, 30.0f));
        
        //2 add
        items.Add(new Jewerely(9, "������� ������", Resources.Load<Sprite>("Icons/Acient_Coin"), 1, "�������� ������ ������������ �������������", 0, 100, 5.0f));
        items.Add(new Weapon(10, "������ �����", Resources.Load<Sprite>("Icons/Axe_Rusty"), 1, "����� ������ ������ �����", 0, 1, 30.0f));
        items.Add(new Weapon(11, "����� � �������", Resources.Load<Sprite>("Icons/Bomb"), 1, "����� ������� ������� ����", 0, 20, 15.0f));
        items.Add(new Item(12, "�����", Resources.Load<Sprite>("Icons/Bone"), 1, "������ ����� - �����",  0, 50.0f));
        items.Add(new Jewerely(13, "���������", Resources.Load<Sprite>("Icons/Diamond"), 1, "����������� ������", 0, 500, 1.0f));
        items.Add(new Weapon(14, "������", Resources.Load<Sprite>("Icons/Dugger"), 1, "������� ������", 0, 1, 30.0f));
        items.Add(new Weapon(15, "������ ������", Resources.Load<Sprite>("Icons/Dugger_Rusty"), 1, "������ ������ ��� ������� ����� ��������� �����", 0, 1, 30.0f));
        items.Add(new Potion(16, "������� ����", Resources.Load<Sprite>("Icons/MushRoom_1"), 1, "�������� �������", 0, 5, 30.0f));
        items.Add(new Potion(17, "�������� ����", Resources.Load<Sprite>("Icons/MushRoom_2"), 1, "�� ��� ���, �� �������� ��������", 0, 5, 30.0f));
        items.Add(new Weapon(18, "������ ���", Resources.Load<Sprite>("Icons/Sword_Rusty"), 1, "����� ������ ������ ���", 0, 2, 30.0f));

        //3 add
        items.Add(new Weapon(19, "�������� ���", Resources.Load<Sprite>("Icons/Steel_Sword"), 1, "�������� ��� ��������� ������� ��������", 0, 10, 15.0f));
        items.Add(new Weapon(20, "�������� ������", Resources.Load<Sprite>("Icons/Steekl_Dugger"), 1, "�������� ������ ��������� ������� ��������", 0, 6, 15.0f));
        items.Add(new Weapon(21, "�������� �����", Resources.Load<Sprite>("Icons/Steel_Axe"), 1, "�������� ����� ��������� ������� ��������", 0, 11, 15.0f));
        items.Add(new Potion(22, "���� �� �����", Resources.Load<Sprite>("Icons/FooD_1"), 1, "������ ������ ����", 0, 15, 15.0f));
        items.Add(new Weapon(23, "������� ���", Resources.Load<Sprite>("Icons/Gold_Sword"), 1, "������ �����", 0, 20, 1.0f));
        items.Add(new Weapon(24, "������� �����", Resources.Load<Sprite>("Icons/Gold_Axe"), 1, "������ �����", 0, 25, 1.0f));
        items.Add(new Weapon(25, "������� ������", Resources.Load<Sprite>("Icons/Gold-Dugger"), 1, "������ �����, �����.", 0, 15, 1.0f));
        items.Add(new Armor(26, "�������� �����", Resources.Load<Sprite>("Icons/Iron_Armor"), 1, "������ ������", 0, 15, 30.0f));
        items.Add(new Armor(27, "������ �����", Resources.Load<Sprite>("Icons/Rusty_Armor"), 1, "����� ������, ��� ����� ����������", 0, 5, 30.0f));
        items.Add(new Armor(28, "������� �����", Resources.Load<Sprite>("Icons/Gold_Armor"), 1, "����� ����� ������� ������ �����", 0, 30, 1.0f));

        //4 add

    }

    public List<Item> GetItems()
    {
        return items;
    }
}