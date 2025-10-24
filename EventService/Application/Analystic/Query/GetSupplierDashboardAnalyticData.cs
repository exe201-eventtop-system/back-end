using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities;
using SharedLibrary.DTOs.Blog;
using SharedLibrary.DTOs.Service;
using SharedLibrary.Jwt;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Analystic.Query
{
    public class GetSupplierDashboardDataQuery
    {
        public string? Token { get; set; }
    }

    public class DashboardItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }
    public class SupplierAnalyticData
    {
        [JsonPropertyName("stats")]
        public List<DashboardItem> GeneralStatistic { get; set; }

        [JsonPropertyName("monthlyRevenue")]
        public List<DashboardItem> MonthlyRevenue { get; set; }
    }

    public class GetSupplierDashboardAnalyticDataQueryHandler : IQueryHandler<GetSupplierDashboardDataQuery, Result<SupplierAnalyticData>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        private readonly IHttpClientFactory httpClientFactory;

        public GetSupplierDashboardAnalyticDataQueryHandler(IUnitOfWork unitOfWork, JwtService jwtService, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<SupplierAnalyticData>> Handle(GetSupplierDashboardDataQuery query, CancellationToken cancellationToken)
        {
            // Get user id for data extraction

            if (query.Token == null)
            {
                return Result<SupplierAnalyticData>
                    .Failure(Error.UnauthorizedError(""), "Failed to process user request");
            }

            Guid SupplierId = await jwtService.ExtractUserIdFromToken(query.Token);

            // Fetch ALL data from the database
            List<UsedService> order = await unitOfWork.UsedServiceRepository.GetAllAsync(x => x.SupplierId == SupplierId, null);

            // The fuck is this ???

            int service_count = 0;
            int blog_count = 0;
            List<DashboardItem> MonthlyItem = new List<DashboardItem>();


            using (var ProductCaller = httpClientFactory.CreateClient("ProductService"))
            {
                var result = await ProductCaller.GetFromJsonAsync<Result<List<MinimalServiceInfo>>>($"/api/services/minimal/supplier/{SupplierId}");


                List<MinimalServiceInfo> services =  result.Data;

                service_count = services.Count;
            }

            using (var BlogCaller = httpClientFactory.CreateClient("BlogService"))
            {
                var result = await BlogCaller.GetFromJsonAsync<Result<List<MinimalBlogInfo>>>($"/api/blogs/minimal/supplier/{SupplierId}");
                ;
                List<MinimalBlogInfo> blogs = result.Data;

                blog_count = blogs.Count;
            }

            for(int i = 1; i <=12; i ++)
            {
                MonthlyItem.Add(new DashboardItem { Name = i.ToString() , Value = order.Where(x => x.CreatedAt.Month == i).Sum(x => x.UnitPrice)});
            }

            return Result<SupplierAnalyticData>.Success(
                new SupplierAnalyticData
                {
                    GeneralStatistic = new List<DashboardItem>
                    {
                        new DashboardItem
                        {
                            Name = "Doanh thu",
                            Value = order.Sum(x => x.UnitPrice * 0.95m),
                        },
                        new DashboardItem
                        {
                            Name = "Dịch vụ",
                            Value = service_count,
                        },
                        new DashboardItem
                        {
                            Name = "Blog",
                            Value = blog_count,
                        },
                    },
                    MonthlyRevenue = MonthlyItem,
                }, "Success");
        }
    }
}
