using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStack;
    [SerializeField] private string type;

    [SerializeField] private int count;

    [SerializeField] private string description;

    [SerializeField] private float dropChance;

    public Item(int id, string name, Sprite icon, int maxStack, string description, int count, float dropChance)
    {
        this.name = name;
        this.id = id;
        this.maxStack = maxStack;
        this.description = description;
        this.icon = icon;
        this.count = count;
        this.dropChance = dropChance;
    }

    public string Name => name;
    public int Id => id;
    public Sprite Icon => icon;
    public int MaxStack => maxStack;
    public string Description => description;

    public float DropChance => dropChance;  

    // Свойство для чтения и записи Count
    public int Count
    {
        get => count;
        set => count = Mathf.Clamp(value, 0, maxStack);
    }

    public string Type => type;


    public virtual Item Clone()
    {
        return (Item)this.MemberwiseClone(); 
    }

    public virtual void Use()
    {
        Debug.Log($"Using item: {name}");
    }
}

[System.Serializable]
public class Weapon : Item
{
    [SerializeField] private int damage;

    public Weapon(int id, string name, Sprite icon, int maxStack, string description, int count, int damage, float dropChance)
        : base(id, name, icon, maxStack, description, count, dropChance)
    {
        this.damage = damage;
    }

    public int Damage => damage;

    public override void Use()
    {
        Debug.Log($"Attacking with {Name}, damage: {damage}");
    }

    public override Item Clone()
    {
        Weapon clonedWeapon = (Weapon)this.MemberwiseClone();
        return clonedWeapon;
    }
}

[System.Serializable]
public class Potion : Item
{
    [SerializeField] private int healing;

    public Potion(int id, string name, Sprite icon, int maxStack, string description, int count, int healing, float dropChance)
        : base(id, name, icon, maxStack, description, count, dropChance)
    {
        this.healing = healing;
    }

    public int Healing => healing;

    public override void Use()
    {
        Debug.Log($"Healing with {Name}, healing amount: {healing}");
    }

    public override Item Clone()
    {
        Potion clonedWeapon = (Potion)this.MemberwiseClone();
        return clonedWeapon;
    }
}

[System.Serializable]
public class Jewerely : Item
{
    [SerializeField] private int price;

    public Jewerely(int id, string name, Sprite icon, int maxStack, string description, int count, int price, float dropChance)
        : base(id, name, icon, maxStack, description, count, dropChance)
    {
        this.price = price;
    }

    public int Price => price;

    public override void Use()
    {
        Debug.Log($"Healing with {Name}, healing amount: {price}");
    }

    public override Item Clone()
    {
        Jewerely clonedWeapon = (Jewerely)this.MemberwiseClone();
        return clonedWeapon;
    }
}

[System.Serializable]
public class Armor : Item
{
    [SerializeField] private int defence;

    public Armor(int id, string name, Sprite icon, int maxStack, string description, int count, int defence, float dropChance)
        : base(id, name, icon, maxStack, description, count, dropChance)
    {
        this.defence = defence;
    }

    public int Defence => defence;

    public override void Use()
    {
        Debug.Log($"Healing with {Name}, healing amount: {defence}");
    }

    public override Item Clone()
    {
        Armor clonedWeapon = (Armor)this.MemberwiseClone();
        return clonedWeapon;
    }
}

[System.Serializable]
public class Ring : Item
{
    [SerializeField] private int baff;

    public Ring(int id, string name, Sprite icon, int maxStack, string description, int count, int baff, float dropChance)
        : base(id, name, icon, maxStack, description, count, dropChance)
    {
        this.baff = baff;
    }

    public int Baff => baff;

    public override void Use()
    {
        Debug.Log($"Healing with {Name}, healing amount: {baff}");
    }

    public override Item Clone()
    {
        Ring clonedWeapon = (Ring)this.MemberwiseClone();
        return clonedWeapon;
    }
}