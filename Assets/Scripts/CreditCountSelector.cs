using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditCountSelector : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI creditCountText;

    public void increaseCreditCount()
    {
        int creditCount = int.Parse(creditCountText.text);
        creditCount++;
        creditCountText.text = creditCount.ToString();
    }

    public void decreaseCreditCount()
    {
        int creditCount = int.Parse(creditCountText.text);
        if (creditCount > 1)
        {
            creditCount--;
            creditCountText.text = creditCount.ToString();
        }
    }
}
