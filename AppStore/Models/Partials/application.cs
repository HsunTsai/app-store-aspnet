using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(applicationMetadata))]
    public partial class application
    {
        public class applicationMetadata
        {
            public int id { get; set; }

            /// <summary>
            /// 應用程式名稱
            /// </summary>
            [Display(Name = "應用程式名稱")]
            public string name { get; set; }

            /// <summary>
            /// 該應用程式所屬設備(Android iOS win32 win64)
            /// </summary>
            [Display(Name = "該應用程式所屬設備(Android iOS win32 win64)")]
            public string device_type { get; set; }

            /// <summary>
            /// 是否將應用程式關閉或下架
            /// </summary>
            [Display(Name = "是否將應用程式關閉或下架")]
            public bool @lock { get; set; } = false;

            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<release> release { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<user> user { get; set; }
        }
    }
}
