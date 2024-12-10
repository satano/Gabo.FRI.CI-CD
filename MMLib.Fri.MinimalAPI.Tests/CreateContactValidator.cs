using FluentAssertions;
using FluentValidation.Results;
using static MMLib.Fri.MinimalAPI.Features.Contacts.CreateContactRequest;

namespace MMLib.Fri.MinimalAPI.Tests
{
    public class CreateContactValidatorTests
    {
        [Fact]
        public void Valid()
        {
            CreateContactValidator validator = new();

            CreateContact request = new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com"
            };

            ValidationResult result = validator.Validate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Invalid()
        {
            CreateContactValidator validator = new();

            CreateContact request = new()
            {
                FirstName = "",
                LastName = "",
                Email = ""
            };

            ValidationResult result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
        }
    }
}
