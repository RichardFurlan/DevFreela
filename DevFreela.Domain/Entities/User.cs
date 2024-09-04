namespace DevFreela.Domain.Entities;

public class User : BaseEntity
{
    public User(string fullName, string email, string password, DateTime birthDate) : base()
    {
        FullName = fullName;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        Active = true;


        Skills = [];
        OwnedProjects = [];
        FreelancerProjects = [];
        Comments = [];
        ProfilePictureUrl = "";
    }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime BirthDate { get; private set; }
    public bool Active { get; private set; }
    public List<UserSkill> Skills { get; private set; }
    public List<Project> OwnedProjects { get; private set; }
    public List<Project> FreelancerProjects { get; private set; }
    public List<ProjectComment> Comments { get; private set; }
    public string ProfilePictureUrl { get; private set; }
    
    public void UpdateProfilePicture(string profilePictureUrl)
    {
        ProfilePictureUrl = profilePictureUrl;
    }
}