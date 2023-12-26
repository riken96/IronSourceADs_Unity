using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
    

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //--------------------------BANNER ADS--------------------//
    public void LoadBanneADs()
    {
        //IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        IronSource.Agent.loadBanner(new IronSourceBannerSize(320, 50), IronSourceBannerPosition.BOTTOM);
    }

    public void ShowBannerADs()
    {
        IronSource.Agent.hideBanner();
    }

    public void HideBanner()
    {
        IronSource.Agent.displayBanner();
    }



}
