using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Driver.Builders;
using UnityEngine;

public class Mongo
{
    private const string MONGO_URI = "mongodb+srv://Inthemindofforrest:h6tjw7UP2kTSnbwd@lobbydb-redag.mongodb.net/test?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "Lobbydb";

    private MongoClient Client;
    private IMongoDatabase DB;

    private IMongoCollection<Model_Account> accounts;

    public void Init()
    {
        Client = new MongoClient(MONGO_URI);
        DB = Client.GetDatabase(DATABASE_NAME);

        //This is where we would initialize collections
        accounts = DB.GetCollection<Model_Account>("account");
        Debug.Log("Database has been Initilized");
    }
    public void Shutdown()
    {
        Client = null;
        DB = null;
    }

    #region Fetch
    public Model_Account FindAccountByEmail(string _Email)
    {
        //return accounts.( Query<Model_Account>.EQ(u => u.Email, _Email);
        var filter = Builders<Model_Account>.Filter.Eq("Email", _Email);
        return accounts.Find(filter).FirstOrDefault();
    }
    public Model_Account FindAccountByUsernameAndDiscriminator(string _Username, string _Discriminator)
    {
        var filter = Builders<Model_Account>.Filter.Eq("Username", _Username);
        return accounts.Find(filter).FirstOrDefault();
    }
    #endregion

    #region Update
    #endregion

    #region Insert
    public bool InsertAccount(string _Username, string _Password, string _Email)
    {
        if(!Utility.IsEmail(_Email))
        {
            Debug.Log(_Email + " is not a email");
            return false;
        }
        if (!Utility.IsUsername(_Username))
        {
            Debug.Log(_Username + " is not a username");
            return false;
        }
        if (FindAccountByEmail(_Email) != null)
        {
            Debug.Log(_Email + " is already being used");
            return false;
        }
        Model_Account NewAccount = new Model_Account();
        NewAccount.Username = _Username;
        NewAccount.ShaPassword = _Password;
        NewAccount.Email = _Email;
        NewAccount.Discriminator = "0000";

        //Roll for unique Discriminator
        //int rollCount = 0;
        //while(FindAccount(NewAccount,_Username, NewAccount.Discriminator) != null)
        //{
        //    NewAccount.Discriminator = Random.Range(0, 9999).ToString("0000");

        //    rollCount++;
        //    if(rollCount > 1000)
        //    {
        //        Debug.Log("We rolled to many times, suggest a username change!");
        //        return false;
        //    }
        //}

        InsertAsyncAccount(NewAccount);

        return true;
    }
    private async void InsertAsyncAccount(Model_Account _Account)
    {
        await accounts.InsertOneAsync(_Account);
    }
    #endregion

    #region Delete
    #endregion
}
