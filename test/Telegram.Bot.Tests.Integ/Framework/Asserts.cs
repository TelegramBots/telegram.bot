using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Framework
{
    public static class Asserts
    {
        public static void JsonEquals(
            object expected,
            object actual,
            params string[] excludeFields
        )
        {
            if (expected is null || actual is null)
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                var expectedToken = JToken.FromObject(expected).RemoveFields(excludeFields);
                var actualToken = JToken.FromObject(actual).RemoveFields(excludeFields);
                bool equals = JToken.DeepEquals(expectedToken, actualToken);

                if (equals)
                {
                    Assert.True(equals);
                }
                else
                {
                    // Print out both JSON values in the case of an inequality
                    string expectedJson = JsonConvert.SerializeObject(expectedToken);
                    string actualJson = JsonConvert.SerializeObject(actualToken);
                    Assert.Equal(expectedJson, actualJson);
                }
            }
        }

        // getMe request returns more information than is present in received updates
        public static void UsersEqual(User expected, User actual)
        {
            JsonEquals(
                expected,
                actual,
                "can_join_groups",
                "can_read_all_group_messages",
                "supports_inline_queries"
            );
        }

        private static JToken RemoveFields(this JToken token, params string[] fields)
        {
            if (fields?.Length > 0)
            {
                _RemoveFields(token, fields);
            }

            return token;
        }

        private static void _RemoveFields(JToken token, string[] fields)
        {
            if (fields.Length > 0 && token is JContainer container)
            {
                var removeList = new List<JToken>();
                foreach (var el in container.Children())
                {
                    if (el is JProperty p && fields.Contains(p.Name))
                    {
                        removeList.Add(el);
                    }

                    _RemoveFields(el, fields);
                }

                foreach (var el in removeList)
                {
                    el.Remove();
                }
            }
        }
    }
}
