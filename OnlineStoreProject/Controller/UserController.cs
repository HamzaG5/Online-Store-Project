﻿using Domain.DTO;
using Infrastructure.Services.UserService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreProject.Controller
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Function("CreateUser")]
        public async Task<HttpResponseData> CreateUserAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")] HttpRequestData req,
            FunctionContext executionContext)
        {
            // get request data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);

            // create response
            var response = req.CreateResponse();

            await response.WriteAsJsonAsync(await _userService.AddUser(userDTO));
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }
    }
}
