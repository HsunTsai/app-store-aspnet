using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppStore.Models
{
    [MetadataType(typeof(user_applicationMetadata))]
    public partial class user_application
    {
        public class user_applicationMetadata
        {
            public int id { get; set; }

            /// <summary>
            /// 使用者ID
            /// </summary>
            public string user_id { get; set; }

            /// <summary>
            /// 應用程式
            /// </summary>
            public int application_id { get; set; }

            /// <summary>
            /// 使用者對於應用程式的權限
            /// </summary>
            public string role { get; set; }

            [JsonIgnore]
            public virtual application application { get; set; }
            [JsonIgnore]
            public virtual user user { get; set; }
            [JsonIgnore]
            public virtual user_application_role user_application_role { get; set; }
        }
    }
}