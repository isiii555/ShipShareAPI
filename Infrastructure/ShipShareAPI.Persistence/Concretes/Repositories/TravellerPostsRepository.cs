﻿using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipShareAPI.Application.Dto.Post.TravellerPost;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class TravellerPostsRepository : ITravellerPostsRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly ILogger<TravellerPostsRepository> _logger;

        public TravellerPostsRepository(ShipShareDbContext dbContext, IRequestUserProvider userProvider, ILogger<TravellerPostsRepository> logger)
        {
            _shipShareDbContext = Guard.Against.Null(dbContext);
            _requestUserProvider = Guard.Against.Null(userProvider);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<TravellerPostDto> CreatePost(CreateTravellerPostRequest createTravellerPostRequest)
        {
            var post = createTravellerPostRequest.Adapt<TravellerPost>();
            post.UserId = _requestUserProvider.GetUserInfo()!.Id;
            var entity = await _shipShareDbContext.TravellerPosts.AddAsync(post);
            await _shipShareDbContext.SaveChangesAsync();
            _logger.LogInformation($"{post.UserId} user created post {post.Id}!");
            var travellerPostDto = entity.Entity.Adapt<TravellerPostDto>();
            return travellerPostDto;
        }

        public async Task<bool> DeletePost(Guid postId)
        {
            var userId = _requestUserProvider?.GetUserInfo()!.Id;
            var post = await _shipShareDbContext.TravellerPosts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is not null)
            {
                if (userId == post.UserId)
                {
                    _shipShareDbContext.TravellerPosts.Remove(post);
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

        public async Task<List<TravellerPost>> GetAllPosts()
        {
            return await _shipShareDbContext.TravellerPosts.ToListAsync();
        }

        public async Task<List<TravellerPost>> GetUserTravellerPosts()
        {
            var userId = _requestUserProvider.GetUserInfo()!.Id;
            return await _shipShareDbContext.TravellerPosts.Where(t => t.UserId ==  userId).ToListAsync();
        }

        public async Task<TravellerPostDto?> UpdatePost(Guid postId, UpdateTravellerPostRequest updateTravellerPostRequest)
        {
            var userId = _requestUserProvider?.GetUserInfo()!.Id;
            var post = _shipShareDbContext.TravellerPosts.FirstOrDefault(p => p.Id == postId);

            if (post is not null)
            {
                if (post.UserId == userId)
                {
                    post.StartDestination = updateTravellerPostRequest.StartDestination;
                    post.DeadlineDate = updateTravellerPostRequest.DeadlineDate;
                    post.Price = updateTravellerPostRequest.Price;
                    post.EndDestination = updateTravellerPostRequest.EndDestination;
                    post.Title = updateTravellerPostRequest.Title;
                    post.Description = updateTravellerPostRequest.Description;
                    var entity = _shipShareDbContext.TravellerPosts.Update(post);
                    await _shipShareDbContext.SaveChangesAsync();
                    _logger.LogInformation($"{post.UserId} user updated post {post.Id}!");
                    var travellerPostDto = entity.Entity.Adapt<TravellerPostDto>();
                    return travellerPostDto;
                }
                throw new Exception("Error 404 not found!");
            }
            throw new Exception("Error 404 not found!");
        }
    }
}
