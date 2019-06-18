using Newtonsoft.Json.Linq;

namespace AppStore.Utils
{
    public class HttpMessage
    {
        JObject message = new JObject();
        string message_id;

        public HttpMessage(string message_id)
        {
            this.message_id = message_id;
            message.Add("Message", message_id);
        }

        public JObject toJson()
        {
            return message;
        }
    }
}