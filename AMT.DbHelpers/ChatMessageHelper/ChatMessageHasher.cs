using System.Text;

namespace AMT.DbHelpers.ChatMessageHelper
{
    public class ChatMessageHasher
    {
        // function to encode message
        public static string EncodeMessage(string message)
        {
            // encode message
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
            return encodedString;
        }

        // function to decode message
        public static string DecodeMessage(string encodedString)
        {
            // decode message
            var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
            return decodedString;
        }
    }
}
