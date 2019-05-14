using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gim.Revit.Helper
{
    class ViewHelper
    {
        public static View3D Default3D(Document document)
        {
            var coll = new FilteredElementCollector(document)
                .OfClass(typeof(View3D));
            return coll.FirstElement() as View3D;
        }
    }
}
