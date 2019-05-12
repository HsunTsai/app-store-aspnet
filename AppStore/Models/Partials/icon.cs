using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(iconMetadata))]
    public partial class icon
    {
        public class iconMetadata
        {
            public int id { get; set; }

            /// <summary>
            /// 路徑
            /// </summary>
            [Display(Name = "路徑")]
            public string path { get; set; }

            /// <summary>
            /// 對應應用程式id
            /// </summary>
            [Display(Name = "對應應用程式id")]
            public int application_id { get; set; }

            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<release> release { get; set; }
        }
    }
}
