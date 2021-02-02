// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Azure.AI.Translator.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Source of the input documents
    /// </summary>
    public partial class SourceInput
    {
        /// <summary>
        /// Initializes a new instance of the SourceInput class.
        /// </summary>
        public SourceInput()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SourceInput class.
        /// </summary>
        /// <param name="sourceUrl">Location of the folder / container or
        /// single file with your documents</param>
        /// <param name="language">Language code
        /// If none is specified, we will perform auto detect on the
        /// document</param>
        /// <param name="storageSource">Possible values include:
        /// 'AzureBlob'</param>
        public SourceInput(string sourceUrl, DocumentFilter filter = default(DocumentFilter), string language = default(string), string storageSource = default(string))
        {
            SourceUrl = sourceUrl;
            Filter = filter;
            Language = language;
            StorageSource = storageSource;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets location of the folder / container or single file with
        /// your documents
        /// </summary>
        [JsonProperty(PropertyName = "sourceUrl")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "filter")]
        public DocumentFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets language code
        /// If none is specified, we will perform auto detect on the document
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'AzureBlob'
        /// </summary>
        [JsonProperty(PropertyName = "storageSource")]
        public string StorageSource { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (SourceUrl == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SourceUrl");
            }
        }
    }
}
