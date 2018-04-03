using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Workwiz.PaymentFramework.Shared.RealexApi.RealVault
{
    [Serializable]
    public class RealexComment
    {
        [XmlAttribute("id")]
        public int id;

        [XmlText]
        public string Value;
        
        public static List<RealexComment> CreateCommentList(IEnumerable<string> optionalComments, int maxNumberOfComments = 2)
        {
            if (null == optionalComments)
            {
                return null;
            }
            List<RealexComment> result = new List<RealexComment>();
            foreach (string comment in optionalComments)
            {
                if (null != comment)
                {
                    string sanitizedComment = MessageContentUtility.TruncateAndStripDisallowed(comment, truncateTo: 255,
                        disallowedCharacters: RealexFields.RealexFieldCommentDisallowRegex);

                    result.Add(new RealexComment()
                    {
                        id = result.Count + 1,
                        Value = sanitizedComment
                    });
                    if (result.Count >= 2)
                    {
                        break;
                    }
                }
            }
            return result.Any() ? result : null;
        }
    }
}
