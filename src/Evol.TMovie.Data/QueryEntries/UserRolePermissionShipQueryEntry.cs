﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evol.Common;
using Evol.TMovie.Domain.Models.AggregateRoots;
using Evol.TMovie.Domain.Models.Entities;
using Evol.TMovie.Domain.QueryEntries.Parameters;
using Evol.TMovie.Domain.QueryEntries;
using Evol.TMovie.Domain.Repositories;
using System.Linq;
using System.Linq.Expressions;

namespace Evol.TMovie.Data.QueryEntries
{
    public class UserRolePermissionShipQueryEntry : IUserRolePermissionShipQueryEntry
    {
        private IUserRolePermissionShipRepository _userRolePermissionShipRepository { get; set; }

        private IPermissionQueryEntry _permissionQueryEntry { get; set; }

        private IRoleQueryEntry _roleQueryEntry { get; set; }

        private IUserQueryEntry _userQueryEntry { get; set; }

        public UserRolePermissionShipQueryEntry(
            IUserRolePermissionShipRepository userRolePermissionShipQueryEntry,
            IPermissionQueryEntry permissionQueryEntry,
            IRoleQueryEntry roleQueryEntry
            )
        {
            _userRolePermissionShipRepository = userRolePermissionShipQueryEntry;
            _permissionQueryEntry = permissionQueryEntry;
            _roleQueryEntry = roleQueryEntry;
        }

        public async Task<UserRolePermissionShip> FetchAsync(Guid id)
        {
             return await _userRolePermissionShipRepository.FindAsync(id);
        }

        public async Task<IList<UserRolePermissionShip>> RetrieveAsync(UserRolePermissionShipQueryParameter param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            if (param.RoleId == null || param.UserId == null)
                throw new ArgumentNullException(($"{nameof(param.RoleId)} & {nameof(param.UserId)}"));

            Expression<Func<UserRolePermissionShip, bool>> query = null;
            if (param.RoleId != null && param.UserId != null)
                query = e => e.RoleId == param.RoleId.Value && e.UserId == param.UserId.Value;
            else if (param.RoleId != null)
                query = e => e.RoleId == param.RoleId.Value;
            else if (param.UserId != null)
                query = e => e.UserId == param.UserId.Value;

            var list = (await _userRolePermissionShipRepository.RetrieveAsync(query)).ToList();
            return list;
        }

        public async Task<IPaged<UserRolePermissionShip>> PagedAsync(UserRolePermissionShipQueryParameter param, int pageIndex, int pageSize)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            if (param.RoleId == null || param.UserId == null)
                throw new ArgumentNullException(($"{nameof(param.RoleId)} & {nameof(param.UserId)}"));

            Expression<Func<UserRolePermissionShip, bool>> query = null;
            if (param.RoleId != null && param.UserId != null)
                query = e => e.RoleId == param.RoleId.Value && e.UserId == param.UserId.Value;
            else if (param.RoleId != null)
                query = e => e.RoleId == param.RoleId.Value;
            else if (param.UserId != null)
                query = e => e.UserId == param.UserId.Value;

            var result = await _userRolePermissionShipRepository.PagedAsync(query, pageIndex, pageSize);
            return result;
        }

        public async Task<IList<Permission>> GetCustomPermissionsByUserIdAsync(Guid userId)
        {
            var list = (await _userRolePermissionShipRepository.RetrieveAsync(e => e.CustomPermissionId != null && e.UserId == userId)).ToList();
            var customPermissionids = list.Select(e => e.CustomPermissionId.Value).ToArray();
            var permissions = await _permissionQueryEntry.GetByPermissionIdsAsync(customPermissionids);
            return permissions;
        }

        public async Task<IList<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            var list = (await _userRolePermissionShipRepository.RetrieveAsync(e => e.RoleId != null && e.UserId == userId)).ToList();
            var roleIds = list.Select(e => e.RoleId.Value).ToArray();
            var permissions = await _roleQueryEntry.GetByIdsAsync(roleIds);
            return permissions;
        }

        public async Task<IList<User>> GetUsersByRoleIdAsync(Guid userId)
        {
            var list = (await _userRolePermissionShipRepository.RetrieveAsync(e => e.RoleId != null && e.UserId == userId)).ToList();
            var userIds = list.Select(e => e.UserId).ToArray();
            var users = await _userQueryEntry.GetByIdsAsync(userIds);
            return users;
        }
    }
}
