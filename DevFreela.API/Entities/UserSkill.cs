namespace DevFreela.API.Entities;

public class UserSkill : BaseEntity
{
    public UserSkill(int idUsuario, int idSkill) : base()
    {
        IdUsuario = idUsuario;
        IdSkill = idSkill;
    }

    public int IdUsuario { get; private set; }
    public User User { get; private set; }
    public int IdSkill { get; private set; }
    public Skill Skill { get; private set; }
}