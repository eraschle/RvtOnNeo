using System.Collections.Generic;

namespace Gim.Revit.Documentation.Wrapper
{
    public class CategoriesWrapper
    {
        public IList<CategoryWrapper> Categories { get; set; }

        public CategoriesWrapper(IList<CategoryWrapper> categories)
        {
            Categories = categories;
        }
    }
}
