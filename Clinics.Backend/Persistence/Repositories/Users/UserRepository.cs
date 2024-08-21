﻿using Domain.Entities.Identity.Users;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Identity.PasswordsHashing;
using Persistence.Repositories.Base;
using Persistence.Repositories.Users.Specifications;

namespace Persistence.Repositories.Users;

public class UserRepository : Repositroy<User>, IUserRepository
{
    #region Ctor DI
    private readonly IPasswordHasher _passwordHasher;
    public UserRepository(ClinicsDbContext context, IPasswordHasher passwordHasher) : base(context)
    {
        _passwordHasher = passwordHasher;
    }
    #endregion

    #region Create method
    public override async Task<Result<User>> CreateAsync(User entity)
    {
        _context.Entry(entity.Role).State = EntityState.Unchanged;
        var passwordResult = entity.SetHashedPassword(_passwordHasher.Hash(entity.HashedPassword));
        if (passwordResult.IsFailure)
            return Result.Failure<User>(passwordResult.Error);
        return await base.CreateAsync(entity);
    }
    #endregion

    #region Get by username
    public async Task<Result<User>> GetByUserNameFullAsync(string userName)
    {
        var query = ApplySpecification(new FullUserSpecification(user => user.UserName == userName));

        var result = await query.ToListAsync();

        if (result.Count == 0)
        {
            return Result.Failure<User>(IdentityErrors.NotFound);
        }
        return result.First();
    }
    #endregion

    #region Verify password
    public async Task<Result<User?>> VerifyPasswordAsync(string userName, string password)
    {
        var userResult = await GetByUserNameFullAsync(userName);
        if (userResult.IsFailure)
            return Result.Failure<User?>(userResult.Error);

        if (!_passwordHasher.Verify(password, userResult.Value.HashedPassword))
            return Result.Success<User?>(null);

        return Result.Success<User?>(userResult.Value);

    }
    #endregion

    #region Get doctor user by user name full
    public async Task<Result<DoctorUser>> GetDoctorUserByUserNameFullAsync(string username)
    {
        // This is a multi level query, so using specification pattern in this case is useless
        var query
            = _context.Set<DoctorUser>()
            .Include(doctroUser => doctroUser.User)
                .ThenInclude(user => user.Role)
            .Where(doctorUser => doctorUser.User.UserName == username)
            .Include(doctorUser => doctorUser.Doctor)
                .ThenInclude(doctor => doctor.PersonalInfo);
        var result = await query.ToListAsync();

        if (result.Count != 1)
            return Result.Failure<DoctorUser>(IdentityErrors.NotFound);

        return result.First();
    }
    #endregion

    #region Get receptionist user by user name full
    public async Task<Result<ReceptionistUser>> GetReceptionistUserByUserNameFullAsync(string username)
    {
        var query
            = _context.Set<ReceptionistUser>()
            .Include(receptionistUser => receptionistUser.User)
                .ThenInclude(user => user.Role)
            .Where(receptionistUser => receptionistUser.User.UserName == username)
            .Include(receptionistUser => receptionistUser.PersonalInfo);

        var result = await query.ToListAsync();

        if (result.Count != 1)
            return Result.Failure<ReceptionistUser>(IdentityErrors.NotFound);

        return result.First();
    }
    #endregion

    #region Register doctor
    public async Task<Result<DoctorUser>> RegisterDoctorAsync(DoctorUser doctorUser)
    {
        _context.Entry(doctorUser.User.Role).State = EntityState.Unchanged;
        _context.Entry(doctorUser.Doctor.Status).State = EntityState.Unchanged;

        var passwordResult = doctorUser.User.SetHashedPassword(_passwordHasher.Hash(doctorUser.User.HashedPassword));
        if (passwordResult.IsFailure)
            return Result.Failure<DoctorUser>(passwordResult.Error);
        try
        {
            var createdDoctorUser = await _context.Set<DoctorUser>().AddAsync(doctorUser);
            await _context.SaveChangesAsync();
            return createdDoctorUser.Entity;
        }
        catch (Exception)
        {
            return Result.Failure<DoctorUser>(IdentityErrors.UnableToRegister);
        }
    }
    #endregion

    #region Register receptionist
    public async Task<Result<ReceptionistUser>> RegisterReceptionistAsync(ReceptionistUser receptionistUser)
    {
        _context.Entry(receptionistUser.User.Role).State = EntityState.Unchanged;

        var passwordResult = receptionistUser.User.SetHashedPassword(_passwordHasher.Hash(receptionistUser.User.HashedPassword));
        if (passwordResult.IsFailure)
            return Result.Failure<ReceptionistUser>(passwordResult.Error);

        try
        {
            var createdreceptionistUser = await _context.Set<ReceptionistUser>().AddAsync(receptionistUser);
            await _context.SaveChangesAsync();
            return createdreceptionistUser.Entity;
        }
        catch (Exception)
        {
            return Result.Failure<ReceptionistUser>(IdentityErrors.UnableToRegister);
        }
    }

    #endregion
}
