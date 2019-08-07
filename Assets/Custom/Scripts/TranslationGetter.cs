using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using Dweiss;

public class Model_Trans
{
    public ObjectId _id { set; get; }

    public double x_trans { private set; get; }
    public double y_trans { private set; get; }
    public double z_trans { private set; get; }
    public double x_rot { private set; get; }
    public double y_rot { private set; get; }
    public double z_rot { private set; get; }
    public double w { private set; get; }
    public int frame_number { private set; get; }
    public string segment_name { set; get; }
    public string translation_type { private set; get; }

    //Possible Methods ...

    public override string ToString() => "Translation: \n Type: " + translation_type + "\n x_trans: " + x_trans + "\n y_trans: " + y_trans + "\n z_trans: " + z_trans;
}

public class Model_Root
{
    public ObjectId _id { set; get; }
    public string root_segment_name { private set; get; }
    public string collection_name { private set; get; }
}

class TransComp : IComparer<Model_Trans>
{
    public int Compare(Model_Trans t1, Model_Trans t2)
    {
        if (t1.frame_number > t2.frame_number)
        {
            return 1;
        }
        else if (t1.frame_number < t2.frame_number)
        {
            return -1;
        }
        return 0;
    }
}

public class TranslationGetter : MonoBehaviour
{
    private Settings settings;
    private string MONGO_URI;
    private string DATABASE_NAME;
    private string COLLECTION_NAME;
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<Model_Trans> translations;
    private IMongoCollection<Model_Root> rootSegments;
    private string rootSegmentName;
    private List<Model_Trans> transList;


    // Start is called before the first frame update
    void Start()
    {
        settings = GameObject.Find("SettingsSingleton").GetComponent<Settings>();
        MONGO_URI = "mongodb://" + settings.ServerIP + ":" + settings.PortNumber;
        DATABASE_NAME = settings.DatabaseName;
        COLLECTION_NAME = settings.CollectionName;
        try
        {
            client = new MongoClient(MONGO_URI);
        }
        catch (System.TimeoutException)
        {
            GameObject.Find("ErrorPanel2").SetActive(true);
        }
        db = client.GetDatabase(DATABASE_NAME);
        if (db.ListCollectionNames().ToList().Count == 0)
        {
           GameObject.Find("ErrorPanel1").SetActive(true); 
        }
        translations = db.GetCollection<Model_Trans>(COLLECTION_NAME);
        transList = translations.Find(trans => true).ToList();
        transList.Sort(new TransComp());
    }

    void LateUpdate()
    {
        rootSegmentName = "Solving";
        Transform Root = transform.root;
        FindAndTransform(Root, rootSegmentName);
    }

    /* string GetSubjectRootSegmentName(string CollectionName)
    {
        rootSegments = db.GetCollection<Model_Root>("root_segments");
        var filter = Builders<Model_Root>.Filter.Eq("collection_name", CollectionName);
        return rootSegments.Find(filter).First().root_segment_name;
    } */

    string strip(string BoneName)
    {
        if (BoneName.Contains(":"))
        {
            string[] results = BoneName.Split(':');
            return results[1];
        }
        return BoneName;
    }

    void FindAndTransform(Transform iTransform, string BoneName)
    {
        int ChildCount = iTransform.childCount;
        for (int i = 0; i < ChildCount; ++i)
        {
            Transform Child = iTransform.GetChild(i);
            if (strip(Child.name) == BoneName)
            {
                ApplyBoneTransform(Child);
                TransformChildren(Child);
                break;
            }
            // if not finding root in this layer, try the children
            FindAndTransform(Child, BoneName);
        }
    }

    void TransformChildren(Transform iTransform)
    {
        int ChildCount = iTransform.childCount;
        for (int i = 0; i < ChildCount; ++i)
        {
            Transform Child = iTransform.GetChild(i);
            ApplyBoneTransform(Child);
            TransformChildren(Child);
        }
    }

    Model_Trans FindBoneInTransList(string BoneName)
    {
        foreach (Model_Trans trans in transList)
        {
            if ((trans.segment_name == BoneName))
            {
                transList.Remove(trans);
                return trans;
            }
        }
        //Debug.LogError("No corresponding bone found for name " + BoneName);
        Model_Trans a = new Model_Trans();
        a.segment_name = "default";
        return a;
    }

    private void ApplyBoneTransform(Transform Bone)
    {
        string BoneName = strip(Bone.gameObject.name);
        // update the bone transform from the data stream
        Model_Trans t = FindBoneInTransList(BoneName);
        if (t.segment_name != "default")
        {
            Quaternion Rot = new Quaternion((float)t.x_rot, (float)t.y_rot, (float)t.z_rot, (float)t.w);
            // mapping right hand to left hand flipping x
            Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);

            Vector3 Translate = new Vector3((float)t.x_trans * 0.001f, (float)t.y_trans * 0.001f, (float)t.z_trans * 0.001f);
            Bone.localPosition = new Vector3(-Translate.x, Translate.y, Translate.z);
        }
    }

}