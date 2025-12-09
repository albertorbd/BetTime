using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetTime.Business;
using BetTime.Models;

namespace BetTime.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IAuthService _authService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(
        ITransactionService transactionService, 
        IAuthService authService,
        ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _authService = authService;
        _logger = logger;
    }

    
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost]
    public IActionResult CreateTransaction([FromBody] TransactionCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_authService.HasAccessToResource(dto.UserId, HttpContext.User))
            return Forbid();

        try
        {
            var transaction = _transactionService.CreateTransaction(dto);
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            return BadRequest(ex.Message);
        }
    }

    
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    public IActionResult GetAllTransactions()
    {
        try
        {
            var transactions = _transactionService.GetAllTransactions();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions");
            return BadRequest(ex.Message);
        }
    }

  
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet("{transactionId}")]
    public IActionResult GetTransactionById(int transactionId)
    {
        try
        {
            var transaction = _transactionService.GetTransactionById(transactionId);
            if (transaction == null)
                return NotFound($"Transaction with ID {transactionId} not found");

            if (!_authService.HasAccessToResource(transaction.UserId, HttpContext.User))
                return Forbid();

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction");
            return BadRequest(ex.Message);
        }
    }

    
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet("user/{userId}")]
    public IActionResult GetTransactionsByUser(int userId)
    {
        if (!_authService.HasAccessToResource(userId, HttpContext.User))
            return Forbid();

        try
        {
            var transactions = _transactionService.GetTransactionsByUser(userId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user's transactions");
            return BadRequest(ex.Message);
        }
    }
}