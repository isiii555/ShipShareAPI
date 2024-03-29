using ShipShareAPI.Application.Dto.Post.TravellerPost;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Repositories
{
    public interface ITravellerPostRepository
    {
        Task<List<TravellerPost>> GetAllPosts();
        Task<List<TravellerPost>> GetAllPostsAdmin();
        Task<TravellerPostDto> CreatePost(CreateTravellerPostRequest createTravellerPostRequest);
        Task<TravellerPostDto?> UpdatePost(Guid postId, UpdateTravellerPostRequest updateTravellerPostRequest);
        Task<bool> SetStatusTravellerPost(Guid postId, bool status);
        Task<bool> DeletePost(Guid postId);
        Task<List<TravellerPost>> GetUserTravellerPosts();
        Task<bool> IncreasePostView(Guid postId);
    }
}
