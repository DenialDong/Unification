using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class testForAssetReference : MonoBehaviour
{
    public AssetReference assetReference;

    // Start is called before the first frame update
    void Start()
    {
        LoadAndUseAsset();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void LoadAndUseAsset()
    {
        assetReference.LoadAssetAsync<GameObject>().Completed += handle =>
        {
            GameObject loadedObject = handle.Result;
            // Use the loaded GameObject, for instance, instantiate it:
            Instantiate(loadedObject);
        };
    }
}