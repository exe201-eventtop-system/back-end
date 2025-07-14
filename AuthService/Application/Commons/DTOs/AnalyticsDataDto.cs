using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class StatItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Change { get; set; } = string.Empty;
    }

    public class MonthlyRevenueItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int Revenue { get; set; }
    }

    public class UserGrowthItemDto
    {
        public string Month { get; set; } = string.Empty;
        public int Users { get; set; }
    }

    public class SupplierGrowthItemDto
    {
        public string Month { get; set; } = string.Empty;
        public int Suppliers { get; set; }
    }

    public class EventTypeItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public class TopServiceItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int RentalCount { get; set; }
    }

    public class LeastRatedServiceItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int NegativeFeedbackCount { get; set; }
    }

    public class AnalyticsDataDto
    {
        public List<StatItemDto> Stats { get; set; } = new();
        public List<MonthlyRevenueItemDto> MonthlyRevenue { get; set; } = new();
        public List<UserGrowthItemDto> UserGrowth { get; set; } = new();
        public List<SupplierGrowthItemDto> SupplierGrowth { get; set; } = new();
        public List<EventTypeItemDto> EventTypes { get; set; } = new();
        public List<TopServiceItemDto> TopServices { get; set; } = new();
        public List<LeastRatedServiceItemDto> LeastRatedServices { get; set; } = new();
    }

}
