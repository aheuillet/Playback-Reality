using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Model_Trans
{
    public ObjectId _id { set; get; }

    public int x { private set; get; }
    public int y { private set; get; }
    public int z { private set; get; }
    public int frame_number { private set; get; }
    public string trans_type { private set; get; }

    //Possible Methods ...

    public override string ToString() => "Translation: \n Type: " + trans_type + "\n x: " + x + "\n y: " + y + "\n z: " + z;
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
        transList.Sort(new TransComp());
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorTk() 
    {
        Transform tf;
        int currentFrameNumber = transList[0].frame_number;
        while (transList[0].frame_number == currentFrameNumber)
        {
            Model_Trans trans = transList[0];   
            switch (trans.trans_type)
            {
                case "Left Hand":
                    tf = this.animator.GetBoneTransform(HumanBodyBones.LeftHand);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(trans.x, trans.y, trans.z));   
                case "Right Hand":
                case "Left Foot":
                case "Right Foot":
                case "Tibia":
                case "Femur":
                case "Head":
                default:
            }
            transList.Remove(trans);
        }
    }

    /* 
        AnimationEvent TransToAnim(Model_Trans t) 
        {
            animator.Set
            
            return evt;
        } */
}