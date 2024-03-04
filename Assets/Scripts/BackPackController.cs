using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;


public class BackPackController : MonoBehaviour
{
    [SerializeField] private BackPackItem _packItemPref;
    [SerializeField] private GameObject _phoneEquip;
    [SerializeField] private GameObject _headPhonesEquip;
    [SerializeField] private GameObject _LaptopEquip;
    [SerializeField] private GameObject _powerbankEquip;
    private MainControlScript _mainControlScript;
    private List<ItemInfo> itemInfos = new List<ItemInfo>();
    private List<BackPackItem> _packItems = new List<BackPackItem>();
    private void Start()
    {
        _mainControlScript = GetComponent<MainControlScript>();
    }
    public void LoadBackPack()
    {
        foreach (var item in _packItems)
        {
            Destroy(item.gameObject);
        }
        _packItems.Clear();
        List<BigInteger> items = _mainControlScript.GetInventoryItems().ToList();
        itemInfos = _mainControlScript.GetItems();
        RemoveItems(itemInfos, items);
        for (int i = 0; i < itemInfos.Count; i++)
        {
            _packItems.Add(Instantiate(_packItemPref, _packItemPref.transform.parent));
            _packItems[i].ShowElement(itemInfos[i]);
        }
        LoadInventory();
    }    
    static void RemoveItems(List<ItemInfo> itemList, List<BigInteger> idList)
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            ItemInfo item = itemList[i];
            if (idList.Contains(item.itemID))
            {
                if (item.count > 1)
                {
                    // Если значение больше 1, уменьшите его на 1
                    item.count -= 1;
                }
                else
                {
                    // Если значение равно 1, удалите элемент из списка
                    itemList.RemoveAt(i);
                }
            }
        }
    }

    public void LoadInventory()
    {
        BigInteger[] items = _mainControlScript.GetInventoryItems();
        if (items[1] == 0) { _headPhonesEquip.SetActive(false); }
        else { _headPhonesEquip.SetActive(true); _headPhonesEquip.GetComponentInChildren<TextMeshProUGUI>().text = GetLvl(items[1]).ToString(); }
        if (items[3] == 0) { _LaptopEquip.SetActive(false); }
        else { _LaptopEquip.SetActive(true); _LaptopEquip.GetComponentInChildren<TextMeshProUGUI>().text = GetLvl(items[3]).ToString(); }
        if (items[0] == 0) { _phoneEquip.SetActive(false); }
        else { _phoneEquip.SetActive(true); _phoneEquip.GetComponentInChildren<TextMeshProUGUI>().text = GetLvl(items[0]).ToString(); }
        if (items[2] == 0) { _powerbankEquip.SetActive(false); }
        else { _powerbankEquip.SetActive(true); _powerbankEquip.GetComponentInChildren<TextMeshProUGUI>().text = GetLvl(items[2]).ToString(); }
    }

    private int GetLvl(BigInteger item)
    {
        string str= item.ToString("X");
        string lvl = str.Substring(48, 16);
        int level = Int32.Parse(lvl);
        return level;
    }

}
