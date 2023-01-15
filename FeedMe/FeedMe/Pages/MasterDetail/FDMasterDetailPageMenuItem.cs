namespace FeedMe.Pages.MasterDetail;

public class FDMasterDetailPageMenuItem
{
    public FDMasterDetailPageMenuItem()
    {
        TargetType = typeof(FDMasterDetailPageDetail);
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }

    public Type TargetType { get; set; }
}