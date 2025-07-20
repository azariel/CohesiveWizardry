using Cohesive_rp_storage_dtos.Requests.Users;
using CohesiveWizardry.Common.Exceptions.HTTP;
using CohesiveWizardry.Storage.WebApi.Workflows.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CohesiveWizardry.Storage.WebApi.Controllers
{
    /// <summary>
    /// Controller around Users. A User is a remote Client(Person).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IAddUserRequestWorkflow addUserRequestWorkflow;
        private IGetUserRequestWorkflow getUserRequestWorkflow;
        private IUpdateUserRequestWorkflow updateUserRequestWorkflow;
        private IDeleteUserRequestWorkflow deleteUserRequestWorkflow;

        public UsersController(
            IAddUserRequestWorkflow addUserRequestWorkflow,
            IGetUserRequestWorkflow getUserRequestWorkflow,
            IUpdateUserRequestWorkflow updateUserRequestWorkflow,
            IDeleteUserRequestWorkflow deleteUserRequestWorkflow)
        {
            this.addUserRequestWorkflow = addUserRequestWorkflow;
            this.getUserRequestWorkflow = getUserRequestWorkflow;
            this.updateUserRequestWorkflow = updateUserRequestWorkflow;
            this.deleteUserRequestWorkflow = deleteUserRequestWorkflow;
        }

        /// <summary>
        /// Just a ping-like endpoint.
        /// </summary>
        [HttpGet]
        [Route("teapot")]
        public async Task<ActionResult<object>> Teapot()
        {
            return "I am a teapot";
        }

        /// <summary>
        /// Get new User from storage.
        /// </summary>
        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<object>> GetUser(GetUserRequestDto userRequest)
        {
            object response = await getUserRequestWorkflow.ExecuteAsync(userRequest);

            if (response == null)
                return NotFound();

            return response;
        }

        /// <summary>
        /// Add new User to storage.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddUser([FromBody] AddUserRequestDto userRequest)
        {
            object response = await addUserRequestWorkflow.ExecuteAsync(userRequest);
            return response;
        }

        /// <summary>
        /// Update (full obj) User to storage.
        /// </summary>
        [HttpPut]
        [Route("{userIdToUpdate}")]
        public async Task<ActionResult<object>> UpdateUser([FromRoute]string UserIdToUpdate, UpdateUserRequestDto userRequest)
        {
            // Validate request
            if(userRequest.Id != UserIdToUpdate)
                throw new BadRequestWebApiException("6b38144f-2b7f-44e9-aa7b-57e4dc05b0e6", $"UserId [{UserIdToUpdate}] to update didn't match the provided body UserId [{userRequest.Id}].");

            object response = await updateUserRequestWorkflow.ExecuteAsync(userRequest);
            return response;
        }

        /// <summary>
        /// Delete User from storage.
        /// </summary>
        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<object>> DeleteUser(DeleteUserRequestDto userRequest)
        {
            object response = await deleteUserRequestWorkflow.ExecuteAsync(userRequest);
            return response;
        }
    }
}
