using Identity.DbContext;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Filter
{
    // Class ============= || Unit of Work ||========================================================
    public class UnitOfWorkFilter : IActionFilter
    {
        // Db Context
        private readonly IdentityContext _dbContext;
        private IDbContextTransaction _transaction; //Transaction



        // || Constructor || =========================================================================
        public UnitOfWorkFilter(IdentityContext _dbContext)
        {
            this._dbContext = _dbContext;
        }






        // || OnActionExecuting || ====================================================================
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) // If Context Null
            {
                throw new ArgumentException(nameof(context));
            }

            if (context.HttpContext.Request.Method == "GET") // IF GET
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false; // No Ef-Core Tracking on Get
                return;
            }
            _transaction = _dbContext.Database.BeginTransaction(); // Begin Transaction
        }





        // || OnActionExecuted || ====================================================================
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context == null) // If Context Null
            {
                throw new ArgumentException(nameof(context));
            }

            if (context.HttpContext.Request.Method == "GET") // IF GET
            {
                return;
            }



            if (context.Exception == null) // If No DbContext Exception
            {
                _dbContext.SaveChangesAsync().Wait(new TimeSpan(0, 0, 10)); // Save and wait max 10 sec for save to complete
                _transaction.CommitAsync().Wait(new TimeSpan(0, 0, 10)); // Commit
                _transaction.Dispose();
            }
            else
            {
                _transaction.RollbackAsync().GetAwaiter().GetResult(); // Rollback
                _transaction.Dispose();
            }
        }


    }
}
