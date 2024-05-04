using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUD : MonoBehaviour
{
    public Image primaryAbility;
    public Image lightningCD;
    public Image moonwalkCD;
    public Image blackholeCD;
    public Image shieldCD;
    public Image currentImage = null;

    // Start is called before the first frame update
    void Start()
    {
        primaryAbility.fillAmount = 0;
        lightningCD.fillAmount = 0;
        moonwalkCD.fillAmount = 0;
        blackholeCD.fillAmount = 0;
        shieldCD.fillAmount = 0;

    }

    public void UpdateKunai(bool onOff)
    {
        if (onOff)
        {
            primaryAbility.fillAmount = 1;
        }else
        {
            primaryAbility.fillAmount = 0;
        }
    }

    public void UpdateSecondary(string abilityName, bool onOff)
    {
        switch (abilityName)
        {
            case "Blackhole":
                if (onOff)
                {
                    if (currentImage != null)
                    {
                        currentImage.fillAmount = 0;
                    }

                    blackholeCD.fillAmount = 1;

                    currentImage = blackholeCD;
                }
                else
                {
                    blackholeCD.fillAmount = 0;
                }
                break;

            case "Shield":
                if (onOff)
                {
                    shieldCD.fillAmount = 1;

                    if (currentImage != null)
                    {
                        currentImage.fillAmount = 0;
                    }
                    currentImage = shieldCD;
                }
                else
                {
                    shieldCD.fillAmount = 0;
                }
                break;

            case "Lightning":
                if (onOff)
                {
                    lightningCD.fillAmount = 1;

                    if (currentImage != null)
                    {
                        currentImage.fillAmount = 0;
                    }
                    currentImage = lightningCD;
                }
                else
                {
                    lightningCD.fillAmount = 0;
                }
                break;

            case "Moonwalk":
                if (onOff)
                {
                    moonwalkCD.fillAmount = 1;

                    if (currentImage != null)
                    {
                        currentImage.fillAmount = 0;
                    }
                    currentImage = moonwalkCD;
                }
                else
                {
                    moonwalkCD.fillAmount = 0;
                }
                break;
        }    
    }
}
