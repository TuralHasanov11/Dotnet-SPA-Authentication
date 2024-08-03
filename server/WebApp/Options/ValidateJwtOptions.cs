using Microsoft.Extensions.Options;

namespace WebApp.Options;

[OptionsValidator]
public partial class ValidateJwtOptions : IValidateOptions<JwtOptions>;