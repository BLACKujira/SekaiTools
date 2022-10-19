namespace SekaiTools.SekaiViewerInterface
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public partial class FilePathCommonPrefixes
    {

        private string prefixField;

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
    }
}