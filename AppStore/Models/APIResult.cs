using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppStore.Models
{
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public bool success { get; set; } = true;
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string message { get; set; } = "";
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object payload { get; set; }
    }
}