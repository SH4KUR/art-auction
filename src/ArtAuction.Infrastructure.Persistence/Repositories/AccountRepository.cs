using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<Account> GetAccount(Guid userId)
        {
            Account account;

            var query = @"
                SELECT 
	                 [account_id] AS AccountId
                    ,[user_id] AS UserId
                    ,[sum]
                    ,[last_update] AS LastUpdate
                FROM [dbo].[account]
                WHERE
	                [user_id] = @UserId";

            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    account = await connection.QueryFirstOrDefaultAsync<Account>(query, new
                    {
                        UserId = userId
                    }, transaction);

                    if (account != null)
                    {
                        account.Vips = await GetVips(account.UserId, connection, transaction);
                        account.Operations = await GetOperations(account.AccountId, connection, transaction);
                    }

                    await transaction.CommitAsync();
                }
            }

            return account;
        }

        private async Task<IEnumerable<Vip>> GetVips(Guid userId, IDbConnection connection, DbTransaction transaction)
        {
            var query = @"
                SELECT
	                 [vip_id] AS VipId
                    ,[operation_id] AS OperationId
                    ,[user_id] AS UserId
                    ,[date_from] AS DateFrom
                    ,[date_until] AS DateUntil
                FROM [dbo].[vip]
                WHERE
	                [user_id] = @UserId
                ORDER BY [date_until] DESC";

            var operations = (await connection.QueryAsync<Vip>(query, new
            {
                UserId = userId
            }, transaction)).ToArray();

            foreach (var operation in operations)
            {
                operation.Operation = await GetOperation(operation.OperationId, connection, transaction);
            }

            return operations;
        }
        
        private async Task<Operation> GetOperation(Guid operationId, IDbConnection connection, DbTransaction transaction)
        {
            var query = @"
                SELECT 
	                 [operation_id] AS OperationId
	                ,[account_id] AS AccountId
                    ,[date_time] AS DateTime
                    ,[operation_type] AS OperationType
                    ,[sum_before] AS SumBefore
                    ,[sum_operation] AS SumOperation
                    ,[sum_after] AS SumAfter
                    ,[description]
                FROM [dbo].[operation]
                WHERE 
	                [operation_id] = @OperationId";

            return await connection.QueryFirstOrDefaultAsync<Operation>(query, new
            {
                OperationId = operationId
            }, transaction);
        }

        private async Task<IEnumerable<Operation>> GetOperations(Guid accountId, IDbConnection connection, DbTransaction transaction)
        {
            var query = @"
                SELECT 
	                 [operation_id] AS OperationId
	                ,[account_id] AS AccountId
                    ,[date_time] AS DateTime
                    ,[operation_type] AS OperationType
                    ,[sum_before] AS SumBefore
                    ,[sum_operation] AS SumOperation
                    ,[sum_after] AS SumAfter
                    ,[description]
                FROM [dbo].[operation]
                WHERE 
	                [account_id] = @AccountId
                ORDER BY [date_time] DESC";

            return await connection.QueryAsync<Operation>(query, new
            {
                AccountId = accountId
            }, transaction);
        }
        
        public async Task AddOperation(Operation operation)
        {
            var query = @"
                INSERT INTO [dbo].[operation] (
                     [operation_id]
	                ,[account_id]
                    ,[date_time]
                    ,[sum_before]
                    ,[sum_operation]
                    ,[sum_after]
                    ,[description]
                )
                VALUES (
                     @OperationId
	                ,@AccountId
                    ,@DateTime
                    ,@SumBefore
                    ,@SumOperation
                    ,@SumAfter
                    ,@Description
                )

                UPDATE [dbo].[account]
                SET 
                     [sum] = @SumAfter
                    ,[last_update] = @DateTime
                 WHERE [account_id] = @AccountId";
            
            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await connection.ExecuteAsync(query, new
                        {
                            operation.OperationId,
                            operation.AccountId,
                            operation.DateTime,
                            operation.SumBefore,
                            operation.SumOperation,
                            operation.SumAfter,
                            operation.Description
                        });
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
        
        public async Task AddVip(Vip vip)
        {
            var query = @"
                INSERT INTO [dbo].[vip] (
	                 [vip_id]
                    ,[operation_id]
                    ,[user_id]
                    ,[date_from]
                    ,[date_until]
                )
                VALUES (
	                ,@VipId
	                ,@OperationId
	                ,@UserId
	                ,@DateFrom
	                ,@DateUntil
                )";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                vip.VipId,
                vip.OperationId,
                vip.UserId,
                vip.DateFrom,
                vip.DateUntil
            });
        }
    }
}