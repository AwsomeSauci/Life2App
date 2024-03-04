using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenController : MonoBehaviour
{
    [SerializeField] private Sprite[] boxesImg;
    [SerializeField] private Image boxImg;
    [SerializeField] private Image ItemImg;
    [SerializeField] private GameObject LightImg;
    [SerializeField] private Button openBtn;
    [SerializeField] private Button claimBtn;
    [SerializeField] private GameObject OpenerObject;
    [SerializeField] private TextMeshProUGUI itemsCnt;
    private ImagesFolder imagesFolder;
    private MainControlScript mainControlScript;
    void Start()
    {
        imagesFolder = FindFirstObjectByType<ImagesFolder>();
        mainControlScript = FindFirstObjectByType<MainControlScript>();
    }

    public void ShowClosedBox()
    {
        ItemImg.gameObject.SetActive(false);
        boxImg.sprite = boxesImg[0];
        itemsCnt.text = "";
        LightImg.SetActive(false);
        openBtn.interactable = true;
        claimBtn.interactable = false;
        OpenerObject.SetActive(true);
    }

    public void ShowNotClaimedBox()
    {
        ItemImg.gameObject.SetActive(false);
        boxImg.sprite = boxesImg[1];
        itemsCnt.text = "";
        LightImg.SetActive(false);
        openBtn.interactable = false;
        claimBtn.interactable = true;
        OpenerObject.SetActive(true);
    }
    public async void ShowClaimedBox(BigInteger id, BigInteger count)
    {
        string hexID = ConvertToHex(id);
        if (hexID[0] == '0') hexID = hexID.Substring(1);
        string subtype = hexID.Substring(16, 16);
        boxImg.sprite = boxesImg[2];
        SetEquipInfo(subtype);
        itemsCnt.text = count.ToString()+"/5";
        LightImg.SetActive(true);
        openBtn.interactable = false;
        claimBtn.interactable = false;
        OpenerObject.SetActive(true);
        await Task.Delay(10000);
        Hide();
    }

    public void Hide()
    {
        OpenerObject.SetActive(false);
    }
    private void SetEquipInfo(string subtupe)
    {
        ItemImg.gameObject.SetActive(true);
        switch (subtupe)
        {
            case PHONE_SUBTYPE:
                {
                    ItemImg.sprite = imagesFolder.SmartPhoneS;
                    break;
                }
            case EARBUDS_SUBTYPE:
                {
                    ItemImg.sprite = imagesFolder.HeadPhonesS;
                    break;
                }
            case POWERBANK_SUBTYPE:
                {
                    ItemImg.sprite = imagesFolder.PowerBankS;
                    break;
                }
            case LAPTOP_SUBTYPE:
                {
                    ItemImg.sprite = imagesFolder.LaptopS;
                    break;
                }
        }
    }
    string ConvertToHex(BigInteger bigIntegerArray)
    {
        return bigIntegerArray.ToString("X");
    }
    private const string PHONE_SUBTYPE = "B5816CFC55FB3CB5";
    private const string EARBUDS_SUBTYPE = "7C1FF20149A3FEF4";
    private const string POWERBANK_SUBTYPE = "64E53703AB552E2E";
    private const string LAPTOP_SUBTYPE = "57A3F6CA210E2B2A";
}
