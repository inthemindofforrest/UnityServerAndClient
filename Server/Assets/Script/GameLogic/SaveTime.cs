using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveTime
{
    public static void Save(TIME.YEAR _ServerTime)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ServerTime";
        FileStream Stream = new FileStream(path, FileMode.Create);

        Formatter.Serialize(Stream, _ServerTime);
        Stream.Close();
    }

    public static TIME.YEAR Load()
    {
        string path = Application.persistentDataPath + "/ServerTime";

        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            TIME.YEAR Year = Formatter.Deserialize(Stream) as TIME.YEAR;
            Stream.Close();

            return Year;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static bool RemoveSavedData()
    {
        string path = Application.persistentDataPath + "/ServerTime";

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                Debug.LogError("Could not delete file");
            }
            return true;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return false;
        }
    }
}
