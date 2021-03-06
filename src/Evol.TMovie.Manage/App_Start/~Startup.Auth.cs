﻿//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Evol.TMovie.Manage.Models.Identity;
//using Evol.TMovie.Manage.Core.Identity;

//namespace Evol.TMovie.Manage
//{
//    public partial class Startup
//    {
//        public void ConfigureIdentity(IServiceCollection services)
//        {
//            // 这个就是被 Identity 使用的
//            services.AddAuthentication(options =>
//            {
//                // This is the Default value for ExternalCookieAuthenticationScheme
//                options.DefaultSignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
//            });

//            //配置权限策略
//            services.AddAuthorization(options =>
//            {
//                ///<see cref="Startup.PerLoad(IServiceCollection, System.IServiceProvider)"/>
//            });

//            // 注册 IHttpContextAccessor ，会用到
//            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//            services.AddScoped<IUserStore<AppUser>, AppUserStore>();
//            services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();

//            // Identity services
//            //services.TryAddSingleton<IdentityMarkerService>(); //.net core 1.1 升级 .net core 2.0 不支持
//            services.TryAddScoped<IUserValidator<AppUser>, UserValidator<AppUser>>();
//            services.TryAddScoped<IPasswordValidator<AppUser>, PasswordValidator<AppUser>>();
//            services.TryAddScoped<IPasswordHasher<AppUser>, DefaultPasswordHasher>();
//            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
//            services.TryAddScoped<IRoleValidator<AppUser>, RoleValidator<AppUser>>();

//            // 错误描述信息
//            services.TryAddScoped<IdentityErrorDescriber>();
//            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<AppUser>>();

//            //身份当事人工厂
//            services.TryAddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, AppRole>>();

//            //三大对象
//            services.TryAddScoped<UserManager<AppUser>, UserManager<AppUser>>();
//            services.TryAddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
//            services.TryAddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
//        }

//    }
//}
