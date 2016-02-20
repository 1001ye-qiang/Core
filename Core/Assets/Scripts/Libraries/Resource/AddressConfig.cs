using UnityEngine;
using System.Collections;

namespace FuckGame.Res
{

    public class AddressConfig
    {
        public static string ResourceBasePath
        {
            get
            {
#if UNITY_ANDROID
                return Application.streamingAssetsPath + "/androidPackages/";
#elif UNITY_IPHONE
                return Application.dataPath + "/Raw/iphonePackages/";
#else
                return Application.dataPath + "/StreamingAssets/Packages/";
#endif
            }
        }

        public static string ResourceLocalPath
        {
            get
            {
                return Application.persistentDataPath + "/Packages/";
            }
        }

        public static string ResourceLocalURL
        {
            get
            {
#if UNITY_EDITOR
                return "file:///" + Application.persistentDataPath + "/Packages/";
#else
                return "file://" + Application.persistentDataPath + "/Packages/";
#endif
            }
        }


        public static string ResourceBaseURL
        {
            get
            {
#if UNITY_ANDROID

#if UNITY_EDITOR
                return "file://" + Application.streamingAssetsPath + "/androidPackages/";
#else
                return Application.streamingAssetsPath + "/androidPackages/";
#endif

#elif UNITY_IPHONE

#if UNITY_EDITOR
                return "file://" + Application.dataPath + "/StreamingAssets/iphonePackages/";
#else
                return "file://" + Application.dataPath + "/Raw/iphonePackages/";
#endif

#else
                return "file://" + Application.dataPath + "/StreamingAssets/Packages/";
#endif

            }
        }


        static string outerURL = "ftp://127.0.0.1/";
        public static string ResourceOuterURL
        {
            get
            {
                return outerURL;
            }
            set
            {
                outerURL = value;
            }
        }
    }
}