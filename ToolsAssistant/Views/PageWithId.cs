using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ToolsAssistant.Views
{
    public abstract class PageWithId:Page
    {
        public string Id { get; set; }
    }
}
