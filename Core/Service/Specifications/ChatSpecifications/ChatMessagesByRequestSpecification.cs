using Domain.Entites.ChatModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.ChatSpecifications
{
    public class ChatMessagesByRequestSpecification : BaseSpecification<ChatMessage,int>
    {
        public ChatMessagesByRequestSpecification(int requestId)
            : base(m => m.TreatmentRequestId == requestId)
        {
            AddOrderBy(m => m.CreatedAt);
        }
    }
}
