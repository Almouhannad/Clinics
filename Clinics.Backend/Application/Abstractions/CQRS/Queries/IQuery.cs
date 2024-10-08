﻿using Domain.Shared;
using MediatR;

namespace Application.Abstractions.CQRS.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
