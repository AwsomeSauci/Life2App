using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesFolder : MonoBehaviour
{
    public Sprite SmartPhone;
    public Sprite Laptop;
    public Sprite HeadPhones;
    public Sprite PowerBank;
    public Sprite SmartPhoneS;
    public Sprite LaptopS;
    public Sprite HeadPhonesS;
    public Sprite PowerBankS;
    public Sprite SmartPhoneE;
    public Sprite LaptopE;
    public Sprite HeadPhonesE;
    public Sprite PowerBankE;
    public Sprite RegularLootBox;
    public Sprite PremiumLootBox;
    public Sprite[] BoxImagesB;
    public Sprite[] BoxImagesS;

    [HideInInspector]
    public static ImagesFolder instance;

    private ImagesFolder()
    { }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
