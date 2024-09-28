namespace iOSClub.Data.DataModels;

public interface ITodo
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public bool Status { get; set; }

    public string Id { get; set; }

    public void Update(ITodo model);
}