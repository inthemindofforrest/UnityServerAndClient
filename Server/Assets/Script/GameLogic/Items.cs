using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public abstract class Items
    {
        protected Dictionary<InventoryList.LIST, Items> ItemList = new Dictionary<InventoryList.LIST, Items>();

        public string Name;
        public int Amount;
        public int CostPerUnit;

        public Items GetClassFromEnum(InventoryList.LIST _Item)
        {
            if (ItemList.ContainsKey(_Item))
            {
                return ItemList[_Item];
            }
            else
            {
                return null;
            }
        }

        public InventoryList.LIST GetEnumFromClass(Inventory.Items _Item)
        {
            return ItemList.FirstOrDefault(x => x.Value == _Item).Key;
        }

        protected abstract void AddItemToDictionary();
    }

    public class Empty : Items
    {
        protected override void AddItemToDictionary()
        {
            ItemList.Add(InventoryList.LIST.Empty, new Empty());
        }
    }

    public class Wood : Items
    {
        protected override void AddItemToDictionary()
        {
            ItemList.Add(InventoryList.LIST.Wood, new Wood());
        }
    }
}