using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int maxItemShopElementsToLoad = 15;

    public static void SaveItems(List<ItemShop> items)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/items.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ItemsBoughtData theListToSave = new ItemsBoughtData(items);

        formatter.Serialize(stream, theListToSave);
        stream.Seek(0, SeekOrigin.Begin);

        stream.Close();
    }
   
    public static ItemsBoughtData LoadItems()
    {
        string path = Application.persistentDataPath + "/items.dat";

        if(File.Exists(path))
        {
            ItemsBoughtData loadedListOfItems = null;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Seek(0,SeekOrigin.Begin);

            loadedListOfItems = (ItemsBoughtData)formatter.Deserialize(stream);
            stream.Close();

            return loadedListOfItems;
        }
        else
        {
            Debug.LogWarning("File not found at " + path);
            return null;
        }
    }

    public static void ResetItemsData()
    {
        string path = Application.persistentDataPath + "/items.dat";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        List<ItemShop> emptyList = new List<ItemShop>();
        ItemsBoughtData theListToSave = new ItemsBoughtData(emptyList);

        formatter.Serialize(stream, theListToSave);
        stream.Seek(0, SeekOrigin.Begin);

        stream.Close();
    }
}
