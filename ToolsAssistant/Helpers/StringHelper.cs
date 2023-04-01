using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsAssistant.Helpers
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 按长度分割字符串
        /// </summary>
        /// <param name="input">要分割的字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static List<string> Split(this string input, int length)
        {
            var count = (int)Math.Ceiling((double)input.Length/(double)length);
            var result = new List<string>();
            for(int i = 0; i < count; i++)
            {
                if(i == count - 1)
                {
                    result.Add(input.Substring(i * length));
                }else
                {
                    result.Add(input.Substring(i * length, length));
                }
            }

            return result;
        }
    }
}
