﻿using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipShareAPI.Application.Dto.Notification;
using ShipShareAPI.Application.Dto.Review;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;

namespace ShipShareAPI.Persistence.Concretes.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly ILogger<ReviewRepository> _logger;
        private readonly INotificationRepository _notificationRepository;

        public ReviewRepository(ShipShareDbContext shipShareDbContext, IRequestUserProvider requestUserProvider, ILogger<ReviewRepository> logger, INotificationRepository notificationRepository)
        {
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
            _logger = Guard.Against.Null(logger);
            _notificationRepository = Guard.Against.Null(notificationRepository);
        }

        public async Task<ReviewDto> CreateReview(Guid userId,CreateReviewRequest createReviewRequest)
        {
            var user = _requestUserProvider.GetUserInfo();
            var createReview = createReviewRequest.Adapt<Review>();
            createReview.ReviewRecipientId = userId;
            createReview.ReviewSenderId = user!.Id;
            var newReview = (await _shipShareDbContext.Reviews.AddAsync(createReview)).Entity;
            await _shipShareDbContext.SaveChangesAsync();
            _logger.LogInformation($"{user.Id} created review {newReview.Id}");
            var reviewDto = newReview.Adapt<ReviewDto>();
            reviewDto.Username = user.Name;
            var newNotification = new CreateNotificationRequest()
            {
               UserId = user!.Id,
               Title = "New Review!",
               Description = $"{user.Name} added new review to your profile!"
            };
            await _notificationRepository.CreateNotification(newNotification);
            return reviewDto;
        }

        public async Task<bool> DeleteReview(Guid reviewId)
        {
            var user = _requestUserProvider.GetUserInfo();
            var review = await _shipShareDbContext.Reviews.Include(r => r.ReviewRecipient).FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review is not null)
            {
                if (review.ReviewSenderId == user!.Id || review.ReviewRecipientId == user!.Id || user!.Role == "Admin")
                {
                    _shipShareDbContext.Reviews.Remove(review);
                    await _shipShareDbContext.SaveChangesAsync();
                    _logger.LogInformation($"{user!.Id} deleted review {review.Id}");
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _shipShareDbContext.Reviews.Include(r => r.ReviewSender).Where(p => p.IsConfirmed).ToListAsync();
        }

        public async Task<List<Review>> GetAllReviewsAdmin()
        {
            return await _shipShareDbContext.Reviews.Include(r => r.ReviewSender).Include(r => r.ReviewRecipient).Where(p => p.IsDeclined == false).ToListAsync();
        }

        public async Task<List<Review>> GetUserReviews(Guid userId)
        {
            return await _shipShareDbContext.Reviews.Include(r => r.ReviewSender).Where(r => r.ReviewRecipientId == userId && r.IsConfirmed).ToListAsync();
        }

        public async Task<bool> SetStatusReview(Guid reviewId, bool status)
        {
            var review = await _shipShareDbContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review is not null)
            {
                if (!status)
                    review.IsDeclined = true;
                review.IsConfirmed = status;
                _shipShareDbContext.Reviews.Update(review);
                await _shipShareDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReviewDto> UpdateReview(Guid reviewId, UpdateReviewRequest updateReviewRequest)
        {
            var user = _requestUserProvider.GetUserInfo();
            var review = await _shipShareDbContext.Reviews.Include(r => r.ReviewRecipient).FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review is not null)
            {
                if (review.ReviewSenderId == user!.Id || user.Role == "Admin")
                {
                    review.Text = updateReviewRequest.Text;
                    review.Rating = updateReviewRequest.Rating;
                    _shipShareDbContext.Reviews.Update(review);
                    await _shipShareDbContext.SaveChangesAsync();
                    _logger.LogInformation($"{user!.Id} updated review {review.Id}");
                }
                else
                    throw new Exception("404 not found");
            }
            throw new Exception("404 not found");
        }

    }
}
