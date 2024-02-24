﻿using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Post;
using ShipShareAPI.Application.Dto.Review;
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
    public class SenderPostsRepository : ISenderPostsRepository
    {
        private readonly ShipShareDbContext _shipShareDbContext;
        private readonly IUploadImageToStorageService _uploadImageToStorageService;

        public SenderPostsRepository(ShipShareDbContext shipShareDbContext, IUploadImageToStorageService uploadImageToStorageService)
        {
            _shipShareDbContext = Guard.Against.Null(shipShareDbContext);
            _uploadImageToStorageService = Guard.Against.Null(uploadImageToStorageService);
        }

        public async Task<SenderPostDto> CreatePost(CreateSenderPostRequest createSenderPostRequest)
        {
            var post = createSenderPostRequest.Adapt<SenderPost>();
            post!.ItemPhotos!.Clear();
            foreach (var formfile in createSenderPostRequest!.ItemPhotos!)
            {
                var imageUrl = _uploadImageToStorageService.UploadImageToStorage(formfile);
                post!.ItemPhotos!.Add(imageUrl);
            }
            var entity = await _shipShareDbContext.SenderPosts.AddAsync(post);
            await _shipShareDbContext.SaveChangesAsync();

            var senderPostDto = entity.Entity.Adapt<SenderPostDto>();
            return senderPostDto;
        }

        public async Task<bool> DeletePost(Guid postId)
        {
            var post = await _shipShareDbContext.SenderPosts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is not null)
            {
                _shipShareDbContext.SenderPosts.Remove(post);
                return true;
            }
            else
                return false;
        }

        public async Task<List<SenderPost>> GetAllPosts()
        {
            return await _shipShareDbContext.SenderPosts.ToListAsync();
        }

        public async Task<SenderPostDto?> UpdatePost(Guid postId, UpdateSenderPostRequest updateSenderPostRequest)
        {
            var post = _shipShareDbContext.SenderPosts.FirstOrDefault(p => p.Id == postId);
            if (post is not null)
            {
                post.StartDestination = updateSenderPostRequest.StartDestination;
                post.DeadlineDate = updateSenderPostRequest.DeadlineDate;
                post.ItemType = updateSenderPostRequest.ItemType;
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
                var senderPostDto = entity.Entity.Adapt<SenderPostDto>();
                return senderPostDto;
            }
            return null;
        }
    }
}