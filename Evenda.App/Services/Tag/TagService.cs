using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.ITag;
using Evenda.App.Dtos.Tag;
using Evenda.App.Models;
using Evenda.Services.Services.Base;
using TagEntity = Evenda.Domain.Entities.TagEntities.Tag;

namespace Evenda.App.Services.Tag
{
    public class TagService : BaseService, ITagService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<TagEntity> _tagRepo;

        #endregion

        #region Ctor

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tagRepo = unitOfWork.GetRepository<TagEntity>();
        }

        #endregion

        #region Methods

        public async Task<DataResponse<IList<TagDto>>> GetTags(bool inUse = true)
        {
            var tags = await _tagRepo.FindAsync(
                predicate: x => !x.IsDeleted && (inUse ? x.Events.Count > 0 : true),
                include: null,
                orderBy: x => x.Events.Count,
                thenOrderBy: null,
                orderByDescending: true
            );

            IList<TagDto> tagDtos = new List<TagDto>();
            foreach (var tag in tags)
            {
                int cnt = await _unitOfWork.GetRepository<Domain.Entities.EventEntities.Event>()
                    .CountAsync(e => e.Tags.Contains(tag));

                tagDtos.Add(new TagDto(tag, cnt));
            }

            return Success(tagDtos);
        }

        public async Task<IList<TagEntity>> ProcessTagsAsStrings(IList<string> tags, bool returnOnlyNewTags = false, bool persistNewTags = true)
        {
            var eventTags = new List<TagEntity>();

            foreach (var tag in tags)
            {
                if (Guid.TryParse(tag, out var tagId) || await _tagRepo.Exists(x => x.Name.ToLower() == tag.ToLower()))
                {
                    if (returnOnlyNewTags) continue;

                    var existingTag = await _tagRepo.FirstOrDefaultAsync(x => x.Id == tagId
                        || x.Name.ToLower() == tag.ToLower());
                    if (existingTag != null)
                    {
                        eventTags.Add(existingTag);
                    }
                    continue;
                }

                var newTag = new TagEntity { Name = tag };

                if (persistNewTags)
                    await _tagRepo.AddAsync(newTag);

                eventTags.Add(newTag);
            }

            return eventTags;
        }

        #endregion
    }
}
