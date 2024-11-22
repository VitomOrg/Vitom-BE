using API.Utils;
using Application.UC_File;
using Ardalis.Result;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.FileEndpointHandler;

public class UploadFileEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        [FromForm] FileEnum fileEnum,
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        Result<string> result = await sender.Send(
            new UploadFile.Command(
                FileEnum: fileEnum,
                File: file
            ),
            cancellationToken
        );
        return result.Check();
    }
}