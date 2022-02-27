using System.Reflection;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Tests.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace ArtAuction.Infrastructure.IntegrationTests.DataAttributes
{
    public class UseAuctionAttribute : BeforeAfterTestAttribute
    {
        public string AuctionId { get; set; } = "{6DE7B006-CA79-4180-85D6-1171283ED4C8}";
        public int AuctionNumber { get; set; } = 14698847;
        public string LotId { get; set; } = "{5B3BC3D7-5AEC-433D-8E87-BAE7701A41AA}";
        public string SellerId { get; set; } = "{71A47031-0DB2-41DF-9FED-7A189357A260}";
        
        private readonly string _categoryId = "{AD87B03B-EDB8-4871-BA93-E34CB6239DDA}";

        public override void Before(MethodInfo methodUnderTest)
        {
            After(methodUnderTest);

            var query = @"
                INSERT INTO [dbo].[category] (
                     [category_id]
                    ,[name]
                )
                VALUES (
                     @CategoryId
                    ,'Some Test CategoryName'
                )

                INSERT INTO [dbo].[lot] (
                     [lot_id]
	                ,[category_id]
                    ,[name]
                    ,[painting_date]
                    ,[photo]
                    ,[description]
                )
                VALUES (
                     @LotId
	                ,@CategoryId
                    ,'Test Lot Name'
                    ,'Beginning of winter 1999'
                    ,'A0'
                    ,'Some test Lot Description'
                )

                SET IDENTITY_INSERT [dbo].[auction] ON 

                INSERT INTO [dbo].[auction] (
                     [auction_id]
                    ,[auction_number]
                    ,[lot_id]
                    ,[seller_id]
                    ,[creation_datetime]
                    ,[start_billing_datetime]
                    ,[end_billing_datetime]
                    ,[start_price]
                    ,[current_price]
                    ,[full_price]
                    ,[bid_step]
                    ,[is_closed]
                    ,[customer_id]
                )
                VALUES (
                     @AuctionId
                    ,@AuctionNumber
	                ,@LotId
	                ,@SellerId
                    ,GETDATE()
                    ,GETDATE()
	                ,DATEADD(MONTH, 1, GETDATE())
	                ,1400
	                ,1550
	                ,2000
	                ,50
	                ,0
                    ,NULL
                )

                SET IDENTITY_INSERT [dbo].[auction] OFF";

            using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString("ArtAuctionDbConnection"));
            connection.Execute(query, new
            {
                AuctionId,
                AuctionNumber,
                LotId,
                CategoryId = _categoryId,
                SellerId
            });
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var query = @"

                DELETE FROM [dbo].[auction]
                WHERE 
                    [auction_id] = @AuctionId

                DBCC CHECKIDENT('[dbo].[auction]', RESEED)

                DELETE FROM [dbo].[lot] 
                WHERE 
                    [lot_id] = @LotId
                
                DELETE FROM [dbo].[category] 
                WHERE 
                    [category_id] = @CategoryId";

            using (var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new
                    {
                        AuctionId,
                        LotId,
                        CategoryId = _categoryId
                    }, transaction);
                    transaction.Commit();
                }
            }
        }
    }
}