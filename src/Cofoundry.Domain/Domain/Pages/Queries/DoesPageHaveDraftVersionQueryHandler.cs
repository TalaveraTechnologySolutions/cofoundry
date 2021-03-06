﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.CQS;

namespace Cofoundry.Domain
{
    public class DoesPageHaveDraftVersionQueryHandler
        : IAsyncQueryHandler<DoesPageHaveDraftVersionQuery, bool>
        , IPermissionRestrictedQueryHandler<DoesPageHaveDraftVersionQuery, bool>
    {
        #region constructor

        private readonly CofoundryDbContext _dbContext;

        public DoesPageHaveDraftVersionQueryHandler(
            CofoundryDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        #endregion

        #region execution

        public Task<bool> ExecuteAsync(DoesPageHaveDraftVersionQuery query, IExecutionContext executionContext)
        {
            var exists = _dbContext
                .PageVersions
                .FilterActive()
                .FilterByPageId(query.PageId)
                .AnyAsync(v => v.WorkFlowStatusId == (int)WorkFlowStatus.Draft);

            return exists;
        }

        #endregion

        #region Permission

        public IEnumerable<IPermissionApplication> GetPermissions(DoesPageHaveDraftVersionQuery query)
        {
            yield return new PageReadPermission();
        }

        #endregion
    }

}
