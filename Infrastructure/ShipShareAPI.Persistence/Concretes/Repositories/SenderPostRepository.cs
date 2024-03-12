using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipShareAPI.Application.Dto.Post.SenderPost;
using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class SenderPostsRepository : ISenderPostRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IUploadImageToStorageService _uploadImageToStorageService;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly ILogger<SenderPostsRepository> _logger;

        public SenderPostsRepository(ShipShareDbContext shipShareDbContext, IUploadImageToStorageService uploadImageToStorageService, IRequestUserProvider requestUserProvider, ILogger<SenderPostsRepository> logger)
        {
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _uploadImageToStorageService = Guard.Against.Null(uploadImageToStorageService);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<SenderPostDto> CreatePost(CreateSenderPostRequest createSenderPostRequest)
        {
            var post = createSenderPostRequest.Adapt<SenderPost>();
            post.UserId = _requestUserProvider.GetUserInfo()!.Id;
            if (post.ItemPhotos is not null)
                post!.ItemPhotos!.Clear();
            if (createSenderPostRequest.ItemPhotos is not null)
            {
                foreach (var formfile in createSenderPostRequest!.ItemPhotos!)
                {
                    var imageUrl = _uploadImageToStorageService.UploadImageToStorage(formfile);
                    post!.ItemPhotos!.Add(imageUrl);
                }
            }
            var entity = await _shipShareDbContext.SenderPosts.AddAsync(post);
            await _shipShareDbContext.SaveChangesAsync();
            _logger.LogInformation($"{post.UserId} user created post {post.Id}!");
            var senderPostDto = entity.Entity.Adapt<SenderPostDto>();
            return senderPostDto;
        }

        public async Task<bool> DeletePost(Guid postId)
        {
            var user = _requestUserProvider?.GetUserInfo();
            var post = await _shipShareDbContext.SenderPosts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is not null)
            {
                if (user!.Id == post.UserId || user.Role == "Admin")
                {
                    _shipShareDbContext.SenderPosts.Remove(post);
                    await _shipShareDbContext.SaveChangesAsync();
                    _logger.LogInformation($"{post.UserId} user deleted post {post.Id}!");
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public async Task<List<SenderPost>> GetAllPosts()
        {
            return await _shipShareDbContext.SenderPosts.ToListAsync();
            //return await _shipShareDbContext.SenderPosts.Where(p => p.IsConfirmed).ToListAsync();
        }

        public async Task<List<SenderPost>> GetAllPostsAdmin()
        {
            return await _shipShareDbContext.SenderPosts.Where(p => p.IsDeclined == false).ToListAsync();
        }

        public async Task<List<SenderPost>> GetUserSenderPosts()
        {
            var userId = _requestUserProvider?.GetUserInfo()!.Id;
            return await _shipShareDbContext.SenderPosts.Where(s => s.UserId == userId && s.IsConfirmed).ToListAsync();
        }

        public async Task<bool> SetStatusSenderPost(Guid postId, bool status)
        {
            var post = await _shipShareDbContext.SenderPosts.FirstOrDefaultAsync(s => s.Id == postId);
            if (post is not null)
            {
                if (!status)
                    post.IsDeclined = true;
                post.IsConfirmed = status;
                _shipShareDbContext.SenderPosts.Update(post);
                await _shipShareDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<SenderPostDto?> UpdatePost(Guid postId, UpdateSenderPostRequest updateSenderPostRequest)
        {
            var user = _requestUserProvider?.GetUserInfo();
            var post = _shipShareDbContext.SenderPosts.FirstOrDefault(p => p.Id == postId);

            if (post is not null)
            {
                if (post.UserId == user!.Id || user.Role == "Admin")
                {
                    post.StartDestination = updateSenderPostRequest.StartDestination;
                    post.DeadlineDate = updateSenderPostRequest.DeadlineDate;
                    post.ItemType = updateSenderPostRequest.ItemType;
                    post.Price = updateSenderPostRequest.Price;
                    post.ItemWeight = updateSenderPostRequest.ItemWeight;
                    post.EndDestination = updateSenderPostRequest.EndDestination;
                    post.Title = updateSenderPostRequest.Title;
                    post.Description = updateSenderPostRequest.Description;
                    if (updateSenderPostRequest.ItemPhotos is not null)
                    {
                        post!.ItemPhotos!.Clear();
                        foreach (var photo in updateSenderPostRequest.ItemPhotos)
                        {
                            post.ItemPhotos.Add(_uploadImageToStorageService.UploadImageToStorage(photo));
                        }
                    }
                    var entity = _shipShareDbContext.SenderPosts.Update(post);
                    await _shipShareDbContext.SaveChangesAsync();
                    _logger.LogInformation($"{post.UserId} user updated post {post.Id}!");
                    var senderPostDto = entity.Entity.Adapt<SenderPostDto>();
                    return senderPostDto;
                }
                throw new Exception("Error 404 not found!");
            }
            throw new Exception("Error 404 not found!");
        }
    }
}
