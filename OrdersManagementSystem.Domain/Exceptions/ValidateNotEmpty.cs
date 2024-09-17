namespace OrdersManagementSystem.Domain.Exceptions
{
    public static class GuidExtensions
    {
        public static void ValidateNotEmpty(this Guid id, string parameterName)
        {
            if (id == Guid.Empty) {
                throw new ArgumentException($"{parameterName} cannot be an empty GUID.");
            }
        }
    }
}
