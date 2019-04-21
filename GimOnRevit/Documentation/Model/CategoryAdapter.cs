using System;
using Rvt = Autodesk.Revit.DB;

namespace Gim.Revit.Documentation.Model
{
    public class CategoryAdapter : Category
    {
        private readonly Rvt.Category category;
        public CategoryAdapter(Rvt.Category rvtCategory)
        {
            category = rvtCategory
                ?? throw new ArgumentException("Category is null");
        }

        public override string Name
        {
            get { return category.Name; }
            set { }
        }

        public override Category Parent
        {
            get
            {
                var parent = category.Parent;
                return parent is null ? null
                    : new CategoryAdapter(parent);
            }

            set { }
        }
    }
}
