using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuScreensController : MonoBehaviour
{
    [SerializeField] private GameObject BackPackScreen;
    [SerializeField] private GameObject ShopScreen;
    [SerializeField] private GameObject BattleScreen;
    [SerializeField] private GameObject BackPackOnBtn;
    [SerializeField] private GameObject BackPackOffBtn;
    [SerializeField] private GameObject ShopOnBtn;
    [SerializeField] private GameObject ShopOffBtn;
    [SerializeField] private GameObject BattleOnBtn;
    [SerializeField] private GameObject BattleOffBtn;
    public UnityEvent onBackPack;
    public UnityEvent onShop;
    public UnityEvent onBattle;
    public void UpenBackPack()
    {
        OffAll();
        BackPackScreen.SetActive(true);
        BackPackOnBtn.SetActive(true);
        onBackPack.Invoke();
    }

    public void UpenShop()
    {
        OffAll();
        ShopScreen.SetActive(true);
        ShopOnBtn.SetActive(true);
        onShop.Invoke();
    }
    public void UpenBattle()
    {
        OffAll();
        BattleScreen.SetActive(true);
        BattleOnBtn.SetActive(true);
        onBattle.Invoke();
    }
    private void OffAll()
    {
        BackPackScreen.SetActive(false);
        ShopScreen.SetActive(false);
        BattleScreen.SetActive(false);

        BackPackOnBtn.SetActive(false);        
        ShopOnBtn.SetActive(false);
        BattleOnBtn.SetActive(false) ;

        ShopOffBtn.SetActive(true);
        BackPackOffBtn.SetActive(true);
        BattleOffBtn.SetActive(true) ;
    }
}
