namespace InvoiceTest.Services
{
    public enum ServiceActionResultCode
    {
        Unknown = 0,

        InvoiceNotFound = 10,
        InvoiceNoteNotFound = 11,

        UserNotFound = 20,

        OnlyAdminsAllowed = 100,
        OnlyOwnersAllowed = 101,

        Success = 200
    }
}
