using AutoMapper;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Helpers;

namespace InvestmentManager.ApplicationCore.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddStockPositionRequest, StockPosition>()
                .ForMember(
                    dest => dest.Cost,
                    opt => opt.MapFrom(src => FinancialMetrics.CalculateCost(src.Quantity, src.AveragePrice)))
                .ForMember(
                    dest => dest.PositionId,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                );

            CreateMap<StockPosition, StockPositionResponse>()
                .ForMember(
                    dest => dest.MarketValue,
                    opt => opt.MapFrom(src => FinancialMetrics.CalculateMarketValue(src.Quantity, src.CurrentPrice))
                )
                .ForMember(
                    dest => dest.PercentualGain,
                    opt => opt.MapFrom(src => FinancialMetrics.CalculatePercentualGain(src.CurrentPrice, src.AveragePrice))
                )
                .ForMember(
                    dest => dest.MonetaryGain,
                    opt => opt.MapFrom(src => FinancialMetrics.CalculateMonetaryGain(src.Quantity, src.CurrentPrice, src.AveragePrice)));
    
            CreateMap<AddStockPositionRequest, AddTransactionRequest>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.AveragePrice))
                .ForMember(
                    dest => dest.DateAndTimeOfTransaction,
                    opt => opt.MapFrom(src => src.DateAndTimeOfStockPosition))
                .ForMember(
                    dest => dest.TransactionType,
                    opt => opt.MapFrom(src => TransactionType.Buy));

            CreateMap<UpdateStockPositionRequest, AddTransactionRequest>()
                .ForMember(
                    dest => dest.DateAndTimeOfTransaction,
                    opt => opt.MapFrom(src => src.DateAndTimeOfStockPosition));

            CreateMap<AddTransactionRequest, Transaction>()
                .ForMember(
                    dest => dest.TransactionId,
                    opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(
                    dest => dest.Cost,
                    opt => opt.MapFrom(src => FinancialMetrics.CalculateCost(src.Quantity, src.Price)))
                .ForMember(
                    dest => dest.TransactionType,
                    opt => opt.MapFrom(src => src.TransactionType.ToString()));

            CreateMap<Transaction, TransactionResponse>();
        }
    }

   
}
