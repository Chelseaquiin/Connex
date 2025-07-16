using Connex.Models;
using Connex.Models.Dtos.Requests;
using Connex.Models.Entities;
using Connex.Services;
using Connex.Services.Inteface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Connex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnexController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailValidationService _emailValidator;
        private readonly IQrCodeGenerator _qrCodeGenerator;

        public ConnexController(IEmailSender emailSender, IEmailValidationService emailValidator, IQrCodeGenerator qrCodeGenerator)
        {
            _emailSender = emailSender;
            _emailValidator = emailValidator;
            _qrCodeGenerator = qrCodeGenerator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> SubmitInvite([FromBody] InviteRequest request)
        {
            try
            {
                bool emailIsValid = await _emailValidator.IsEmailValidAsync(request.Email);
                if (!emailIsValid)
                    return BadRequest("Email address is not valid.");

                var invites = new Invites
                {
                    FullName = request.FullName,
                    ReasonForApplying = request.ReasonForApplying,
                    Email = request.Email
                };

                Utility.Invites.Add(invites);
                return CreatedAtAction(nameof(GetInvite), new { id = invites.Id }, invites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while submitting the invite: {ex.Message}");
            }
        }

        [HttpGet("Get_All_Invites")]
        [Authorize]
        public IActionResult GetAllInvites()
        {
            try
            {
                return Ok(Utility.Invites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving invites: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public IActionResult GetInvite(Guid id)
        {
            try
            {
                var invite = Utility.Invites.FirstOrDefault(i => i.Id == id);
                if (invite == null) return NotFound();
                return Ok(invite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the invite: {ex.Message}");
            }
        }

        [HttpGet("Pending")]
        [Authorize]
        public IActionResult GetPendingInvites()
        {
            try
            {
                var pending = Utility.Invites
                    .Where(i => i.Status == InviteStatus.Pending)
                    .ToList();
                return Ok(pending);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving pending invites: {ex.Message}");
            }
        }

        [HttpPatch("{id:guid}/Status")]
        [Authorize]
        public async Task<IActionResult> UpdateInviteStatus(Guid id, [FromBody] UpdateInviteStatusRequest request)
        {
            try
            {
                var invite = Utility.Invites.FirstOrDefault(i => i.Id == id);
                if (invite == null) return NotFound();

                invite.Status = request.Status;

                string subject;
                string htmlBody;

                if (request.Status == InviteStatus.Approved)
                {
                    var qrCodeUrl = _qrCodeGenerator.GenerateQrCodeUrl(new
                    {
                        invite.FullName,
                        invite.Email,
                        invite.Event,
                        invite.Id
                    });

                    subject = "You're Invited!";
                    htmlBody = $@"
                <p>Hello {invite.FullName},</p>
                <p>Your invitation for <strong>{invite.Event}</strong> has been approved!</p>
                <p>Scan this QR Code at the entrance:</p>
                <img src='{qrCodeUrl}' alt='QR Code' />
            ";
                }
                else if (request.Status == InviteStatus.Rejected)
                {
                    subject = "Invitation Not Approved";
                    htmlBody = $@"
                <p>Hello {invite.FullName},</p>
                <p>We’re sorry to inform you that your request to attend <strong>{invite.Event}</strong> was not approved.</p>
            ";
                }
                else
                {
                    return BadRequest("Unsupported status update.");
                }

                await _emailSender.SendInvitationEmailAsync(invite.Email, subject, htmlBody);
                return Ok(invite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the invite status: {ex.Message}");
            }
        }

    }
}
