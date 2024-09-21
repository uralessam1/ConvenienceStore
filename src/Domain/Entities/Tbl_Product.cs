using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConvenienceStoreApi.Domain.Entities;

public class Tbl_Product : BaseAuditableEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int PK_Product { get; set; }
    public decimal Price { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(200)]
    public string Description { get; set; }

    public int Quantity { get; set; }

}