//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class release
    {
        public int id { get; set; }
        public int application_id { get; set; }
        public string environment_type { get; set; }
        public string version { get; set; }
        public int version_code { get; set; }
        public string notes { get; set; }
        public int icon_id { get; set; }
        public long size { get; set; }
        public bool force_update { get; set; }
        public bool @lock { get; set; }
    
        public virtual application application { get; set; }
        public virtual environment environment { get; set; }
        public virtual icon icon { get; set; }
    }
}
