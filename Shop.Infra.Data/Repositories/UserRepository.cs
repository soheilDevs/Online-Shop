using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Domain.ViewModels.Paging;
using Shop.Infra.Data.Context;

namespace Shop.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Constractor

        private readonly ShopDbContext _context;

        public UserRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Account

        public async Task<bool> IsUserExistPhoneNumber(string phoneNumber)
        {
            return await _context.Users.AsQueryable().AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.AsQueryable()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _context.Users.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == userId);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }


        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public bool CheckPermission(long permissionId, string phoneNumber)
        {
            long userid = _context.Users.AsQueryable().Single(c => c.PhoneNumber == phoneNumber).Id;

            var userRole = _context.UserRoles.AsQueryable()
                .Where(c => c.UserId == userid).Select(r => r.RoleId).ToList();

            if (!userRole.Any())
                return false;

            var permissions = _context.RolePermissions.AsQueryable()
                .Where(c => c.PermissionId == permissionId).Select(c => c.RoleId).ToList();

            return permissions.Any(c => userRole.Contains(c));
        }

        public async Task<bool> IsExistProductFavorite(long productId, long userId)
        {
            return await _context.UserFavorites.AsQueryable()
                .AnyAsync(c => c.ProductId == productId && c.UserId == userId);
        }

        public async Task<bool> IsExistProductCompare(long productId, long userId)
        {
            return await _context.UserCompares.AsQueryable()
                .Where(c => !c.IsDelete && c.ProductId == productId && c.UserId == userId)
                .AnyAsync();
        }

        public async Task AddUserFavorite(UserFavorite userFavorite)
        {
            await _context.UserFavorites.AddAsync(userFavorite);
        }

        public async Task AddUserCompare(UserCompare userCompare)
        {
            await _context.UserCompares.AddAsync(userCompare);
        }

        public async Task<List<UserCompare>> GetUserCompares(long userId)
        {
            return await _context.UserCompares.Include(c => c.Product).AsQueryable()
                .Where(c => c.UserId == userId && !c.IsDelete)
                .ToListAsync();
        }

        public async Task<int> UserFavoriteCount(long userId)
        {
            return await _context.UserFavorites.AsQueryable()
                .Where(c => c.UserId == userId)
                .CountAsync();
        }

        public async Task<List<UserFavorite>> GetUserFavorites(long userId)
        {
            return await _context.UserFavorites.Include(c => c.Product).AsQueryable()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public void UpdateUserCompare(UserCompare userCompare)
        {
            _context.UserCompares.Update(userCompare);
        }

        public async Task RemoveProductInUserCompare(long id)
        {
            var currentUserCompare = await _context.UserCompares.AsQueryable().SingleOrDefaultAsync(c => c.Id == id);


            if (currentUserCompare != null)
            {

                _context.UserCompares.Remove(currentUserCompare);
            }
        }

        public async Task<UserCompare> GetUSerCompare(long userId, long productId)
        {
            return await _context.UserCompares.AsQueryable()
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveAllRangeUserCompare(long userId)
        {
            var data = await _context.UserCompares.Where(c => c.UserId == userId).ToListAsync();

            if (data!=null &&data.Any())
            {
                _context.UserCompares.RemoveRange(data);
                
            }
        }

        public async Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares)
        {
            var query = _context.UserCompares.Include(c => c.Product).ThenInclude(c => c.ProductFeatures).AsQueryable();

            #region Paging

            var pager = Pager.Build(userCompares.PageId, await query.CountAsync(), userCompares.TakeEntity,
                userCompares.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).Where(c => !c.IsDelete).ToListAsync();

            #endregion

            return userCompares.SetPaging(pager).SetCompares(allData);
        }

        public async Task<UserFavoritesViewModel> UserFavorites(UserFavoritesViewModel userFavorites)
        {
            var query = _context.UserFavorites.Include(c => c.Product).AsQueryable();

            #region Paging

            var pager = Pager.Build(userFavorites.PageId, await query.CountAsync(), userFavorites.TakeEntity,
                userFavorites.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();

            #endregion

            return userFavorites.SetPaging(pager).SetFavorites(allData);
        }

        #endregion

        #region Admin

        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber == filter.PhoneNumber);
            }

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetUsers(allData);
        }

        public async Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId)
        {
            return await _context.Users.AsQueryable()
                .Include(c => c.UserRoles)
                .Where(c => c.Id == userId)
                .Select(x => new EditUserFromAdmin
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserGender = x.UserGender,
                    RoleIds = x.UserRoles.Where(c => c.UserId == userId).Select(c => c.RoleId).ToList()
                }).SingleOrDefaultAsync();
        }

        public async Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId)
        {
            return await _context.Roles.AsQueryable()
                .Include(c => c.RolePermissions)
                .Where(c => c.Id == roleId)
                .Select(x => new CreateOrEditRoleViewModel
                {
                    Id = x.Id,
                    RoleTitle = x.RoleTitle,
                    SelectedPermissions = x.RolePermissions.Select(c => c.PermissionId).ToList()
                }).SingleOrDefaultAsync();
        }

        public async Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter)
        {
            var query = _context.Roles.AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(c => EF.Functions.Like(c.RoleTitle, $"%{filter.RoleName}%"));
            }

            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetRoles(allData);
        }

        public async Task CreateRole(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _context.Permissions.Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task<Role> GetRoleById(long id)
        {
            return await _context.Roles.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task RemoveAllPermissionSelectRole(long roleId)
        {
            var allRolePermission = await _context.RolePermissions.Where(c => c.RoleId == roleId).ToListAsync();
            if (allRolePermission.Any())
            {
                _context.RolePermissions.RemoveRange(allRolePermission);
            }
        }

        public async Task AddPermissionToRole(List<long> selectedPermission, long roleId)
        {
            if (selectedPermission != null && selectedPermission.Any())
            {
                var rolePermission = new List<RolePermission>();
                foreach (var permissionId in selectedPermission)
                {
                    rolePermission.Add(new RolePermission
                    {
                        PermissionId = permissionId,
                        RoleId = roleId
                    });
                }

                await _context.RolePermissions.AddRangeAsync(rolePermission);
            }
        }

        public async Task<List<Role>> GetAllActiveRoles()
        {
            return await _context.Roles.AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task RemoveAllUserSelectRole(long userId)
        {
            var allUserRoles = await _context.UserRoles.AsQueryable().Where(c => c.UserId == userId).ToListAsync();
            if (allUserRoles.Any())
            {
                _context.UserRoles.RemoveRange(allUserRoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddUserToRole(List<long> selectedRole, long userId)
        {
            if (selectedRole != null && selectedRole.Any())
            {
                var userRoles = new List<UserRole>();
                foreach (var roleId in selectedRole)
                {
                    userRoles.Add(new UserRole
                    {
                        RoleId = roleId,
                        UserId = userId
                    });
                }

                await _context.UserRoles.AddRangeAsync(userRoles);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

    }
}