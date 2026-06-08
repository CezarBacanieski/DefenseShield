using DefenseShield.Domain.Entities;

namespace DefenseShield.Application.Interfaces;

public interface IAlertRepository
{
    List<RiskAlert> GetAll();

    RiskAlert? GetById(Guid id);

    void Add(RiskAlert alert);

    void Update(RiskAlert alert);

    void Delete(Guid id);
}
