using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.ITag;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.API.Controllers
{
    public class TagsController : DefaultController
    {
        #region Fields

        private readonly ITagService _tagService;

        #endregion

        #region Ctor

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        #endregion

        #region Actions

        [HttpGet()]
        public async Task<IActionResult> GetTags([FromQuery(Name = "in-use")] bool inUse = true)
        {
            var response = await _tagService.GetTags(inUse);
            return HandleResponse(response);
        }

        #endregion
    }
}
