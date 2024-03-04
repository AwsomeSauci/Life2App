using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class BackPackItem : MonoBehaviour
{
    [SerializeField] private Color orange;
    [SerializeField] private Color green;
    [SerializeField] private TextMeshProUGUI _countTxt;
    [SerializeField] private TextMeshProUGUI _levelTxt;
    [SerializeField] private Image _image;
    private MainControlScript _mainController;
    private ImagesFolder _folder;
    private BigInteger _id;
    private int _count;
    private const string EQUIPMENT_TYPE = "4A78BC8049ECDA3D";
    private const string EQUIPMENT_PART_TYPE = "62EB12BD8F7E363E";
    private const string LOOTBOX_TYPE = "D9FAE74DDFF89E31";
    private const string PHONE_SUBTYPE = "B5816CFC55FB3CB5";
    private const string EARBUDS_SUBTYPE = "7C1FF20149A3FEF4";
    private const string POWERBANK_SUBTYPE = "64E53703AB552E2E";
    private const string LAPTOP_SUBTYPE = "57A3F6CA210E2B2A";
    private const string REGULAR_LOOTBOX_SUBTYPE = "6E477AD27FB26738";
    private const string PREMIUM_LOOTBOX_SUBTYPE = "EDCC438796E76959";
    private bool isLootBox=false;
    string type;
    string subtype;
    string lvl;
    public void ShowElement(ItemInfo itemInfo)
    {
        _mainController = FindAnyObjectByType<MainControlScript>();
        _folder = FindFirstObjectByType<ImagesFolder>();
        _count = (Int32)itemInfo.count;
        _id = itemInfo.itemID;
        string hexID = ConvertToHex(_id);
        if (hexID[0] == '0') hexID= hexID.Substring(1);
        type = hexID.Substring(0, 16);
        subtype = hexID.Substring(16, 16);
        lvl = hexID.Substring(48, 16);
        int level = Int32.Parse(lvl);
        SetInfo(type, subtype, level);
        gameObject.SetActive(true);
    }
    public void OnObjectClick()
    {
        if (isLootBox)
        {
            switch(subtype)
            {
                case REGULAR_LOOTBOX_SUBTYPE:
                    _mainController.ShowBoxOpenScreen();
                    break;
            }
        }
       
    }

    private void SetInfo(string type, string subtype, int lvl)
    {
        switch (type)
        {
            case EQUIPMENT_TYPE:
                {
                    SetEquipInfo(subtype, lvl);
                    break;
                }
            case EQUIPMENT_PART_TYPE:
                {
                    SetEquipPartInfo(subtype);
                    break;
                }
            case LOOTBOX_TYPE:
                {
                    isLootBox=true;
                    SetLootBoxInfo(subtype);
                    break;
                }

        }
    }
    private void SetEquipInfo(string subtupe, int lvl)
    {
        _levelTxt.transform.parent.gameObject.SetActive(true);
        if(lvl <1) lvl = 1;
        _levelTxt.text = lvl.ToString();
        switch (subtupe)
        {
            case PHONE_SUBTYPE:
                {
                    _image.sprite = _folder.SmartPhone;
                    break;
                }
            case EARBUDS_SUBTYPE:
                {
                    _image.sprite = _folder.HeadPhones;
                    break;
                }
            case POWERBANK_SUBTYPE:
                {
                    _image.sprite = _folder.PowerBank;
                    break;
                }
            case LAPTOP_SUBTYPE:
                {
                    _image.sprite = _folder.Laptop;
                    break;
                }
        }
    }
    private void SetEquipPartInfo(string subtupe)
    {
        _countTxt.transform.parent.gameObject.SetActive(true);
        _countTxt.text = _count.ToString()+"/5";
        if(_count>=5) { _countTxt.color = green; }
        else { _countTxt.color = orange; }
        switch (subtupe)
        {
            case PHONE_SUBTYPE:
                {
                    _image.sprite = _folder.SmartPhoneS;
                    break;
                }
            case EARBUDS_SUBTYPE:
                {
                    _image.sprite = _folder.HeadPhonesS;
                    break;
                }
            case POWERBANK_SUBTYPE:
                {
                    _image.sprite = _folder.PowerBankS;
                    break;
                }
            case LAPTOP_SUBTYPE:
                {
                    _image.sprite = _folder.LaptopS;
                    break;
                }
        }
    }
    private void SetLootBoxInfo(string subtupe)
    {
        _countTxt.transform.parent.gameObject.SetActive(true);
        _countTxt.text = _count.ToString();
        switch (subtupe)
        {
            case REGULAR_LOOTBOX_SUBTYPE:
                {
                    _image.sprite = _folder.RegularLootBox;
                    break;
                }
            case PREMIUM_LOOTBOX_SUBTYPE:
                {
                    _image.sprite = _folder.PremiumLootBox;
                    break;
                }
        }
    }
    string ConvertToHex(BigInteger bigIntegerArray)
    {
        return bigIntegerArray.ToString("X");
    }

}
