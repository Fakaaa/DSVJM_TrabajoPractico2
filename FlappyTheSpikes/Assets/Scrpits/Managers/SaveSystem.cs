using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int maxItemShopElementsToLoad = 15;

    public static void SaveItems(ItemShop [] item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/items.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ItemShop[]theListToSave = new ItemShop[item.Length];

        for (int i = 0; i < item.Length; i++)
        {
            theListToSave[i] = item[i];
        }

        formatter.Serialize(stream, theListToSave);
        stream.Seek(0, SeekOrigin.Begin);

        stream.Close();
    }
   
    public static ItemShop[] LoadItems()
    {
        string path = Application.persistentDataPath + "/items.dat";

        if(File.Exists(path))
        {
            ItemShop[] loadedListOfItems = null;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Seek(0,SeekOrigin.Begin);

            loadedListOfItems = (ItemShop[])formatter.Deserialize(stream);
            stream.Close();

            return loadedListOfItems;
        }
        else
        {
            Debug.LogError("File not found at " + path);
            return null;
        }
    }
}
