using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Contracts.IHelper
{
    public interface IDropdownHelper
    {
        Task<IEnumerable<SelectListItem>> GetTagSelectItems(bool OnlyInUse = true);
        Task<IEnumerable<SelectListItem>> GetCatgorySelectItems(bool OnlyInUse = true);
    }
}
