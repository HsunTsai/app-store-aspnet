using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(trackingMetadata))]
    public partial class tracking
    {
        public class trackingMetadata
        {
            public int id { get; set; }
            
            /// <summary>
            /// 應用程式id
            /// </summary>
            [Display(Name = "應用程式id")]
            public int application_id { get; set; }

            /// <summary>
            /// 環境種類
            /// </summary>
            [Display(Name = "環境種類")]
            public string environment_type { get; set; }

            /// <summary>
            /// 設備種類
            /// </summary>
            [Display(Name = "設備種類")]
            public string device_type { get; set; }

            /// <summary>
            /// 設備id
            /// </summary>
            [Display(Name = "設備id")]
            public string device_id { get; set; }

            /// <summary>
            /// 使用者id
            /// </summary>
            [Display(Name = "使用者id")]
            public string user_id { get; set; }

            /// <summary>
            /// 行為id
            /// </summary>
            [Display(Name = "行為id")]
            public Nullable<int> action_id { get; set; }

            /// <summary>
            /// 參數
            /// </summary>
            [Display(Name = "參數")]
            public string param { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            [Display(Name = "描述")]
            public string detail { get; set; }

            /// <summary>
            /// 時間戳記
            /// </summary>
            [Display(Name = "時間戳記")]
            public long time_stamp { get; set; }

            [JsonIgnore]
            public virtual application application { get; set; }
            [JsonIgnore]
            public virtual device device { get; set; }
            [JsonIgnore]
            public virtual environment environment { get; set; }
        }
    }
}
