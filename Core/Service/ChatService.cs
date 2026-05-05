using AutoMapper;
using Domain.Entites.ChatModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Service.Abstraction;
using Service.Specifications.ChatSpecifications;
using Service.Specifications.TreatmentRequestSpecificaition;
using Shared.CommonResult;
using Shared.DTOs.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
        public class ChatService : IChatService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<IEnumerable<ChatMessageResponseDTO>>> GetChatHistoryAsync(int requestId, string identityUserId)
            {
                var request = await _unitOfWork.GetRepository<TreatmentRequest,int>()
                    .GetByIdAsync(new TreatmentRequestByIdSpecification(requestId));

            if (request is null)
                return Error.NotFound("Request.Notfound");

                if (request.Status != TreatmentRequestStatus.Accepted)
                    return  Error.NotFound("Request.NotAccepted");

                var isPatient = request.Case.Patient.IdentityUserId == identityUserId;
                var isStudent = request.Student.IdentityUserId == identityUserId;

                if (!isPatient && !isStudent)
                    return Error.Unauthorized("Request.Unauthorized");

                var messages = await _unitOfWork.GetRepository<ChatMessage,int>()
                    .GetAllAsync(new ChatMessagesByRequestSpecification(requestId));

                var result = _mapper.Map<IEnumerable<ChatMessageResponseDTO>>(messages);

                return Result<IEnumerable<ChatMessageResponseDTO>>.Ok(result);
            }
        }
}
