using System.IO;
using System.Xml.Serialization;

namespace SekaiTools.SekaiViewerInterface
{
    // 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/", IsNullable = false)]
    public partial class ListBucketResult
    {
        private string nameField;

        private string prefixField;

        private ushort keyCountField;

        private ushort maxKeysField;

        private string delimiterField;

        private bool isTruncatedField;

        private FilePathContents[] contentsField;

        private FilePathCommonPrefixes[] commonPrefixesField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Prefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }

        /// <remarks/>
        public ushort KeyCount
        {
            get
            {
                return this.keyCountField;
            }
            set
            {
                this.keyCountField = value;
            }
        }

        /// <remarks/>
        public ushort MaxKeys
        {
            get
            {
                return this.maxKeysField;
            }
            set
            {
                this.maxKeysField = value;
            }
        }

        /// <remarks/>
        public string Delimiter
        {
            get
            {
                return this.delimiterField;
            }
            set
            {
                this.delimiterField = value;
            }
        }

        /// <remarks/>
        public bool IsTruncated
        {
            get
            {
                return this.isTruncatedField;
            }
            set
            {
                this.isTruncatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CommonPrefixes")]
        public FilePathCommonPrefixes[] CommonPrefixes
        {
            get
            {
                return this.commonPrefixesField;
            }
            set
            {
                this.commonPrefixesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Contents")]
        public FilePathContents[] Contents
        {
            get
            {
                return this.contentsField;
            }
            set
            {
                this.contentsField = value;
            }
        }

        public static readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(ListBucketResult));

        public static ListBucketResult Deserialize(byte[] xmlData)
        {
            return (ListBucketResult)xmlSerializer.Deserialize(new MemoryStream(xmlData));
        }
    }
}