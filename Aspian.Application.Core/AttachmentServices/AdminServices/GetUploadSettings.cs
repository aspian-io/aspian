using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using MediatR;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class GetUploadSettings
    {
        public class Query : IRequest<AttachmentUploadSettingsDto> { }

        public class Handler : IRequestHandler<Query, AttachmentUploadSettingsDto>
        {
            private readonly DataContext _context;
            private readonly IOptionAccessor _optionAccessor;
            public Handler(DataContext context, IOptionAccessor optionAccessor)
            {
                _optionAccessor = optionAccessor;
                _context = context;
            }

            public async Task<AttachmentUploadSettingsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var settings = await _optionAccessor.GetOptionValuesBySectionAsync(SectionEnum.AdminAttachment, SiteTypeEnum.Blog);
                var isAutoProceedAllowed = settings.SingleOrDefault(x => x.Key == KeyEnum.Attachment__Upload_Auto_Proceed).Value == ValueEnum.Attachment__Allowed;
                var isMultipleUploadAllowed = settings.SingleOrDefault(x => x.Key == KeyEnum.Attachment__Multiple_Upload).Value == ValueEnum.Attachment__Allowed;
                var minAllowedNumberOfFiles = Convert.ToInt32(settings.SingleOrDefault(x => x.Key == KeyEnum.Attachment__Upload_Min_NumberOfFile).ValueDescription);
                var maxAllowedNumberOfFiles = Convert.ToInt32(settings.SingleOrDefault(x => x.Key == KeyEnum.Attachment__Upload_Max_NumberOfFile).ValueDescription);
                var maxAllowedFileSize = Convert.ToInt64(settings.SingleOrDefault(x => x.Key == KeyEnum.Attachment__Upload_Max_FileSize).ValueDescription);

                var fileTypes = await _optionAccessor.GetOptionValuesBySectionAsync(SectionEnum.AdminAttachmentFileTypes, SiteTypeEnum.Blog);
                var allowedFileTypes = fileTypes.Where(ft => ft.Value == ValueEnum.AttachmentFileType__Allowed).Select(x => x.KeyDescription).ToList();

                return new AttachmentUploadSettingsDto
                {
                    UploadAllowedFileTypes = allowedFileTypes,
                    IsAutoProceedUploadAllowed = isAutoProceedAllowed,
                    IsMultipleUploadAllowed = isMultipleUploadAllowed,
                    UploadMinAllowedNumberOfFiles = minAllowedNumberOfFiles,
                    UploadMaxAllowedNumberOfFiles = maxAllowedNumberOfFiles,
                    UploadMaxAllowedFileSize = maxAllowedFileSize
                };
            }
        }
    }
}