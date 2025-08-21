using System.Text.RegularExpressions;
using Domain.DomainResults;

namespace Domain.ValueObjects;

public partial record Email
{
    private const string PatternEmail = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";
    private Email(string emailVal) => Value = emailVal;
    public string Value { get; set; }
    public static ResultData<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value) || !EmailRegex().IsMatch(value))
        {
            return ResultData<Email>.Error("Email inválido");
        }

        return ResultData<Email>.Success(new Email(value));
    }
    [GeneratedRegex(PatternEmail)]
    private static partial Regex EmailRegex();

}