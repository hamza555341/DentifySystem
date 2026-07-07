using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Presentation.Controllers;
using Shared.DTOs.ChatBot;
using System.Net.Http.Json;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ChatBotController : ApiBaseController
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ChatBotController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Ask([FromBody] ChatBotRequestDTO request)
    {
        var aiServiceUrl = _configuration["AIServiceChatBot:BaseUrl"];
        var sessionId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var response = await _httpClient.PostAsJsonAsync($"{aiServiceUrl}/chat", new
        {
            question = request.Question,
            history = request.History ?? "",
            session_id = sessionId
        });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, error);
        }

        var result = await response.Content.ReadFromJsonAsync<ChatBotResponseDTO>();
        return Ok(result);
    }
}