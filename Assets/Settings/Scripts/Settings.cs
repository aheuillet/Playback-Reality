/*******************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity license.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 *******************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dweiss {
	[System.Serializable]
	public class Settings : ASettings {

        [Header("--Application Settings--")]
        public string ServerIP = "192.168.4.77";
        public string PortNumber = "27017";
        public string DatabaseName = "vicon";
        public string CollectionName = "Mike_2019-08-08 06:58:46.637068";

       

        /* [Header("--Lists and arrays--")]
        public float[] arrayExample;

        public List<float> listExample; */


        /* [Header("--Enum and Class--")]
        public EnumExample enumExample;
        public MySpecialClassExample classExample; */



        #region Enums and classes for serialization

        public enum EnumExample {
            Enum1,Enum2
        }


        [System.Serializable]
        public class MySpecialClassExample
        {
            public string txt = "abcd";
        }
        #endregion


        private new void Awake() {
			base.Awake ();
            SetupSingelton();
        }


        #region  Singelton
        public static Settings _instance;
        public static Settings Instance { get { return _instance; } }
        private void SetupSingelton()
        {
            if (_instance != null)
            {
                Debug.LogError("Error in settings. Multiple singeltons exists: " + _instance.name + " and now " + this.name);
            }
            else
            {
                _instance = this;
            }
        }
        #endregion



    }
}