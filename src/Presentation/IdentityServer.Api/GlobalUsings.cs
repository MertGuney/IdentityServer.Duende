﻿global using IdentityServer.Application;
global using IdentityServer.Application.Common.Extensions;
global using IdentityServer.Application.Features.Commands.Auth.ChangePassword;
global using IdentityServer.Application.Features.Commands.Auth.ForgotPassword;
global using IdentityServer.Application.Features.Commands.Auth.Register;
global using IdentityServer.Application.Features.Commands.Auth.ResetPassword;
global using IdentityServer.Application.Features.Commands.Auth.TFAs.Activate;
global using IdentityServer.Application.Features.Commands.Auth.TFAs.Deactivate;
global using IdentityServer.Application.Features.Commands.Auth.TFAs.Enable;
global using IdentityServer.Application.Features.Commands.Auth.VerifyCode;
global using IdentityServer.Application.Features.Queries.Users.GetCurrentUser;
global using IdentityServer.Infrastructure;
global using IdentityServer.Persistence;
global using IdentityServer.Persistence.SeedData;
global using IdentityServer.Shared.Models;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using static Duende.IdentityServer.IdentityServerConstants;
