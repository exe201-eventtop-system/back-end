using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces.ApiCaller
{
    public interface IApiEndpointCaller
    {
        Task<TResult?> GetAsync<TResult>(string uri);

        Task<TResult?> PostAsync<TResult>(string uri, object body);

        Task<TResult?> PutAsync<TResult>(string uri, object body);

        Task<TResult?> DeleteAsync<TResult>(string uri);
    }
}
