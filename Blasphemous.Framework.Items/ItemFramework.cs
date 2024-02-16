using Blasphemous.ModdingAPI;
using Framework.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.Framework.Items;

/// <summary>
/// Handles adding custom items
/// </summary>
public class ItemFramework : BlasMod
{
    internal ItemFramework() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    /// <summary>
    /// Once all items are registered, count how many of each type
    /// </summary>
    protected override void OnAllInitialized()
    {
        foreach (var item in ItemRegister.Items.Where(i => i.AddInventorySlot))
            AddItemCount(item);
    }

    /// <summary>
    /// When a new game is started, grant all carry on start items
    /// </summary>
    protected override void OnNewGame()
    {
        foreach (var item in ItemRegister.Items.Where(i => i.CarryOnStart))
            item.GiveItem();
    }

    /// <summary>
    /// Increments the count for a type of item
    /// </summary>
    private void AddItemCount(ModItem item)
    {
        switch (item.ItemType)
        {
            case ModItem.ModItemType.RosaryBead:
                itemCountsByType[InventoryManager.ItemType.Bead] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Bead].y + 1);
                return;
            case ModItem.ModItemType.Prayer:
                itemCountsByType[InventoryManager.ItemType.Prayer] = new Vector2(17, itemCountsByType[InventoryManager.ItemType.Prayer].y + 1);
                return;
            case ModItem.ModItemType.Relic:
                itemCountsByType[InventoryManager.ItemType.Relic] = new Vector2(7, itemCountsByType[InventoryManager.ItemType.Relic].y + 1);
                return;
            case ModItem.ModItemType.SwordHeart:
                itemCountsByType[InventoryManager.ItemType.Sword] = new Vector2(11, itemCountsByType[InventoryManager.ItemType.Sword].y + 1);
                return;
            case ModItem.ModItemType.QuestItem:
                Main.ItemFramework.LogWarning("Can not add an inventory slot for quest items!");
                return;
            case ModItem.ModItemType.Collectible:
                itemCountsByType[InventoryManager.ItemType.Collectible] = new Vector2(44, itemCountsByType[InventoryManager.ItemType.Collectible].y + 1);
                return;
        }
    }

    /// <summary>
    /// Checks how many normal items and how many total items
    /// </summary>
    internal Vector2 GetItemCountOfType(InventoryManager.ItemType itemType)
    {
        return itemCountsByType.TryGetValue(itemType, out Vector2 amount) ? amount : new Vector2();
    }

    private readonly Dictionary<InventoryManager.ItemType, Vector2> itemCountsByType = new()
    {
        { InventoryManager.ItemType.Bead, new Vector2(44, 44) },
        { InventoryManager.ItemType.Prayer, new Vector2(17, 17) },
        { InventoryManager.ItemType.Relic, new Vector2(7, 7) },
        { InventoryManager.ItemType.Sword, new Vector2(11, 11) },
        { InventoryManager.ItemType.Collectible, new Vector2(44, 44) },
    };
}
