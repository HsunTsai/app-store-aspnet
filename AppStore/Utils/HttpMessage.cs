using Newtonsoft.Json.Linq;

namespace AppStore.Utils
{
    public class HttpMessage
    {
        JObject message = new JObject();

        public HttpMessage(string message_id)
        {
            message.Add("message", message_id);
        }

        public string toString()
        {
            return  message.ToString();
        }
    }
}