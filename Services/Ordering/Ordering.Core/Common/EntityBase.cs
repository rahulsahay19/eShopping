namespace Ordering.Core.Common;

//This will serve as common fields for domain
//This means, every entity will have below props by default in ordering Microservice
public abstract class EntityBase
{
    //Protected set is made to use in the derived classes
    public int Id { get; protected set; }
    //Below Properties are Audit properties
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}