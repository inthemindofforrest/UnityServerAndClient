using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using UnityEngine;

public class Mongo
{
    private const string MONGO_URI = "mongodb+srv://Inthemindofforrest:h6tjw7UP2kTSnbwd@lobbydb-redag.mongodb.net/test?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "Lobbydb";

    private MongoClient Client;
    private IMongoDatabase DB;

    private IMongoCollection<Model_Account> accounts;
    private IMongoCollection<Model_Stats> AccountInfo;
    private IMongoCollection<Model_Civ_Stats> AccountCivs;

    public void Init()
    {
        Client = new MongoClient(MONGO_URI);
        DB = Client.GetDatabase(DATABASE_NAME);

        //This is where we would initialize collections
        accounts = DB.GetCollection<Model_Account>("account");
        AccountInfo = DB.GetCollection<Model_Stats>("AccountStats");
        AccountCivs = DB.GetCollection<Model_Civ_Stats>("AccountCivs");
        Console.DeveloperConsole.sLog("Database has been Initilized");
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
        filter = (filter & Builders<Model_Account>.Filter.Eq("Discriminator", _Discriminator));
        return accounts.Find(filter).FirstOrDefault();
    }
    public Model_Account FindAccountByConnectionID(int _cnnID)
    {
        var filter = Builders<Model_Account>.Filter.Eq("ActiveConnection", _cnnID);
        return accounts.Find(filter).FirstOrDefault();
    }

    public Model_Stats FindAccountStatsByConnectionID(int _cnnID)
    {
        //return accounts.( Query<Model_Account>.EQ(u => u.Email, _Email);
        var filter = Builders<Model_Stats>.Filter.Eq("ActiveConnection", _cnnID);
        return AccountInfo.Find(filter).FirstOrDefault();
    }
    public string FindAccountEmailByConnectionID(int _cnnID)
    {
        //return accounts.( Query<Model_Account>.EQ(u => u.Email, _Email);
        var filter = Builders<Model_Account>.Filter.Eq("ActiveConnection", _cnnID);
        var Account = accounts.Find(filter).FirstOrDefault();
        return Account.Email;
    }

    public Model_Civ_Stats FindCivStatsByConnectionID(int _cnnID)
    {
        //Gets the email from the logged in account
        var filter = Builders<Model_Account>.Filter.Eq("ActiveConnection", _cnnID);
        var Account = accounts.Find(filter).FirstOrDefault();
        Console.DeveloperConsole.sLog(Account.Email);

        //Gets the Array from the email
        var filter2 = Builders<Model_Civ_Stats>.Filter.Eq("Email", Account.Email);
        var itty = AccountCivs.Find(filter2).FirstOrDefault();
        Console.DeveloperConsole.sLog(itty.Civs.Length.ToString());
        return itty;
    }
    #endregion

    #region Update
    public void UpdateAccountAfterDisconnection(string _Email)
    {
        var filter = Builders<Model_Account>.Filter.Eq("Email", _Email);
        var account = accounts.Find(filter).FirstOrDefault();

        var UpdatedAccount = Builders<Model_Account>.Update.Set("ActiveConnection", 0)
                                                  .Set("Token", (string)null)
                                                  .Set("Status", 0);
        accounts.UpdateOne(filter, UpdatedAccount);

        
    }
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
        int rollCount = 0;
        while (FindAccountByUsernameAndDiscriminator(_Username, NewAccount.Discriminator) != null)
        {
            NewAccount.Discriminator = Random.Range(0, 9999).ToString("0000");

            rollCount++;
            if (rollCount > 1000)
            {
                Debug.Log("We rolled to many times, suggest a username change!");
                return false;
            }
        }

        InsertAsyncAccount(NewAccount);

