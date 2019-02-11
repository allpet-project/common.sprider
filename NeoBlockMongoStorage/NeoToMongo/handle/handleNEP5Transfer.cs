﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NeoToMongo
{
    class handleNEP5Transfer
    {
        static string collectionType = "NEP5transfer";
        static IMongoCollection<NEP5.Transfer> _Collection;
        public static IMongoCollection<NEP5.Transfer> Collection
        {
            get
            {
                if (_Collection == null)
                {
                    var client = new MongoClient(Config.mongodbConnStr);
                    var database = client.GetDatabase(Config.mongodbDatabase);
                    _Collection = database.GetCollection<NEP5.Transfer>(collectionType);
                }
                return _Collection;
            }
        }

        public static void handle(int blockindex, DateTime blockTime, string txid,int n, MyJson.JsonNode_Object notification)
        {

            string findStr = "{{txid:'{0}',n:{1}}}";
            findStr = string.Format(findStr, txid, n);
            BsonDocument findB = BsonDocument.Parse(findStr);
            var quaryArr = Collection.Find(findB).ToList();
            if(quaryArr.Count==0)
            {
                //获取nep5资产信息测试
                string nep5AssetID = notification["contract"].AsString();
                //var findBsonNEP5AssetBson = BsonDocument.Parse("{assetid:'" + nep5AssetID + "'}");
                //var queryNEP5AssetBson = handleNep5.Collection.Find(findBsonNEP5AssetBson).ToList();
                var queryNEP5AssetBson = Mongo.Find(handleNep5.Collection, "assetid", nep5AssetID);

                var NEP5decimals = queryNEP5AssetBson[0].decimals;

                NEP5.Transfer tf = new NEP5.Transfer(blockindex, txid, n, notification, NEP5decimals);

                Collection.InsertOne(tf);
            }else
            {
                NEP5.Transfer tf = quaryArr[0];
            }

            //Task.Run(()=>
            //{
            //    handleAddress.HandleNotifyItem(blockindex,tf.from, txid, blockTime);
            //    handleAddress.HandleNotifyItem(blockindex, tf.to, txid, blockTime);
            //});
        }
    }
}
