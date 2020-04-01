using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveData
{
    [System.Serializable]
    public class User
    {
        public string Username;
        public string ShaPassword;

        public User(string _Username, string _ShaPassword)
        {
            Username = _Username;
            ShaPassword = _ShaPassword;
        }
    }


    public static void Save(string _Username, string _ShaPassword)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SavedLogin";
        FileStream Stream = new FileStream(path, FileMode.Create);

        User user = new User(_Username, _ShaPassword);

        Formatter.Serialize(Stream, user);
        Stream.Close();
    }

    public static User Load()
    {
        string path = Application.persistentDataPath + "/SavedLogin";

        if(File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            User user = Formatter.Deserialize(Stream) as User;
            Stream.Close();

            return user;
        }
        return null;
    }

    public static bool RemoveSavedData()
    {
        string path = Application.persistentDataPath + "/SavedLogin";

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
        return false;
    }
}
