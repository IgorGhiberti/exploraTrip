using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public partial record Email
{
    private const string PatternEmail = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";
    private Email(string emailVal) => Value = emailVal;
    public string Value { get; set; }
    public static Email Create(string value)
    {
        if (string.IsNullOrEmpty(value) || !EmailRegex().IsMatch(value))
        {
            //TODO deixar no response pattern com OneOf
            throw new Exception("E-mail inválido.");
        }

        return new Email(value);
    }
    [GeneratedRegex(PatternEmail)]
    private static partial Regex EmailRegex();

}