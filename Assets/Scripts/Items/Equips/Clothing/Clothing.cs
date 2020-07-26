using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment/Clothing", fileName = "Clothing.asset")]
public class Clothing : Equipment
{
    [SerializeField] ClothingType clothingType;
    GameObject clothingSlot;

    public ClothingType ClothingStyle { get => clothingType; }
    public override void Action()
    {
        UpdatePlayerData(); //Gets the updated information of the player.        

        clothingSlot = clothingType.GetEquipmentSlot();
        RemoveCurrentEquip(clothingSlot);

        GameObject newClothing = (GameObject)Instantiate(equippedPrefab, clothingSlot.transform);

        Debug.Log("Equipping clothing: " + itemName);
        player.UpdateEquip(clothingType.TypeName, id);

        inventory.RemoveItem(id);
    }

    protected override void RemoveCurrentEquip(GameObject clothingSlot)
    {
        //ItemID id = clothingSlot.GetComponentInChildren<ItemID>();

        /*
        if (id != null)
        {
            int ID = id.itemID;
            Debug.Log(ID);
            Destroy(id.gameObject);
            player.ui.RemoveEquip("Clothing");
            inventory.AddToInventory(ID);
        }*/
    }
}
