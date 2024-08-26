using DevFreela.API.Enums;

namespace DevFreela.API.Entities;

public class Project : BaseEntity
{
    public Project()
    {
        
    }
    public Project(string title, string description, int idClient, int idFreelancer, decimal totalCost) : base()
    {
        Title = title;
        Description = description;
        IdClient = idClient;
        IdFreelancer = idFreelancer;
        TotalCost = totalCost;

        Status = EnumProjectStatus.Created;
        Comments = [];
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public int IdClient { get; private set; }
    public User Client { get; private set; }
    public int IdFreelancer { get; private set; }
    public User Freelancer { get; private set; }
    public decimal TotalCost { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public EnumProjectStatus Status { get; private set; }
    public List<ProjectComment> Comments { get; private set; }

    public void Cancel()
    {
        if (Status == EnumProjectStatus.InProgress || Status == EnumProjectStatus.Suspended)
        {
            Status = EnumProjectStatus.Cancelled;
        }
    }

    public void Start()
    {
        if (Status == EnumProjectStatus.Created)
        {
            Status = EnumProjectStatus.InProgress;
            StartedAt = DateTime.Now;
        }
    }

    public void Complete()
    {
        if (Status == EnumProjectStatus.PaymentPending || Status == EnumProjectStatus.InProgress)
        {
            Status = EnumProjectStatus.Completed;
            CompletedAt = DateTime.Now;
        }
    }

    public void SetPaymentPending()
    {
        if (Status == EnumProjectStatus.InProgress)
        {
            Status = EnumProjectStatus.PaymentPending;
        }
    }

    public void Update(string title, string description, decimal totalCost)
    {
        Title = title;
        Description = description;
        TotalCost = totalCost;
    }
}