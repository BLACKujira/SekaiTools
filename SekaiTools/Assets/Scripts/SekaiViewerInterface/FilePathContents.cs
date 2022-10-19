namespace SekaiTools.SekaiViewerInterface
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public partial class FilePathContents
    {

        private string keyField;

        private System.DateTime lastModifiedField;

        private string eTagField;

        private uint sizeField;

        private FilePathContentsOwner ownerField;

        private string storageClassField;

        /// <remarks/>
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastModified
        {
            get
            {
                return this.lastModifiedField;
            }
            set
            {
                this.lastModifiedField = value;
            }
        }

        /// <remarks/>
        public string ETag
        {
            get
            {
                return this.eTagField;
            }
            set
            {
                this.eTagField = value;
            }
        }

        /// <remarks/>
        public uint Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public FilePathContentsOwner Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }

        /// <remarks/>
        public string StorageClass
        {
            get
            {
                return this.storageClassField;
            }
            set
            {
                this.storageClassField = value;
            }
        }
    }


}