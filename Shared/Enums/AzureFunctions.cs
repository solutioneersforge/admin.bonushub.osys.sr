using System.ComponentModel;

namespace Shared.Enums
{
    public enum AzureFunctions
    {
        [Description("api/FunctionGetAdds")]
        GetAdds,
        [Description("api/FunctionAddAdd")]
        AddAdd,
        [Description("api/FunctionStoreImage")]
        StoreImage,
        [Description("api/FunctionDeleteAdd")]
        DeleteAdd,
        [Description("api/FunctionGetChangeLogs")]
        GetLogs,
        [Description("api/FunctionGetAllowedAddsRanges")]
        GetAllowedAddsRanges,
        [Description("api/FunctionUpdateAllowedAddsRange")]
        UpdateAllowedAddsRange, 
        [Description("api/FunctionGetSettingsByName")]
        GetSettingsByName,
        [Description("api/FunctionUpdateSettingsByName")]
        UpdateSettingsByName, 
        [Description("api/FunctionReorderAdds")]
        ReorderAdds,
    }
}
