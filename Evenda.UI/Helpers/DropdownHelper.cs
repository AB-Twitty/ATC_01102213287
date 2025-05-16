using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Contracts.IHelper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Helpers
{
    public class DropdownHelper : IDropdownHelper
    {
        #region Fields

        private readonly ITagApiClient _tagApiClient;
        private readonly IEventApiCLient _eventApiClient;

        #endregion

        #region Ctor

        public DropdownHelper(ITagApiClient tagApiClient, IEventApiCLient eventApiClient)
        {
            _tagApiClient = tagApiClient;
            _eventApiClient = eventApiClient;
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

        public async Task<IEnumerable<SelectListItem>> GetCatgorySelectItems(bool OnlyInUse = true)
        {
            var categories = await _eventApiClient.GetCategories(OnlyInUse);
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
