using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    [MetadataType(typeof(releaseMetadata))]
    public partial class release
    {
        public class releaseMetadata
        {
            public int id { get; set; }

            /// <summary>
            /// 對應應用程式id
            /// </summary>
            [Display(Name = "對應應用程式id")]
            public int application_id { get; set; }

            /// <summary>
            /// 該應用程式所屬設備(Android iOS win32 win64)
            /// </summary>
            [Display(Name = "該應用程式所屬設備(Android iOS win32 win64)")]
            public string device_type { get; set; }

            /// <summary>
            /// 該釋出的版本所在的環境
            /// </summary>
            [Display(Name = "該釋出的版本所在的環境")]
            public string environment_type { get; set; }

            /// <summary>
            /// 版號(string)
            /// </summary>
            [Display(Name = "版號(string)")]
            public string version { get; set; }

            /// <summary>
            /// 版號(int)
            /// </summary>
            [Display(Name = "版號(int)")]
            public int version_code { get; set; }

            /// <summary>
            /// 更新版本訊息
            /// </summary>
            [Display(Name = "更新版本訊息")]
            public string notes { get; set; }

            /// <summary>
            /// app對應的icon
            /// </summary>
            [Display(Name = "app對應的icon")]
            public int icon_id { get; set; }

            /// <summary>
            /// 上傳的程序大小
            /// </summary>
            [Display(Name = "上傳的程序大小")]
            public long size { get; set; }

            /// <summary>
            /// 是否強制使用者更新
            /// </summary>
            [Display(Name = "是否強制使用者更新")]
            public bool force_update { get; set; }

            /// <summary>
            /// 開啟或關閉該版本
            /// </summary>
            [Display(Name = "開啟或關閉該版本")]
            public bool @lock { get; set; }

            /// <summary>
            /// 釋出版本的時間戳記
            /// </summary>
            [Display(Name = "釋出版本的時間戳記")]
            public long release_time { get; set; }

            [JsonIgnore]
            public virtual application application { get; set; }
            [JsonIgnore]
            public virtual environment environment { get; set; }
            [JsonIgnore]
            public virtual icon icon { get; set; }
        }
    }
}
