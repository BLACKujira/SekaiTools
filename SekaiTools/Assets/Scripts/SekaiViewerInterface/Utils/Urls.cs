using SekaiTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.SekaiViewerInterface
{
    public enum MasterSever { cn,ww }

    public static class Url
    {
        public static readonly MasterUrl MasterUrl = new MasterUrl();
        public static readonly AssetUrl AssetUrl = new AssetUrl();
    }

    public class MasterUrl
    {
        public string this[MasterSever masterSever , ServerRegion serverRegion] 
        {
            get 
            {
                switch (masterSever)
                {
                    case MasterSever.cn:
                        //使用此服务器可能对其造成过大负担
                        WindowController.ShowLog(Message.Error.STR_ERROR, "此服务器不可在SekaiTools中使用");
                        return null;
                    case MasterSever.ww:
                        switch (serverRegion)
                        {
                            case ServerRegion.jp:
                                return Env.VITE_JSON_DOMAIN_MASTER + "/sekai-master-db-diff";
                            case ServerRegion.tw:
                                return Env.VITE_JSON_DOMAIN_MASTER + "/sekai-master-db-tc-diff";
                            case ServerRegion.cn:
                                return Env.VITE_JSON_DOMAIN_MASTER + "/sekai-master-db-cn-diff";
                            case ServerRegion.en:
                                return Env.VITE_JSON_DOMAIN_MASTER + "/sekai-master-db-en-diff";
                            default:
                                throw new ServerNotFoundException();
                        }
                    default: throw new MasterSeverNotFoundException();
                }
            }
        }

        [System.Serializable]
        public class MasterSeverNotFoundException : System.Exception
        {
            public MasterSeverNotFoundException() { }
            public MasterSeverNotFoundException(string message) : base(message) { }
            public MasterSeverNotFoundException(string message, System.Exception inner) : base(message, inner) { }
            protected MasterSeverNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }

    public enum AssetSever { cn, ww, minio }
    public class AssetUrl
    {
        public string this[AssetSever assetSever, ServerRegion serverRegion] 
        {
            get 
            {
                switch (assetSever)
                {
                    case AssetSever.cn:
                        //使用此服务器可能对其造成过大负担
                        WindowController.ShowLog(Message.Error.STR_ERROR, "此服务器不可在SekaiTools中使用");
                        return null;
                    case AssetSever.ww:
                        switch (serverRegion)
                        {
                            case ServerRegion.jp:
                                return Env.VITE_ASSET_DOMAIN_WW + "/sekai-assets";
                            case ServerRegion.tw:
                                return Env.VITE_ASSET_DOMAIN_WW + "/sekai-tc-assets";
                            case ServerRegion.cn:
                                return Env.VITE_ASSET_DOMAIN_WW + "/sekai-cn-assets";
                            case ServerRegion.en:
                                return Env.VITE_ASSET_DOMAIN_WW + "/sekai-en-assets";
                            default:
                                throw new ServerNotFoundException();
                        }

                    case AssetSever.minio:
                        switch (serverRegion)
                        {
                            case ServerRegion.jp:
                                return Env.VITE_ASSET_DOMAIN_MINIO + "/sekai-assets";
                            case ServerRegion.tw:
                                return Env.VITE_ASSET_DOMAIN_MINIO + "/sekai-tc-assets";
                            case ServerRegion.cn:
                                return Env.VITE_ASSET_DOMAIN_MINIO + "/sekai-cn-assets";
                            case ServerRegion.en:
                                return Env.VITE_ASSET_DOMAIN_MINIO + "/sekai-en-assets";
                            default:
                                throw new ServerNotFoundException();
                        }
                    default: throw new AssetSeverNotFoundException();
                }
            }
        }


        [System.Serializable]
        public class AssetSeverNotFoundException : System.Exception
        {
            public AssetSeverNotFoundException() { }
            public AssetSeverNotFoundException(string message) : base(message) { }
            public AssetSeverNotFoundException(string message, System.Exception inner) : base(message, inner) { }
            protected AssetSeverNotFoundException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }

}