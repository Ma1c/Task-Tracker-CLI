public class Tasks
{
    public int id { get; set; }
    public string description { get; set; }
    public string status { get; set; }
    public string createdAt { get; set; }
    public string updatedAt { get; set; }

    public override string ToString()
    {
        return $"id: {id}, description: {description}, status: {status}, created at: {createdAt}, updatedAt: {updatedAt}";
    }
}
