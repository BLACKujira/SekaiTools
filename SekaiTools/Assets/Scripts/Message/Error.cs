using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Message
{
    public static class Error
    {
        public const string STR_ERROR = "错误";
        public const string STR_WARNING_DO = "危险操作确认";
        public const string STR_TABLE_CORRUPTION = "数据表损坏，请尝试更新数据表";
    }

    public static class IO
    {
        public const string STR_SAVECOMPLETE = "保存完成";
        public const string STR_PLEASECREATEDATA = "请创建存档";
    }

    public static class Info
    {
        public const string STR_NOW_LOADING_L2DMODEL = "正在加载模型";

    }
}