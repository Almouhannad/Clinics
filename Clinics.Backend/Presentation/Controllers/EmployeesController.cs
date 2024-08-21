﻿using Application.Employees.Commands.AttachFamilyMemberToEmployee;
using Application.Employees.Commands.CreateEmployee;
using Domain.Entities.Identity.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

namespace Presentation.Controllers;

[Route("api/Employees")]
public class EmployeesController : ApiController
{

    #region DI for MeditR sender
    public EmployeesController(ISender sender) : base(sender)
    {
    }
    #endregion

    [Authorize(Roles = Roles.AdminName)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return HandleFailure(result);
        return Created();
    }
    [Authorize(Roles = Roles.DoctorName)]
    [HttpPut("FamilyMembers")]
    public async Task<IActionResult> AttachFamilyMember([FromBody] AttachFamilyMemberToEmployeeCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return HandleFailure(result);
        return Ok();
    }
}
