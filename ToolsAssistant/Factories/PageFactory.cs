using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsAssistant.Views;

namespace ToolsAssistant.Factories
{
    public class PageFactory<T> where T : PageWithId,new()
    {
        private static List<T> _pages = new List<T>();

        public static T GetPage(string Id)
        {
            var page = _pages.Find(x => x.Id == Id);
            if(page == null)
            {
                page = new T();
                _pages.Add(page);
            }

            return page;
        }
    }
}
