using System.Collections.Generic;

namespace Assets
{
    public class ShipInventory
    {
        public List<ShipLoot> items;
        public float MaxWeight;

        public void Additem(ShipLoot shipLoot)
        {
            if (GetCurrentWeight() <= MaxWeight)
            {
                items.Add(shipLoot);
            }



        }
        public void DelItem()
        {


        }
        public float GetCurrentWeight()
        {
            float result = 0;
            foreach (var item in items)
            {
                result += item.ItemWeight;
            }
            return result;
        }
        public void ShipTransfer(ShipInventory Targetinventory, ShipLoot shipLoot)
        {
            Targetinventory.items.Add(shipLoot);
            items.Remove(shipLoot);

        }

        public void ShipTransfer(ShipInventory targetinventory, ShipLoot item, int count)
        {
            // Проверили сколько мы максимум можем предметов этого типа перетащить (Условно у нас 10 арбузов
            int itemCountInInventory = GetItemCountByName(item);


            // Если мы хотим перекинуть 15 арбузов, а у нас их всего 10, то ничего не делаем
            if(itemCountInInventory < count) 
            {
                return;
            }
            // А если получилось так, что кол-во адекватное, то мы столько раз вызываем метод с 34 строки
            for (int i = 0; i < count; i++) 
            {
                ShipTransfer(targetinventory, item);
            }
        }

        public int GetItemCountByName(ShipLoot shipLoot)
        {
            int result = 0;
            foreach (var item in items)
            {
                if (item.Name == shipLoot.Name)
                {
                    result++;
                }
            }
            return result;
        }
        
    }
}