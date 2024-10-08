﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Repositories
{
    public class AboutRepository : IAboutRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AboutRepository> _logger;

        public AboutRepository(ApplicationDbContext context, ILogger<AboutRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InstituteInfo?> GetInstituteInfosAsync()
        {
            try
            {
                return await _context.InstituteInfo.FindAsync("1");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HomePageArticlesVideo?> GetHomePageArticlesVideo(string Id)
        {
            try
            {
                return await _context.HomePageArticlesVideos.FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<HomePageArticlesVideo>> GetHomePageArticlesVideosAsync()
        {
            try
            {
                return await Task.FromResult(_context.Set<HomePageArticlesVideo>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateInstituteInfoAsync(InstituteInfo model)
        {
            try
            {
                var info = await GetInstituteInfosAsync();
                if (info == null) return false;

                info.Address = model.Address;
                info.Region = model.Region;
                info.Province = model.Province;
                info.District = model.District;
                info.City = model.City;
                info.TelephoneNo = model.TelephoneNo;
                info.Email = model.Email;
                info.FocalPerson = model.FocalPerson;
                info.ProviderType = model.ProviderType;
                info.ProviderClassification = model.ProviderClassification;

                await Task.FromResult(_context.InstituteInfo.Update(info));
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateHomePageArticlesVideoAsync(HomePageArticlesVideo model)
        {
            try
            {
                _context.HomePageArticlesVideos.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteHomePageArticlesVideoAsync(string Id)
        {
            try
            {
                var model = await _context.HomePageArticlesVideos.FindAsync(Id) ?? throw new NullReferenceException("Article or Video not found");
                model.Title = null;
                model.Description = null;
                model.FilePath = null;
                _context.HomePageArticlesVideos.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
