using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items;

    private void Awake()
    {
        items = new List<Item>();

        //1 add
        items.Add(new Potion(1, "Яблоко", Resources.Load<Sprite>("Icons/Apple"), 1, "Обычное яблоко", 0, 3, 80.0f));
        items.Add(new Weapon(2, "Топор", Resources.Load<Sprite>("Icons/Axe"), 1, "Хороший топор.", 0, 3, 30.0f));
        items.Add(new Weapon(3, "Зелье силы", Resources.Load<Sprite>("Icons/Endurance_Potion"), 1, "Востанавливает силу", 0, 3, 10.0f));
        items.Add(new Weapon(4, "Золото", Resources.Load<Sprite>("Icons/Gold"), 99, "Гора монет", 0, 1, 30.0f));
        items.Add(new Potion(5, "Зелье жизни", Resources.Load<Sprite>("Icons/HP_Potion"), 1, "Востанавливает жизнь", 0, 10, 30.0f));
        items.Add(new Weapon(7, "Меч", Resources.Load<Sprite>("Icons/Sword_1"), 1, "Обычный железеный меч", 0, 2, 30.0f));
        items.Add(new Weapon(8, "Палка", Resources.Load<Sprite>("Icons/Wand"), 1, "Это! ... плака..", 0, 1, 30.0f));
        
        //2 add
        items.Add(new Jewerely(9, "Древняя монета", Resources.Load<Sprite>("Icons/Acient_Coin"), 1, "Странная монета неизвестного происхождения", 0, 100, 5.0f));
        items.Add(new Weapon(10, "Ржавый Топор", Resources.Load<Sprite>("Icons/Axe_Rusty"), 1, "Очень ветхий старый топор", 0, 1, 30.0f));
        items.Add(new Weapon(11, "Бомба с порохом", Resources.Load<Sprite>("Icons/Bomb"), 1, "Может нанести большой урон", 0, 20, 15.0f));
        items.Add(new Item(12, "Кость", Resources.Load<Sprite>("Icons/Bone"), 1, "Просто кость - мусор",  0, 50.0f));
        items.Add(new Jewerely(13, "Бриллиант", Resources.Load<Sprite>("Icons/Diamond"), 1, "Драгоценный камень", 0, 500, 1.0f));
        items.Add(new Weapon(14, "Кинжал", Resources.Load<Sprite>("Icons/Dugger"), 1, "Обычный кинжал", 0, 1, 30.0f));
        items.Add(new Weapon(15, "Ржавый кинжал", Resources.Load<Sprite>("Icons/Dugger_Rusty"), 1, "Только глупец или безумец будет сражаться таким", 0, 1, 30.0f));
        items.Add(new Potion(16, "Красный гриб", Resources.Load<Sprite>("Icons/MushRoom_1"), 1, "Возможно мухомор", 0, 5, 30.0f));
        items.Add(new Potion(17, "Зеленный гриб", Resources.Load<Sprite>("Icons/MushRoom_2"), 1, "ХЗ что это, но наверное съедобно", 0, 5, 30.0f));
        items.Add(new Weapon(18, "Ржавый меч", Resources.Load<Sprite>("Icons/Sword_Rusty"), 1, "Очень ветхий старый меч", 0, 2, 30.0f));

        //3 add
        items.Add(new Weapon(19, "Стальной меч", Resources.Load<Sprite>("Icons/Steel_Sword"), 1, "Отличный меч сделанный хорошим матсером", 0, 10, 15.0f));
        items.Add(new Weapon(20, "Стальной кинжал", Resources.Load<Sprite>("Icons/Steekl_Dugger"), 1, "Отличный кинжал сделанный хорошим матсером", 0, 6, 15.0f));
        items.Add(new Weapon(21, "Стальной топор", Resources.Load<Sprite>("Icons/Steel_Axe"), 1, "Отличный топор сделанный хорошим матсером", 0, 11, 15.0f));
        items.Add(new Potion(22, "Мясо на кости", Resources.Load<Sprite>("Icons/FooD_1"), 1, "Жирное сочное мясо", 0, 15, 15.0f));
        items.Add(new Weapon(23, "Золотой меч", Resources.Load<Sprite>("Icons/Gold_Sword"), 1, "Оружие богов", 0, 20, 1.0f));
        items.Add(new Weapon(24, "Золотой топор", Resources.Load<Sprite>("Icons/Gold_Axe"), 1, "Оружие богов", 0, 25, 1.0f));
        items.Add(new Weapon(25, "Золотой кинжал", Resources.Load<Sprite>("Icons/Gold-Dugger"), 1, "Оружие богов, почти.", 0, 15, 1.0f));
        items.Add(new Armor(26, "Железная броня", Resources.Load<Sprite>("Icons/Iron_Armor"), 1, "Крпкая защита", 0, 15, 30.0f));
        items.Add(new Armor(27, "Ржавая Броня", Resources.Load<Sprite>("Icons/Rusty_Armor"), 1, "Такой хрупки, что скоро рассыпется", 0, 5, 30.0f));
        items.Add(new Armor(28, "Золотая броня", Resources.Load<Sprite>("Icons/Gold_Armor"), 1, "Такой блеск ослепит любого врага", 0, 30, 1.0f));

        //4 add

    }

    public List<Item> GetItems()
    {
        return items;
    }
}