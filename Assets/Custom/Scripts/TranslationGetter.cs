using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Model_Trans
{
    public int _id { set; get; }

    public double x_trans {set; get; }
    public double y_trans { set; get; }
    public double z_trans { set; get; }
    public double x_rot { set; get; }
    public double y_rot { set; get; }
    public double z_rot { set; get; }
    public double w { set; get; }
    public int frame_number { set; get; }
    public string segment_name { set; get; }
    public string translation_type { set; get; }

    //Possible Methods ...

    public override string ToString() => "Translation: \n Type: " + translation_type + "\n x_trans: " + x_trans + "\n y_trans: " + y_trans + "\n z_trans: " + z_trans;
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
    private string SERVER_HOST = "192.168.4.77/middleware";
    private string DATABASE_NAME = "vicon";
    private string COLLECTION_NAME = "Mike_2019-08-14 04:25:17.710194";
    private string rootSegmentName;
    private List<Model_Trans> transList;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        settings = GameObject.Find("SettingsSingleton").GetComponent<Settings>();
        if (settings) {
            settings.LoadToScript();
            DATABASE_NAME = settings.DatabaseName;
            COLLECTION_NAME = settings.CollectionName;
            SERVER_HOST = settings.ServerIP;
        }        
        string URL = "http://" + SERVER_HOST + "?collection=" + COLLECTION_NAME;
        IEnumerator couroutine = GetJsonTextFromURL(URL);
        yield return StartCoroutine(couroutine);
        transList.Sort(new TransComp());
        this.gameObject.transform.Rotate(-79.15f, 0.0f, -173.728f); //Rotating GameObject to macth Unity axis orientation
    }

    IEnumerator GetJsonTextFromURL(string URL) {
        UnityWebRequest request = new UnityWebRequest();
        request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        }
        else {
           transList = JsonConvert.DeserializeObject<List<Model_Trans>>(request.downloadHandler.text);
        }
    }

    void LateUpdate()
    {
        if (transList != null) 
        {
            rootSegmentName = "Root";
            Transform Root = transform.root;
            FindAndTransform(Root, rootSegmentName);
        }
        else {
            //Debug.Log("translist is null!");
        }
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
            // flipping the x-axis (because Unity is a right-handed system while Vicon is a left-handed system)
            Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);

            Vector3 Translate = new Vector3((float)t.x_trans * 0.001f, (float)t.y_trans * 0.001f, (float)t.z_trans * 0.001f);
            Bone.localPosition = new Vector3(-Translate.x, Translate.y, Translate.z);
        }
    }

}