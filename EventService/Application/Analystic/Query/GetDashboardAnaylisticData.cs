using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Commons.UoW;
using SharedLibrary.DTOs.Blog;
using SharedLibrary.DTOs.Service;
using SharedLibrary.DTOs.User;
using SharedLibrary.Enum;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Analystic.Query
{
    public class GetAnalysticQuery
    {
        public int Year { get; set; }
        public string? UserToken { get; set; }
    }

    public class TopService
    {
        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("service_name")]
        public string ServiceName { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("supplier_name")]
        public string SupplierName { get; set; }

        [JsonPropertyName("uses_count")]
        public int ServiceUseCount { get; set; }
    }

    public class AnalysitcData
    {
        [JsonPropertyName("request_time")]
        public DateTime RequestDate { get; set; }

        [JsonPropertyName("lifetime")]
        public GeneralAnalysticData LifeTimeData { get; set; }

        [JsonPropertyName("yearly")]
        public YearlyGeneralAnalysticData YearlyData { get; set; }

        [JsonPropertyName("most_rated_service")]
        public List<TopService> TopRatingServices { get; set; }

        [JsonPropertyName("least_rated_service")]
        public List<TopService> LeastRatingServices { get; set; }

        //[JsonPropertyName("trending_service")]
        //public List<TopService> TrendingServices { get; set; }
    }

    public class GeneralAnalysticData
    {
        [JsonPropertyName("used_service_count")]
        public int TotalUsedServiceCount { get; set; }

        [JsonPropertyName("service_count")]
        public int TotalServiceCount { get; set; }

        [JsonPropertyName("event_count")]
        public int TotalEventCompleted {  get; set; }

        [JsonPropertyName("customer_count")]
        public int TotalCustomerCount { get; set; }

        [JsonPropertyName("supplier_count")]
        public int TotalSupplierCount { get; set; }

        [JsonPropertyName("blog_count")]
        public int TotalBlogCount { get; set; }

        [JsonPropertyName("lifetime_revenue")]
        public decimal TotalRevenue { get; set; }
    }

    public class YearlyGeneralAnalysticData
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("data")]
        public GeneralAnalysticData Data { get; set; }

        [JsonPropertyName("data_month")]
        public List<MonthlyGeneralAnalysticData> MonthlyData { get; set; }
    }

    public class MonthlyGeneralAnalysticData
    {
        [JsonPropertyName("month")]
        public int Month {  get; set; }

        [JsonPropertyName("data")]
        public GeneralAnalysticData Data { get; set; }
    }

    public class GetAnalysticDataQueryHandler : IQueryHandler<GetAnalysticQuery, Result<AnalysitcData>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JwtService jwtService;

        public GetAnalysticDataQueryHandler(IUnitOfWork unitOfWork, JwtService jwtService, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<AnalysitcData>> Handle(GetAnalysticQuery query, CancellationToken cancellationToken)
        {
            //if (query.UserToken == null) 
            //{
            //    return Result<AnalysitcData>
            //        .Failure(Error.UnauthenticatedError("You must provide security credential to view this data"), "Failed while trying to process user request");
            //}

            // We need a method to extract user role from the given token.

            /*if (jwtService.ExtractUserRoleFromToken(query.UserToken) != UserRole.Admin.ToString())
            {
                return Result<AnalysitcData>
                    .Failure(Error.UnauthorizedError("You dont have permission"), "Failed while trying to process user request");
            }*/


            // Idk man, just query the whole database into the memory.
            var orders_list = await unitOfWork.UsedServiceRepository.GetAllAsync();
            var events_list = await unitOfWork.EventRepository.GetAllAsync();
            var transaction_list = await unitOfWork.TransactionRepository.GetAllAsync();
            var feedback_list = await unitOfWork.FeedbackRepository.GetAllAsync();

            // Also just get all informations you could possibly need lol,
            List<MinimalUserInfo> minimalUserInfo;
            List<MinimalServiceInfo> minimalServiceInfo;
            List<MinimalBlogInfo> minimalBlogInfo;
            AnalysitcData data;

            try
            {
                // Getting ALL product information from product service
                using (var productClient = httpClientFactory.CreateClient("ProductService"))
                {
                    var product_query_result = await productClient.GetFromJsonAsync<Result<List<MinimalServiceInfo>>>("/api/services/minimal");
                    minimalServiceInfo = product_query_result.Data;

                    if (minimalServiceInfo == null)
                    {
                        throw new Exception("Failed to fetch user info from product service.");
                    }
                };

                // Getting ALL user information from auth service
                using (var authClient = httpClientFactory.CreateClient("AuthService"))
                {
                    //authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", query.UserToken.Split(" ")[1]);
                    var auth_query_result = await authClient.GetFromJsonAsync<Result<List<MinimalUserInfo>>>("/api/users/minimal");
                    
                    minimalUserInfo = auth_query_result?.Data;

                    if (minimalUserInfo == null)
                    {
                        throw new Exception("Failed to fetch user info from user service.");
                    }

                }

                // Getting ALL blog information from the blog service
                using (var blogClient = httpClientFactory.CreateClient("BlogService"))
                {
                    var blog_query_result = await blogClient.GetFromJsonAsync<Result<List<MinimalBlogInfo>>>("/api/blogs/minimal");
                    minimalBlogInfo = blog_query_result?.Data;

                    if (minimalBlogInfo == null)
                    {
                        throw new Exception("Failed to fetch blog information from blog service.");
                    }
                }

                // Life time data
                GeneralAnalysticData lifetime = new GeneralAnalysticData
                {
                    TotalCustomerCount = minimalUserInfo.Count(x => x.Role == UserRole.Customer),
                    TotalSupplierCount = minimalUserInfo.Count(x => x.Role == UserRole.Supplier),
                    TotalServiceCount = minimalServiceInfo.Count(),
                    TotalEventCompleted = events_list.Count(),
                    TotalRevenue = transaction_list.Sum(x => x.Amount),
                    TotalUsedServiceCount = orders_list.Count(),
                    TotalBlogCount = minimalBlogInfo.Count(),
                };

                // Monthly data of the current year
                List<MonthlyGeneralAnalysticData> monthly_data = new();

                for (int i = 1; i <= 12; i++)
                {
                    monthly_data.Add(new MonthlyGeneralAnalysticData
                    {
                        Month = i,
                        Data = new GeneralAnalysticData
                        {
                            TotalBlogCount = minimalBlogInfo.Count(x => x.CreatedAt.Year == query.Year && x.CreatedAt.Month == i),
                            TotalCustomerCount = minimalUserInfo.Count(x => x.CreatedDate.Year == query.Year && x.CreatedDate.Month == i && x.Role == UserRole.Customer),
                            TotalSupplierCount = minimalUserInfo.Count(x => x.CreatedDate.Year == query.Year && x.CreatedDate.Month == i && x.Role == UserRole.Supplier),
                            TotalRevenue = transaction_list.Where(x => x.CreatedAt.Year == query.Year && x.CreatedAt.Month == i).Sum(x => x.Amount),
                            TotalEventCompleted = events_list.Count(x => x.CreatedAt.Year == query.Year && x.CreatedAt.Month == i),
                            TotalServiceCount = events_list.Count(x => x.CreatedAt.Year == query.Year && x.CreatedAt.Month == i),
                            TotalUsedServiceCount = orders_list.Count(x => x.CreatedAt.Year == query.Year && x.CreatedAt.Month == i),
                        }
                    });
                }

                // Yearly data of the current yeat
                YearlyGeneralAnalysticData yearly_data = new YearlyGeneralAnalysticData
                {
                    Year = query.Year,
                    Data = new GeneralAnalysticData
                    {
                        TotalRevenue = transaction_list.Where(x => x.CreatedAt.Year == query.Year).Sum(x => x.Amount),
                        TotalBlogCount = minimalBlogInfo.Count(x => x.CreatedAt.Year == query.Year),
                        TotalCustomerCount = minimalUserInfo.Count(x => x.CreatedDate.Year == query.Year && x.Role == UserRole.Customer),
                        TotalSupplierCount = minimalUserInfo.Count(x => x.CreatedDate.Year == query.Year && x.Role == UserRole.Supplier),
                        TotalEventCompleted = events_list.Count(x => x.CreatedAt.Year == query.Year),
                        TotalServiceCount = minimalServiceInfo.Count(x => x.createdDate.Year == query.Year),
                        TotalUsedServiceCount = orders_list.Count(x => x.CreatedAt.Year == query.Year),
                    },
                    MonthlyData = monthly_data,
                };

                var grouped_feedback = feedback_list.GroupBy(x => x.UsedService.ServiceId).OrderByDescending(x => x.Average(x => x.RatingService));

                // Best rating services
                List<TopService> bestServices = grouped_feedback.Take(3).Select(x => new TopService
                {
                    ServiceId = x.Key,
                    ServiceName = minimalServiceInfo.First(y => y.Id == x.Key).Name,
                    SupplierId = minimalServiceInfo.First(y => y.Id == x.Key).SupplierId,
                    SupplierName = minimalUserInfo.First(y => y.Id == minimalServiceInfo.First(y => y.Id == x.Key).SupplierId).SupplierName,
                    ServiceUseCount = orders_list.Count(y => y.ServiceId == x.Key),
                }).ToList();

                // Worst rating services
                List<TopService> worstServices = grouped_feedback.Skip(3).Reverse().Take(3).Select(x => new TopService
                {
                    ServiceId = x.Key,
                    ServiceName = minimalServiceInfo.First(y => y.Id == x.Key).Name,
                    SupplierId = minimalServiceInfo.First(y => y.Id == x.Key).SupplierId,
                    SupplierName = minimalUserInfo.First(y => y.Id == minimalServiceInfo.First(y => y.Id == x.Key).SupplierId).SupplierName,
                    ServiceUseCount = orders_list.Count(y => y.ServiceId == x.Key),
                }).ToList();

                // Data to be returned
                data = new AnalysitcData
                {
                    LifeTimeData = lifetime,
                    TopRatingServices = bestServices,
                    LeastRatingServices = worstServices,
                    YearlyData = yearly_data,
                    RequestDate = DateTime.UtcNow,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.StackTrace);

                return Result<AnalysitcData>
                    .Failure(Error.UnhandledError($"{ex.Message}"), "Failed to process user request");
            }

            return Result<AnalysitcData>.Success(data, "Success");
        }
    }
}