        return true;
    }
    private async void InsertAsyncAccount(Model_Account _Account)
    {
        await accounts.InsertOneAsync(_Account);
    }

    public Model_Account LoginAccount(string _UsernameOrEmail, string _Password, int _cnnID, string _Token)
    {
        Model_Account MyAccount = null;
        FilterDefinition<Model_Account> filter = null;


        if (Utility.IsEmail(_UsernameOrEmail))
        {
            filter = Builders<Model_Account>.Filter.Eq("Email", _UsernameOrEmail);
            filter = (filter & Builders<Model_Account>.Filter.Eq("ShaPassword", _Password));
            MyAccount = accounts.Find(filter).FirstOrDefault();
        }
        else
        {
            string[] data = _UsernameOrEmail.Split('#');
            if (data[1] != null)
            {
                filter = Builders<Model_Account>.Filter.Eq("Username", data[0]);
                filter = (filter & Builders<Model_Account>.Filter.Eq("Discriminator", data[1]));
                filter = (filter & Builders<Model_Account>.Filter.Eq("ShaPassword", _Password));
                MyAccount = accounts.Find(filter).FirstOrDefault();
            } 
        }
        if(MyAccount != null)
        {
            var UpdatedAccount = Builders<Model_Account>.Update.Set("ActiveConnection", _cnnID)
                                                  .Set("Token", _Token)
                                                  .Set("Status", 1)
                                                  .Set("LastLogin", System.DateTime.Now);
            accounts.UpdateOne(filter, UpdatedAccount);
        }
        else
        {
            //Did not log in
        }
        return MyAccount;
    }

    #region Stats
    public bool InsertStats(string _Email)
    {
        Model_Stats NewStats = new Model_Stats();
        NewStats.Gold = 0;
        NewStats.Email = _Email;

        InsertAsyncStats(NewStats);

        return true;
    }
    private async void InsertAsyncStats(Model_Stats _Stat)
    {
        await AccountInfo.InsertOneAsync(_Stat);
    }
    public Model_Stats UpdateStats(string _Email)
    {
        Model_Stats MyAccount = null;
        FilterDefinition<Model_Stats> filter = null;

            filter = Builders<Model_Stats>.Filter.Eq("Email", _Email);
            MyAccount = AccountInfo.Find(filter).FirstOrDefault();

        if (MyAccount != null)
        {
            int NewGold = MyAccount.Gold + Random.Range(2, 20);

            var UpdatedAccount = Builders<Model_Stats>.Update.Set("Gold", NewGold);
            MyAccount.Gold = NewGold;
            AccountInfo.UpdateOne(filter, UpdatedAccount);
        }
        else
        {
            //Did not log in
        }
        return MyAccount;
    }
    #endregion

    #region Civs
    public bool InsertCiv(string _Email)
    {
        Model_Civ_Stats NewStats = new Model_Civ_Stats();
        NewStats.Email = _Email;
        NewStats.Civs = new GameLogic.SerializedCiv[1];

        GameLogic.Civ TempNewCiv = new GameLogic.Civ();
        TempNewCiv.AssignStarterAtts();

        NewStats.Civs[0] = new GameLogic.SerializedCiv();
        NewStats.Civs[0] = GameLogic.Civ.CreateSerializedCivFromCiv(TempNewCiv);

        InsertAsyncCiv(NewStats);

        return true;
    }
    private async void InsertAsyncCiv(Model_Civ_Stats _Stat)
    {
        await AccountCivs.InsertOneAsync(_Stat);
    }
    public Model_Civ_Stats UpdateCiv(string _Email)
    {
        Model_Civ_Stats MyAccount = null;
        FilterDefinition<Model_Civ_Stats> filter = null;

        filter = Builders<Model_Civ_Stats>.Filter.Eq("Email", _Email);
        MyAccount = AccountCivs.Find(filter).FirstOrDefault();

        if (MyAccount != null)
        {
            //var UpdatedAccount = Builders<Model_Civ_Stats>.Update.Set("Gold", NewGold);
            //MyAccount.Gold = NewGold;
            //AccountInfo.UpdateOne(filter, UpdatedAccount);
        }
        else
        {
            //Did not log in
        }
        return MyAccount;
    }
    #endregion

    #endregion

    #region Delete
    #endregion
}
