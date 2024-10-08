﻿using Framework.Inventory;
using Framework.Managers;
using Gameplay.UI.Others.MenuLogic;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blasphemous.Framework.Items;

// Initialize any custom items
[HarmonyPatch(typeof(InventoryManager), nameof(InventoryManager.InitializeObjects))]
class InventoryInitialize_Patch
{
    public static void Postfix(InventoryManager __instance, GameObject ___mainObject,
        Dictionary<string, RosaryBead> ___allBeads,
        Dictionary<string, Relic> ___allRellics,
        Dictionary<string, Prayer> ___allPrayers,
        Dictionary<string, Sword> ___allSwords,
        Dictionary<string, QuestItem> ___allQuestItems,
        Dictionary<string, global::Framework.Inventory.CollectibleItem> ___allCollectibleItems)
    {
        foreach (ModItem item in ItemRegister.Items)
        {
            if (item is ModRosaryBead bead)
            {
                RosaryBead Bead = bead.CreateRosaryBead(___mainObject);
                ___allBeads.Add(bead.Id, Bead);
            }
            else if (item is ModRelic relic)
            {
                Relic Relic = relic.CreateRelic(___mainObject);
                ___allRellics.Add(relic.Id, Relic);
            }
            else if (item is ModPrayer prayer)
            {
                Prayer Prayer = prayer.CreatePrayer(___mainObject);
                ___allPrayers.Add(prayer.Id, Prayer);
            }
            else if (item is ModSwordHeart swordHeart)
            {
                Sword SwordHeart = swordHeart.CreateSwordHeart(___mainObject);
                ___allSwords.Add(swordHeart.Id, SwordHeart);
            }
            else if (item is ModQuestItem questItem)
            {
                QuestItem QuestItem = questItem.CreateQuestItem(___mainObject);
                ___allQuestItems.Add(questItem.Id, QuestItem);
            }
            else if (item is ModCollectible collectible)
            {
                global::Framework.Inventory.CollectibleItem Collectible = collectible.CreateCollectible(___mainObject);
                ___allCollectibleItems.Add(collectible.Id, Collectible);
            }
        }
    }
}

// Add extra slots to inventory tabs based on how many custom items
[HarmonyPatch(typeof(NewInventory_LayoutGrid), nameof(NewInventory_LayoutGrid.ShowMaxSlotsForCurrentTabType))]
class InventoryLayout_Patch
{
    public static void Postfix(List<NewInventory_GridItem> ___cachedGridElements, InventoryManager.ItemType ___currentItemType)
    {
        Vector2 itemCounts = Main.ItemFramework.GetItemCountOfType(___currentItemType);
        if (itemCounts.x == 0)
            return;

        for (int i = (int)itemCounts.x; i < (int)itemCounts.y && i < 72; i++)
        {
            ___cachedGridElements[i].gameObject.SetActive(true);
        }
    }
}

// Fix navigation errors on inventory screen
[HarmonyPatch(typeof(NewInventory_LayoutGrid), nameof(NewInventory_LayoutGrid.LinkLastSlotToLastRowFirstSlot))]
class InventoryLayoutNav_Patch
{
    public static bool Prefix(List<NewInventory_GridItem> ___cachedGridElements, InventoryManager.ItemType ___currentItemType)
    {
        int totalSlots = (int)Main.ItemFramework.GetItemCountOfType(___currentItemType).y;
        if (totalSlots == 0)
            return true;

        int firstIdx = 8 * (totalSlots / 8);
        int lastIdx = totalSlots - 1;
        if (firstIdx >= totalSlots) firstIdx -= 8;

        Navigation firstNav = ___cachedGridElements[firstIdx].Button.navigation;
        firstNav.selectOnLeft = ___cachedGridElements[lastIdx].Button.interactable ? ___cachedGridElements[lastIdx].Button : null;
        ___cachedGridElements[firstIdx].Button.navigation = firstNav;
        Navigation lastNav = ___cachedGridElements[lastIdx].Button.navigation;
        lastNav.selectOnRight = ___cachedGridElements[firstIdx].Button.interactable ? ___cachedGridElements[firstIdx].Button : null;
        ___cachedGridElements[lastIdx].Button.navigation = lastNav;

        return false;
    }
}

// Prevent incorrect errors for object effect types
[HarmonyPatch(typeof(ObjectEffect), nameof(ObjectEffect.ShowError))]
class ObjectEffectError_Patch
{
    public static bool Prefix() => false;
}
