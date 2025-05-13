using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Contracts.IHelper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Helpers
{
    public class DropdownHelper : IDropdownHelper
    {
        #region Fields

        private readonly ITagApiClient _tagApiClient;

        #endregion

        #region Ctor

        public DropdownHelper(ITagApiClient tagApiClient)
        {
            _tagApiClient = tagApiClient;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<SelectListItem>> GetTagSelectItems(bool OnlyInUse = true)
        {
            var tags = await _tagApiClient.SendGetTagsInUseReq();
            return tags.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString(),
                Selected = false
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetCatgorySelectItems()
        {
            var categories = new[] { "Tech", "Business", "Health", "Education", "Entertainment" };
            return categories.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = false
            });
        }

        #endregion
    }
}
