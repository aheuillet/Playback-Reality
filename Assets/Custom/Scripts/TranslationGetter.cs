using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Model_Trans {
        public ObjectId _id { set; get; }
        
        public int x { private set; get; }
        public int y { private set; get; }
        public int z { private set; get; }
        public int frame_number {private set; get; }
        public string trans_type { private set; get; }

    //Possible Methods ...

    public override string ToString() => "Translation: \n Type: " + trans_type + "\n x: " + x + "\n y: " + y + "\n z: " + z;
}

public class TranslationGetter : MonoBehaviour
{
    private const string MONGO_URI = "mongodb://127.0.0.1:27017";
    private const string DATABASE_NAME = "vicon";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<Model_Trans> translations;
    private List<Model_Trans> transList;
    private Animator animator;
   
    // Start is called before the first frame update
    void Start()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        translations = db.GetCollection<Model_Trans>("translations");
        transList = translations.Find(trans => true).ToList();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var trans in transList)
        {
            print(trans.ToString());
    }
}
