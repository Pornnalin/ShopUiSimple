using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the singleton instance and access to shop items data.
/// </summary>
public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public ShopItem shopItemSO;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
