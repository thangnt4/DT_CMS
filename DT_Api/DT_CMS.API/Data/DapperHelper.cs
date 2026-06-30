using Microsoft.Data.SqlClient;

namespace DT_CMS.API.Data;

public class DapperHelper
{
    private readonly string _cs;

    public DapperHelper(IConfiguration cfg) =>
        _cs = cfg.GetConnectionString("DefaultConnection")!;

    public SqlConnection CreateConnection() => new(_cs);
}
