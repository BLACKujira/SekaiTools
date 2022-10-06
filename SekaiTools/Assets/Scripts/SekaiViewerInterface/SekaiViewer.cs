using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SekaiViewerInterface
{
    public enum ServerRegion { jp , tw , cn , en}


    [System.Serializable]
    public class ServerNotFoundException : System.Exception
    {
        public ServerNotFoundException() { }
        public ServerNotFoundException(string message) : base(message) { }
        public ServerNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected ServerNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public static class SekaiViewer
    {
        public static ServerRegion serverRegion = ServerRegion.jp;
        public static MasterSever masterSever = MasterSever.ww;
        public static AssetSever assetSever = AssetSever.ww;

        public static string MasterUrl => Url.MasterUrl[masterSever, serverRegion];
        public static string AssetUrl => Url.AssetUrl[assetSever, serverRegion];
    }
}