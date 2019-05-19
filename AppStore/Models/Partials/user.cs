using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(userMetadata))]
    public partial class user
    {
        public class userMetadata
        {
            /// <summary>
            /// 帳號
            /// </summary>
            [Display(Name = "帳號")]
            public string id { get; set; }

            /// <summary>
            /// 權限
            /// </summary>
            [Display(Name = "權限")]
            public string role { get; set; }

            /// <summary>
            /// 密碼
            /// </summary>
            [Display(Name = "密碼")]
            public string password { get; set; }

            /// <summary>
            /// 使用者是否啟用
            /// </summary>
            [Display(Name = "使用者是否啟用")]
            public bool @lock { get; set; }

            [JsonIgnore]
            public virtual user_role user_role { get; set; }
            [JsonIgnore]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<user_application> user_application { get; set; }
        }
    }
}
