using DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor
{
    public interface IAttachmentService
    {
        ResultViewModel<List<ReadAttachmentDto>> GetAll(QueryViewModel<ReadAttachmentDto> queryViewModel);
        Task<ResultViewModel<List<ReadAttachmentDto>>> Add(EditAttachmentDto attachmentDto);
        //Task<ResultViewModel<List<ReadAttachmentDto>>> Add(List<EditAttachmentDto> attachmentDto);
        ResultViewModel<ReadAttachmentDto> Delete(int id);
        ResultViewModel<AttachmentDto> Get(int id);

        //Task<ResultViewModel<string>> UploadProfileFile(ProfileFileAttachmentDTO attachmentDto);
       // Task<ResultViewModel<string>> DownloadProfileFile(string Filename);
    }
}
