# UnityServerAndClient

Notes for me to edit at home

--Mongo.cs Line 42

Function -- FindAccountByUsernameAndDiscriminator

var filter = Builders<Model_Account>.Filter.Eq("Username", _Username);
filter = (filter & Builders<Model_Account>.Filter.Eq("Discriminator", _Discriminator));
return accounts.Find(filter).FirstOrDefault();
