using DevFreela.Domain.Enums;

namespace DevFreela.Domain.Entities;

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

    public bool Start()
    {
        if (Status == EnumProjectStatus.Created)
        {
            Status = EnumProjectStatus.InProgress;
            StartedAt = DateTime.Now;
            return true;
        }

        return false;
    }

    public void Complete()
    {
        if (Status == EnumProjectStatus.PaymentPending)
        {
            Status = EnumProjectStatus.Completed;
            CompletedAt = DateTime.Now;
        }
        
    }

    public bool SetPaymentPending()
    {
        if (Status != EnumProjectStatus.InProgress)
            return false;
        Status = EnumProjectStatus.PaymentPending;
        CompletedAt = null;
        return true;
    }

    public void Update(string title, string description, decimal totalCost)
    {
        Title = title;
        Description = description;
        TotalCost = totalCost;
    }
    
    public void AssignUsers(User client, User freelancer)
    {
        Client = client;
        Freelancer = freelancer;
    }
}