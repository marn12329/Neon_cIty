using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using TMPro;
using UnityEngine.UI;

public class AnalyticsManger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    // select hero
    private void ChooseHero(string name)
    {
        CustomEvent examplEvent = new CustomEvent("ChooseHero")
        {
            { "ChooseHero", name }
        };
        
        AnalyticsService.Instance.RecordEvent(examplEvent);
        Debug.Log($"Recording Event Heroname {name}");
    }

    // Buy Coin Pack
    private void BuyCionPack(int amount)
    {
        CustomEvent examplEvent = new CustomEvent("Buycoinpacks")
        {
            { "coinpacks", amount }
        };
        
        AnalyticsService.Instance.RecordEvent(examplEvent);
        Debug.Log($"Recording Event BuyCoinPack {amount}");
    }

    // Buy Crystal Pack
    private void BuyCrystalPack(int amount)
    {
        CustomEvent examplEvent = new CustomEvent("BuyCrystalPack")
        {
            { "crystalpack", amount }
        };
        
        AnalyticsService.Instance.RecordEvent(examplEvent);
        Debug.Log($"Recording Event BuyCrystalPack {amount}");
    }
    
    // Try again
    private void Tryagain(int amount)
    {
        CustomEvent examplEvent = new CustomEvent("Trayagain")
        {
            { "tryagain", amount }
        };
        
        AnalyticsService.Instance.RecordEvent(examplEvent);
        Debug.Log($"Recording Event Trayagain {amount}");
    }
    public void OnChooseHero1()
    {
        ChooseHero("Archer");   
    }

    public void OnChooseHero2()
    {
        ChooseHero("Knight");   
    }
    
    public void OnChooseHero3()
    {
        ChooseHero("mage");   
    }

    public void BuyCoinpack1()
    {
        BuyCionPack(1);
    }
    
    public void BuyCoinpack2()
    {
        BuyCionPack(1);
    }
    
    public void BuyCoinpack3()
    {
        BuyCionPack(1);
    }
    
    public void ButCrystalpack1()
    {
        BuyCrystalPack(1);
    }
    
    public void ButCrystalpack2()
    {
        BuyCrystalPack(1);
    }
    
    public void ButCrystalpack3()
    {
        BuyCrystalPack(1);
    }
    
    public void Tryagainplay()
    {
        Tryagain(1);
    }

}    

    