using AutoMapper;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.ApplicationCore.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddStockPositionRequest, StockPosition>()
                .ForMember(
                    dest => dest.PositionId,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                )
                .ForMember(
                    dest => dest.Symbol,
                    opt => opt.MapFrom(src => src.Symbol.ToUpper())
                )
                .ForMember(
                    dest => dest.TradingCountry,
                    opt => opt.MapFrom(src => src.TradingCountry.ToString()));

            CreateMap<StockPosition, StockPositionResponse>();

            CreateMap<AddStockPositionRequest, AddTransactionRequest>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.AveragePrice))
                .ForMember(
                    dest => dest.TransactionType,
                    opt => opt.MapFrom(src => TransactionType.Buy));

            CreateMap<UpdateStockPositionRequest, AddTransactionRequest>();
               
            CreateMap<AddTransactionRequest, Transaction>()
                .ForMember(
                    dest => dest.TransactionId,
                    opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(
                    dest => dest.TransactionType,
                    opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(
                    dest => dest.TradingCountry,
                    opt => opt.MapFrom(src => src.TradingCountry.ToString()));

            CreateMap<Transaction, TransactionResponse>();
        }
    }

   
}
