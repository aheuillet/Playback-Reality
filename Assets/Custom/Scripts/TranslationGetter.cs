using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Model_Trans
{
    public ObjectId _id { set; get; }

    public int x_trans { private set; get; }
    public int y_trans { private set; get; }
    public int z_trans { private set; get; }
    public int x_rot { private set; get; }
    public int y_rot { private set; get; }
    public int z_rot { private set; get; }
    public int w { private set; get; }
    public int frame_number { private set; get; }
    public string trans_type { set; get; }

    //Possible Methods ...

    public override string ToString() => "Translation: \n Type: " + trans_type + "\n x_trans: " + x_trans + "\n y_trans: " + y_trans + "\n z_trans: " + z_trans;
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
    private GameObject skeleton;


    // Start is called before the first frame update
    void Start()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        translations = db.GetCollection<Model_Trans>("translations");
        transList = translations.Find(trans => true).ToList();
        transList.Sort(new TransComp());
        animator = GetComponent<Animator>();
        skeleton = this.gameObject.transform.GetChild(0).gameObject;
    }

    void LateUpdate()
    {
      Output_GetSubjectRootSegmentName OGSRSN = Client.GetSubjectRootSegmentName(SubjectName);
      Transform Root = transform.root;
      FindAndTransform( Root, OGSRSN.SegmentName);
    }

    string strip(string BoneName)
    {
      if (BoneName.Contains(":"))
      {
        string[] results = BoneName.Split(':');
        return results[1];
      }
      return BoneName;
    }
    void FindAndTransform(Transform iTransform, string BoneName )
    {
      int ChildCount = iTransform.childCount;
      for (int i = 0; i < ChildCount; ++i)
      {
        Transform Child = iTransform.GetChild(i);
        if( strip( Child.name) == BoneName )
        { 
          ApplyBoneTransform(Child);
          TransformChildren(Child);
          break;
        }
        // if not finding root in this layer, try the children
        FindAndTransform(Child, BoneName);
      }
    }
    void TransformChildren(Transform iTransform )
    {
      int ChildCount = iTransform.childCount;
      for (int i = 0; i < ChildCount; ++i)
      {
        Transform Child = iTransform.GetChild(i);
        ApplyBoneTransform(Child);
        TransformChildren(Child);
      }
    }

    Model_Trans FindBoneInTrans(string BoneName) 
    {
        foreach (Model_Trans trans in transList)
        {
            if (trans.trans_type == BoneName)
            {
                return trans;
            }
        }
        Debug.LogError("No corresponding bone found");
        Model_Trans a = new Model_Trans();
        a.trans_type = "default";
        return a;
    }

    private void ApplyBoneTransform(Transform Bone)
    {
      string BoneName = strip(Bone.gameObject.name);
      // update the bone transform from the data stream
      Model_Trans t = FindBoneInTrans(BoneName);
      if (t.trans_type != "default")
      {
        Quaternion Rot = new Quaternion((float)t.x_rot, (float)t.y_rot, (float)t.z_rot, (float)t.w);
        // mapping right hand to left hand flipping x
        Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
    
        Vector3 Translate = new Vector3((float)t.x_trans * 0.001f, (float)t.y_trans * 0.001f, (float)t.z_trans * 0.001f);
        Bone.localPosition = new Vector3(-Translate.x, Translate.y, Translate.z);
      }

      /* // If there's a scale for this subject in the datastream, apply it here.
      if (IsScaled)
      {
        Output_GetSegmentStaticScale OScale = Client.GetSegmentScale(SubjectName, BoneName);
        if (OScale.Result == Result.Success)
        {
          Bone.localScale = new Vector3((float)OScale.Scale[0], (float)OScale.Scale[1], (float)OScale.Scale[2]);
        }
      }*/
    } 

    /* 
        AnimationEvent TransToAnim(Model_Trans t) 
        {
            animator.Set
            
            return evt;
        } */
}