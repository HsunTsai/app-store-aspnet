using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(applicationMetadata))]
    public partial class application
    {
        partial void Initialize()
        {
            privacy_type = "public";
            @lock = false;
        }

        public partial class applicationMetadata
        {
            public int id { get; set; }

            /// <summary>
            /// 應用程式名稱
            /// </summary>
            [Display(Name = "應用程式名稱")]
            public string name { get; set; }

            /// <summary>
            /// 該應用程式隱私(pivate or public)
            /// </summary>
            [Display(Name = "該應用程式是否公開")]
            //private string _privacy_type = "public";
            public string privacy_type { get; set; }

            /// <summary>
            /// 是否將應用程式關閉或下架
            /// </summary>
            [Display(Name = "是否將應用程式關閉或下架")]
            public bool @lock { get; set; }
            
            [JsonIgnore]
            public virtual privacy privacy { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<mobile_crash> mobile_crash { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<release> release { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<tracking> tracking { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<user_application> user_application { get; set; }
        }
    }
}
