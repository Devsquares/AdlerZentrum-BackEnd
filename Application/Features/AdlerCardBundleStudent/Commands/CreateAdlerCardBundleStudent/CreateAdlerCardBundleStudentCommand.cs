using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdlerCardBundleStudent.Commands.CreateAdlerCardBundleStudent
{
    public partial class CreateAdlerCardBundleStudentCommand : IRequest<Response<int>>
    {
		public string StudentId { get; set; }
		//public ApplicationUser Student { get; set; }
		public int AdlerCardsBundleId { get; set; }
		//public AdlerCardsBundle AdlerCardsBundle { get; set; }
		//public DateTime PurchasingDate { get; set; }
    }

    public class CreateAdlerCardBundleStudentCommandHandler : IRequestHandler<CreateAdlerCardBundleStudentCommand, Response<int>>
    {
        private readonly IAdlerCardBundleStudentRepositoryAsync _adlercardbundlestudentRepository;
        private readonly IAdlerCardsBundleRepositoryAsync _adlerCardsBundleRepositoryAsync;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public CreateAdlerCardBundleStudentCommandHandler(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository, IMapper mapper,
            UserManager<ApplicationUser> userManager, IAdlerCardsBundleRepositoryAsync adlerCardsBundleRepositoryAsync, IAccountService accountService)
        {
            _adlercardbundlestudentRepository = adlercardbundlestudentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _adlerCardsBundleRepositoryAsync = adlerCardsBundleRepositoryAsync;
            _accountService = accountService;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardBundleStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _userManager.FindByIdAsync(request.StudentId).Result;
            if(student == null)
            {
                throw new ApiException("Student Not Found");
            }
            var bundle = _adlerCardsBundleRepositoryAsync.GetByIdAsync(request.AdlerCardsBundleId).Result;
            if (bundle == null)
            {
                throw new ApiException("bundle Not Found");
            }
            
            var adlercardbundlestudent = _mapper.Map<Domain.Entities.AdlerCardBundleStudent>(request);
            adlercardbundlestudent.PurchasingDate = DateTime.Now;
            await _adlercardbundlestudentRepository.AddAsync(adlercardbundlestudent);
            await _accountService.UpdateAdlerCardBalance(request.StudentId,bundle.Count);
            return new Response<int>(adlercardbundlestudent.Id);
        }
    }
}
