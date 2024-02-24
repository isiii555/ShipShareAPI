using ShipShareAPI.Application.Dto.Post;
using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface ISenderPostsRepository
    {
        Task<List<SenderPost>> GetAllPosts();
        Task<SenderPostDto> CreatePost(CreateSenderPostRequest createSenderPostRequest);
        Task<SenderPostDto?> UpdatePost(Guid postId,UpdateSenderPostRequest updateSenderPostRequest);
        Task<bool> DeletePost(Guid postId);
    }
}
