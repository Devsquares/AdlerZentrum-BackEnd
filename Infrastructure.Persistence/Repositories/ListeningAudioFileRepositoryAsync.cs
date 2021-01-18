using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class ListeningAudioFileRepositoryAsync : GenericRepositoryAsync<ListeningAudioFile>, IListeningAudioFileRepositoryAsync
    {
        private readonly DbSet<ListeningAudioFile> _listeningaudiofiles;


        public ListeningAudioFileRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _listeningaudiofiles = dbContext.Set<ListeningAudioFile>();

        }
    }

}
