using LoanApplications.Core;
using LoanApplications.Core.Data;
using LoanApplications.Core.Enums;

namespace LoanApplications.Tests;

public class EligibilityRuleEvaluatorTests
{
    private readonly EligibilityRuleEvaluator _evaluator = new();

    private static LoanApplication CreateApplication(
        decimal monthlyIncome = 3000,
        decimal requestedAmount = 5000,
        int termMonths = 24) => new()
    {
        Id = Guid.NewGuid(),
        Name = "John Smith",
        Email = "test@test.com",
        MonthlyIncome = monthlyIncome,
        RequestedAmount = requestedAmount,
        TermMonths = termMonths,
        Status = LoanStatus.Pending,
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public void Evaluate_ReturnsThreeEntries()
    {
        var application = CreateApplication();

        var results = _evaluator.Evaluate(application);

        Assert.Equal(3, results.Count);
        Assert.Contains(results, r => r.RuleName == LoanApplicationRule.Income);
        Assert.Contains(results, r => r.RuleName == LoanApplicationRule.RequestedAmount);
        Assert.Contains(results, r => r.RuleName == LoanApplicationRule.Term);
    }

    [Fact]
    public void Evaluate_AllRulesPass_WhenApplicationIsValid()
    {
        var application = CreateApplication(monthlyIncome: 3000, requestedAmount: 10000, termMonths: 24);

        var results = _evaluator.Evaluate(application);

        Assert.All(results, r =>
        {
            Assert.True(r.Passed);
            Assert.Null(r.Message);
        });
    }

    [Theory]
    [InlineData(2000)]
    [InlineData(5000)]
    public void Income_Passes_WhenAtLeast2000(decimal income)
    {
        var application = CreateApplication(monthlyIncome: income);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.Income);

        Assert.True(entry.Passed);
        Assert.Null(entry.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1999)]
    public void Income_Fails_WhenBelow2000(decimal income)
    {
        var application = CreateApplication(monthlyIncome: income);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.Income);

        Assert.False(entry.Passed);
        Assert.Contains($"£{income:N0}", entry.Message);
    }

    [Theory]
    [InlineData(3000, 12000)]
    [InlineData(3000, 11000)]
    public void RequestedAmount_Passes_WhenAtMostIncomeTimesFour(decimal income, decimal requested)
    {
        var application = CreateApplication(monthlyIncome: income, requestedAmount: requested);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.RequestedAmount);

        Assert.True(entry.Passed);
        Assert.Null(entry.Message);
    }

    [Fact]
    public void RequestedAmount_Fails_WhenExceedsIncomeTimesFour()
    {
        var application = CreateApplication(monthlyIncome: 3000, requestedAmount: 12001);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.RequestedAmount);

        Assert.False(entry.Passed);
        Assert.Contains("£12,000", entry.Message);
        Assert.Contains("£12,001", entry.Message);
    }

    [Theory]
    [InlineData(12)]
    [InlineData(36)]
    [InlineData(60)]
    public void Term_Passes_WhenBetween12And60(int termMonths)
    {
        var application = CreateApplication(termMonths: termMonths);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.Term);

        Assert.True(entry.Passed);
        Assert.Null(entry.Message);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(61)]
    [InlineData(0)]
    public void Term_Fails_WhenOutsideRange(int termMonths)
    {
        var application = CreateApplication(termMonths: termMonths);

        var results = _evaluator.Evaluate(application);
        var entry = results.Single(r => r.RuleName == LoanApplicationRule.Term);

        Assert.False(entry.Passed);
        Assert.Contains($"{termMonths} months", entry.Message);
    }
}
