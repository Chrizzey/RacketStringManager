using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidX.Annotations;
using RacketStringManager.Model;

namespace RacketStringManager.Services
{
    public interface IUiService
    {
        Task GotoPageAsync(string uri);
        
        Task GotoPageAsync(string uri, Job job);

        Task GoBackAsync();

        Task<bool> GetUserConfirmation(string title, string message, string accept, string cancel);

        Task DisplayAlertAsync(string title, string message, string cancel);
    }
}
