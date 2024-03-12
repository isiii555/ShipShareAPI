using ShipShareAPI.Application.Dto.Post.SenderPost;
using ShipShareAPI.Domain.Entities;


namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface ISenderPostRepository
    {
        Task<List<SenderPost>> GetAllPosts();
        Task<List<SenderPost>> GetAllPostsAdmin();
        Task<SenderPostDto> CreatePost(CreateSenderPostRequest createSenderPostRequest);
        Task<SenderPostDto?> UpdatePost(Guid postId,UpdateSenderPostRequest updateSenderPostRequest);
        Task<bool> SetStatusSenderPost(Guid postId, bool status);
        Task<bool> DeletePost(Guid postId);
        Task<List<SenderPost>> GetUserSenderPosts();
    }
}
