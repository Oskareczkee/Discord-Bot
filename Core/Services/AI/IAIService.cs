using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.AI
{
    public interface IAIService
    {
        Task<string> GenerateContent(string message);
    }
}
