using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerDIA.Utils
{
    public class PaneListItemTemplate
    {
        public Type ModelType { get; }
        public string Label { get; }
        public string Icon { get; }
        public PaneListItemTemplate(Type type,string icon)
        {
            ModelType = type;
            Label = type.Name.Replace("ViewModel","");
            Icon = icon ;
        }
    }
}
